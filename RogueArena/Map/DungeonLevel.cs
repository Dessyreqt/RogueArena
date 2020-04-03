namespace RogueArena.Map
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Xna.Framework;
    using RogueArena.Data;
    using RogueArena.Events;

    public class DungeonLevel
    {
        public DungeonLevel()
        {
            // here for deserialization purposes
        }

        public DungeonLevel(int levelNumber, int width, int height)
        {
            LevelNumber = levelNumber;
            Map = new DungeonMap(levelNumber, width, height);
            Entities = new List<Entity>();
        }

        public int LevelNumber { get; set; }
        public List<Entity> Entities { get; set; }
        public DungeonMap Map { get; set; }
        public DungeonLevel PreviousFloor { get; set; }
        public DungeonLevel NextFloor { get; set; }
        
        public DungeonLevel GoToFloor(int floor, Entity player, MessageLog messageLog)
        {
            if (floor > LevelNumber)
            {
                Entities.Remove(player);

                if (NextFloor == null)
                {
                    NextFloor = new DungeonLevel(LevelNumber + 1, Constants.MapWidth, Constants.MapHeight);
                    NextFloor.Map.Tiles = NextFloor.Map.InitializeTiles();
                    NextFloor.Map.MakeMap(
                        Constants.MaxRooms,
                        Constants.MinRoomSize,
                        Constants.MaxRoomSize,
                        Constants.MapWidth,
                        Constants.MapHeight,
                        player,
                        NextFloor.Entities,
                        Constants.MaxMonstersPerRoom,
                        Constants.MaxItemsPerRoom);
                    NextFloor.PreviousFloor = this;

                    player.FighterComponent.Heal(player.FighterComponent.MaxHp / 2);

                    messageLog.AddMessage("You take a moment to rest, and recover your strength.", Color.Violet);
                }
                else
                {
                    Entity stairsEntity = NextFloor.Entities.Single(entity => entity.StairsComponent != null && entity.StairsComponent?.ToFloor == LevelNumber);

                    player.X = stairsEntity.X;
                    player.Y = stairsEntity.Y;

                    NextFloor.Entities.Add(player);
                }

                return NextFloor;
            }
            else if (floor < LevelNumber)
            {
                if (PreviousFloor == null)
                {
                    return null;
                }
                else
                {
                    Entities.Remove(player);
                    Entity stairsEntity = PreviousFloor.Entities.Single(entity => entity.StairsComponent != null && entity.StairsComponent?.ToFloor == LevelNumber);

                    player.X = stairsEntity.X;
                    player.Y = stairsEntity.Y;

                    PreviousFloor.Entities.Add(player);
                    return PreviousFloor;
                }
            }

            return this;
        }


    }
}
