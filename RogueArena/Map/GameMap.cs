﻿namespace RogueArena.Map
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using RogueArena.Data;
    using RogueArena.Rng;

    [Serializable]
    public class GameMap
    {
        public const int FovBasic = 0;

        public GameMap()
        {
            // here for deserialization purposes
        }

        public GameMap(int width, int height)
        {
            Width = width;
            Height = height;
            Tiles = InitializeTiles();
        }

        public int Width { get; set; }
        public int Height { get; set; }
        public Tile[,] Tiles { get; set; }

        public Tile[,] InitializeTiles()
        {
            var tiles = new Tile[Width, Height];

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    tiles[x, y] = new Tile(false);
                }
            }

            return tiles;
        }

        public void MakeMap(
            int maxRooms,
            int minRoomSize,
            int maxRoomSize,
            int mapWidth,
            int mapHeight,
            Entity player,
            List<Entity> entities,
            int maxMonstersPerRoom,
            int maxItemsPerRoom)
        {
            var rooms = new List<Rectangle>();

            for (int i = 0; i < maxRooms; i++)
            {
                var width = Rng.Next(minRoomSize, maxRoomSize);
                var height = Rng.Next(minRoomSize, maxRoomSize);
                var x = Rng.Next(mapWidth - width - 1);
                var y = Rng.Next(mapHeight - height - 1);
                var newRoom = new Rectangle(x, y, width, height);

                if (rooms.Any(room => newRoom.Intersects(room)))
                {
                    continue;
                }

                CreateRoom(newRoom);
                var newCenter = newRoom.Center;

                if (rooms.Count == 0)
                {
                    player.X = newCenter.X;
                    player.Y = newCenter.Y;
                }
                else
                {
                    var prevCenter = rooms[rooms.Count - 1].Center;

                    if (Rng.Next(2) == 1)
                    {
                        CreateHorizontalTunnel(prevCenter.X, newCenter.X, prevCenter.Y);
                        CreateVerticalTunnel(prevCenter.Y, newCenter.Y, newCenter.X);
                    }
                    else
                    {
                        CreateVerticalTunnel(prevCenter.Y, newCenter.Y, prevCenter.X);
                        CreateHorizontalTunnel(prevCenter.X, newCenter.X, newCenter.Y);
                    }
                }

                PlaceEntities(newRoom, entities, maxMonstersPerRoom, maxItemsPerRoom);

                rooms.Add(newRoom);
            }
        }

        public bool IsBlocked(int x, int y)
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height)
            {
                return true;
            }

            if (!Tiles[x, y].Walkable)
            {
                return true;
            }

            return false;
        }

        public void ComputeFov(int playerX, int playerY, int maxRadius, bool lightWalls, int algorithm)
        {
            switch (algorithm)
            {
                case FovBasic:
                    ComputeFovCircularRaycasting(playerX, playerY, maxRadius, lightWalls);
                    break;
                default:
                    return;
            }
        }

        private void PlaceEntities(Rectangle room, List<Entity> entities, int maxMonstersPerRoom, int maxItemsPerRoom)
        {
            var numMonsters = Rng.Next(maxMonstersPerRoom);
            var numItems = Rng.Next(maxItemsPerRoom);

            for (int i = 0; i < numMonsters; i++)
            {
                var x = Rng.Next(room.X1 + 1, room.X2 - 1);
                var y = Rng.Next(room.Y1 + 1, room.Y2 - 1);

                if (!entities.Any(entity => entity.X == x && entity.Y == y))
                {
                    Entity monster;

                    if (Rng.Next(100) < 80)
                    {
                        monster = Monsters.Get(Monsters.Orc, x, y);
                    }
                    else
                    {
                        monster = Monsters.Get(Monsters.Troll, x, y);
                    }

                    entities.Add(monster);
                }
            }

            for (int i = 0; i < numItems; i++)
            {
                var x = Rng.Next(room.X1 + 1, room.X2 - 1);
                var y = Rng.Next(room.Y1 + 1, room.Y2 - 1);

                if (!entities.Any(entity => entity.X == x && entity.Y == y))
                {
                    var itemChance = Rng.Next(100);

                    if (itemChance < 70)
                    {
                        var item = Items.Get(Items.HealingPotion, x, y);
                        entities.Add(item);
                    }
                    else if (itemChance < 80)
                    {
                        var item = Items.Get(Items.FireballScroll, x, y);
                        entities.Add(item);
                    }
                    else if (itemChance < 90)
                    {
                        var item = Items.Get(Items.ConfusionScroll, x, y);
                        entities.Add(item);
                    }
                    else
                    {
                        var item = Items.Get(Items.LightningScroll, x, y);
                        entities.Add(item);
                    }
                }
            }
        }

        private void ComputeFovCircularRaycasting(int playerX, int playerY, int maxRadius, bool lightWalls)
        {
            int xMin = 0;
            int yMin = 0;
            int xMax = Width;
            int yMax = Height;
            int radiusSquared = maxRadius * maxRadius;

            int xOrig;
            int yOrig;

            if (maxRadius > 0)
            {
                xMin = Math.Max(0, playerX - maxRadius);
                yMin = Math.Max(0, playerY - maxRadius);
                xMax = Math.Min(Width, playerX + maxRadius + 1);
                yMax = Math.Min(Height, playerY + maxRadius + 1);
            }

            foreach (var tile in Tiles)
            {
                tile.InView = false;
            }

            xOrig = xMin;
            yOrig = yMin;

            while (xOrig < xMax)
            {
                CastRay(playerX, playerY, xOrig++, yOrig, radiusSquared, lightWalls);
            }

            xOrig = xMax - 1;
            yOrig = yMin + 1;

            while (yOrig < yMax)
            {
                CastRay(playerX, playerY, xOrig, yOrig++, radiusSquared, lightWalls);
            }

            xOrig = xMax - 2;
            yOrig = yMax - 1;

            while (xOrig > 0)
            {
                CastRay(playerX, playerY, xOrig--, yOrig, radiusSquared, lightWalls);
            }

            xOrig = xMin;
            yOrig = yMax - 2;

            while (yOrig > 0)
            {
                CastRay(playerX, playerY, xOrig, yOrig--, radiusSquared, lightWalls);
            }

            if (lightWalls)
            {
                PostProcess(xMin, yMin, playerX, playerY, -1, -1);
                PostProcess(playerX, yMin, xMax - 1, playerY, 1, -1);
                PostProcess(xMin, playerY, playerX, yMax - 1, -1, 1);
                PostProcess(playerX, playerY, xMax - 1, yMax - 1, 1, 1);
            }
        }

        private void PostProcess(int x0, int y0, int x1, int y1, int dx, int dy)
        {
            int cx;
            int cy;
            for (cx = x0; cx <= x1; cx++)
            {
                for (cy = y0; cy <= y1; cy++)
                {
                    int x2 = cx + dx;
                    int y2 = cy + dy;
                    if (cx < Width && cy < Height && Tiles[cx, cy].InView && Tiles[cx, cy].Transparent)
                    {
                        if (x2 >= x0 && x2 <= x1)
                        {
                            if (x2 < Width && cy < Height && !Tiles[x2, cy].Transparent)
                            {
                                Tiles[x2, cy].InView = true;
                            }
                        }

                        if (y2 >= y0 && y2 <= y1)
                        {
                            if (cx < Width && y2 < Height && !Tiles[cx, y2].Transparent)
                            {
                                Tiles[cx, y2].InView = true;
                            }
                        }

                        if (x2 >= x0 && x2 <= x1 && y2 >= y0 && y2 <= y1)
                        {
                            if (x2 < Width && y2 < Height && !Tiles[x2, y2].Transparent)
                            {
                                Tiles[x2, y2].InView = true;
                            }
                        }
                    }
                }
            }
        }

        private void CastRay(int xOrig, int yOrig, int xDest, int yDest, int radiusSquared, bool lightWalls)
        {
            int xCur = xOrig;
            int yCur = yOrig;
            bool inMap = false;
            bool blocked = false;
            bool end = false;

            var lineData = new BresenhamLineData();
            InitLineData(xOrig, yOrig, xDest, yDest, lineData);

            if (xCur.Between(0, Width) && yCur.Between(0, Height))
            {
                inMap = true;
                Tiles[xCur, yCur].InView = true;
            }

            while (!end)
            {
                end = StepLineData(ref xCur, ref yCur, lineData);

                if (radiusSquared > 0)
                {
                    int curRadius = (xCur - xOrig) * (xCur - xOrig) + (yCur - yOrig) * (yCur - yOrig);

                    if (curRadius > radiusSquared)
                    {
                        return;
                    }
                }

                if (xCur.Between(0, Width) && yCur.Between(0, Height))
                {
                    inMap = true;
                    if (!blocked && !Tiles[xCur, yCur].Transparent)
                    {
                        blocked = true;
                    }
                    else if (blocked)
                    {
                        return;
                    }

                    if (lightWalls || !blocked)
                    {
                        Tiles[xCur, yCur].InView = true;
                    }
                }
                else if (inMap)
                {
                    return;
                }
            }
        }

        private bool StepLineData(ref int xCur, ref int yCur, BresenhamLineData data)
        {
            if (data.StepX * data.DeltaX > data.StepY * data.DeltaY)
            {
                if (data.OrigX == data.DestX)
                {
                    return true;
                }

                data.OrigX += data.StepX;
                data.E -= data.StepY * data.DeltaY;

                if (data.E < 0)
                {
                    data.OrigY += data.StepY;
                    data.E += data.StepX * data.DeltaX;
                }
            }
            else
            {
                if (data.OrigY == data.DestY)
                {
                    return true;
                }

                data.OrigY += data.StepY;
                data.E -= data.StepX * data.DeltaX;

                if (data.E < 0)
                {
                    data.OrigX += data.StepX;
                    data.E += data.StepY * data.DeltaY;
                }
            }

            xCur = data.OrigX;
            yCur = data.OrigY;

            return false;
        }

        private void InitLineData(int xOrig, int yOrig, int xDest, int yDest, BresenhamLineData data)
        {
            data.OrigX = xOrig;
            data.OrigY = yOrig;
            data.DestX = xDest;
            data.DestY = yDest;
            data.DeltaX = xDest - xOrig;
            data.DeltaY = yDest - yOrig;

            if (data.DeltaX > 0)
            {
                data.StepX = 1;
            }
            else if (data.DeltaX < 0)
            {
                data.StepX = -1;
            }
            else
            {
                data.StepX = 0;
            }

            if (data.DeltaY > 0)
            {
                data.StepY = 1;
            }
            else if (data.DeltaY < 0)
            {
                data.StepY = -1;
            }
            else
            {
                data.StepY = 0;
            }

            if (data.StepX * data.DeltaX > data.StepY * data.DeltaY)
            {
                data.E = data.StepX * data.DeltaX;
                data.DeltaX *= 2;
                data.DeltaY *= 2;
            }
            else
            {
                data.E = data.StepY * data.DeltaY;
                data.DeltaX *= 2;
                data.DeltaY *= 2;
            }
        }

        private void CreateRoom(Rectangle room)
        {
            for (int x = room.X1 + 1; x < room.X2; x++)
            {
                for (int y = room.Y1 + 1; y < room.Y2; y++)
                {
                    Tiles[x, y].Walkable = true;
                    Tiles[x, y].Transparent = true;
                }
            }
        }

        private void CreateHorizontalTunnel(int x1, int x2, int y)
        {
            for (int x = Math.Min(x1, x2); x < Math.Max(x1, x2) + 1; x++)
            {
                Tiles[x, y].Walkable = true;
                Tiles[x, y].Transparent = true;
            }
        }

        private void CreateVerticalTunnel(int y1, int y2, int x)
        {
            for (int y = Math.Min(y1, y2); y < Math.Max(y1, y2) + 1; y++)
            {
                Tiles[x, y].Walkable = true;
                Tiles[x, y].Transparent = true;
            }
        }
    }

    internal static class IntExtensions
    {
        public static bool Between(this int comp, int min, int max)
        {
            return min <= comp && comp < max;
        }
    }
}
