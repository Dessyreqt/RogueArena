namespace RogueArena.Map
{
    using System;
    using System.Collections.Generic;

    public class GameMap
    {
        private readonly Random _random;

        public GameMap(int width, int height, Random random)
        {
            Width = width;
            Height = height;
            _random = random;
            Tiles = InitializeTiles();
        }

        public int Width { get; private set; }
        public int Height { get; private set; }
        public Tile[,] Tiles { get; private set; }

        public Tile[,] InitializeTiles()
        {
            var tiles = new Tile[Width, Height];

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    tiles[x, y] = new Tile(true);
                }
            }

            return tiles;
        }

        public void MakeMap(int maxRooms, int minRoomSize, int maxRoomSize, int mapWidth, int mapHeight, Entity player)
        {
            var rooms = new List<Rectangle>();

            for (int i = 0; i < maxRooms; i++)
            {
                var width = _random.Next(minRoomSize, maxRoomSize);
                var height = _random.Next(minRoomSize, maxRoomSize);
                var x = _random.Next(mapWidth - width - 1);
                var y = _random.Next(mapHeight - height - 1);
                var newRoom = new Rectangle(x, y, width, height);

                foreach (var otherRoom in rooms)
                {
                    if (newRoom.Intersects(otherRoom))
                    {
                        break;
                    }
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

                    if (_random.Next(2) == 1)
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

                rooms.Add(newRoom);
            }
        }

        public bool IsBlocked(int x, int y)
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height)
            {
                return true;
            }

            if (Tiles[x, y].Blocked)
            {
                return true;
            }

            return false;
        }

        private void CreateRoom(Rectangle room)
        {
            for (int x = room.X1 + 1; x < room.X2; x++)
            {
                for (int y = room.Y1 + 1; y < room.Y2; y++)
                {
                    Tiles[x, y].Blocked = false;
                    Tiles[x, y].BlockSight = false;
                }
            }
        }

        private void CreateHorizontalTunnel(int x1, int x2, int y)
        {
            for (int x = Math.Min(x1, x2); x < Math.Max(x1, x2) + 1; x++)
            {
                Tiles[x, y].Blocked = false;
                Tiles[x, y].BlockSight = false;
            }
        }

        private void CreateVerticalTunnel(int y1, int y2, int x)
        {
            for (int y = Math.Min(y1, y2); y < Math.Max(y1, y2) + 1; y++)
            {
                Tiles[x, y].Blocked = false;
                Tiles[x, y].BlockSight = false;
            }
        }
    }
}