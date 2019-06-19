namespace RogueArena
{
    using System.Collections.Generic;
    using Map;
    using Microsoft.Xna.Framework;
    using SadConsole;

    public static class RenderFunctions
    {
        public static void RenderAll(Console console, List<Entity> entities, GameMap gameMap, Dictionary<string, Color> colors)
        {
            for (int x = 0; x < gameMap.Width; x++)
            {
                for (int y = 0; y < gameMap.Height; y++)
                {
                    var wall = gameMap.Tiles[x, y].BlockSight;

                    if (wall)
                    {
                        console.SetBackground(x, y, colors["dark_wall"]);
                    }
                    else
                    {
                        console.SetBackground(x, y, colors["dark_ground"]);
                    }
                }
            }

            foreach (var entity in entities)
            {
                DrawEntity(console, entity);
            }
        }

        public static void ClearAll(Console console, List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                ClearEntity(console, entity);
            }
        }

        private static void DrawEntity(Console console, Entity entity)
        {
            console.Print(entity.X, entity.Y, $"{entity.Character}", entity.Color);
        }

        private static void ClearEntity(Console console, Entity entity)
        {
            console.Clear(entity.X, entity.Y);
        }
    }
}
