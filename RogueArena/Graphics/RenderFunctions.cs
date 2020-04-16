namespace RogueArena.Graphics
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Xna.Framework;
    using RogueArena.Data;
    using RogueArena.Data.Components;
    using RogueArena.Data.Lookup;
    using RogueArena.Map;
    using RogueArena.Messages;
    using SadConsole;
    using SadConsole.Input;
    using Rectangle = Microsoft.Xna.Framework.Rectangle;

    public enum RenderOrder
    {
        Stairs,
        Corpse,
        Item,
        Actor,
    }

    public static class RenderFunctions
    {
        public static void RenderBar(Console panel, int x, int y, int totalWidth, string name, int value, int maximum, Color barColor, Color backColor)
        {
            int barWidth = (int)(value / (double)maximum * totalWidth);

            panel.Fill(new Rectangle(x, y, totalWidth, 1), null, backColor, null);
            if (barWidth > 0)
            {
                panel.Fill(new Rectangle(x, y, barWidth, 1), null, barColor, null);
            }

            var text = $"{name}: {value}/{maximum}";

            panel.Print((x + totalWidth) / 2 - text.Length / 2 + 1, y, text);
        }

        public static void RenderAll(
            Console console,
            Console panel,
            List<Entity> entities,
            Entity player,
            DungeonMap dungeonMap,
            bool fovRecompute,
            MessageLog messageLog,
            int barWidth,
            MouseEventArgs mouse)
        {
            if (fovRecompute)
            {
                for (int x = 0; x < dungeonMap.Width; x++)
                {
                    for (int y = 0; y < dungeonMap.Height; y++)
                    {
                        var wall = !dungeonMap.Tiles[x, y].Transparent;
                        if (dungeonMap.Tiles[x, y].InView)
                        {
                            if (wall)
                            {
                                console.SetBackground(x, y, Colors.LightWall);
                            }
                            else
                            {
                                console.SetBackground(x, y, Colors.LightGround);
                            }

                            dungeonMap.Tiles[x, y].Explored = true;
                        }
                        else if (dungeonMap.Tiles[x, y].Explored)
                        {
                            if (wall)
                            {
                                console.SetBackground(x, y, Colors.DarkWall);
                            }
                            else
                            {
                                console.SetBackground(x, y, Colors.DarkGround);
                            }
                        }
                    }
                }
            }

            var entitiesInRenderOrder = entities.OrderBy(x => x.RenderOrder);

            foreach (var entity in entitiesInRenderOrder)
            {
                DrawEntity(console, entity, dungeonMap);
            }

            panel.Clear();

            var panelY = 1;

            foreach (var message in messageLog.Messages)
            {
                panel.Print(messageLog.X, panelY, message.Text, message.Color);
                panelY++;
            }

            RenderBar(panel, 1, 1, barWidth, "HP", player.Get<FighterComponent>().Hp, player.Get<FighterComponent>().MaxHp, Color.Red, Color.DarkRed);
            panel.Print(1, 3, $"Dungeon level: {dungeonMap.DungeonLevel}");

            panel.Print(1, 0, GetNamesUnderMouse(mouse, entities, dungeonMap));
        }

        public static void ClearAll(Console console, List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                ClearEntity(console, entity);
            }
        }

        private static string GetNamesUnderMouse(MouseEventArgs mouse, List<Entity> entities, DungeonMap dungeonMap)
        {
            var pos = mouse.MouseState.CellPosition;
            var names = entities.Where(entity => entity.X == pos.X && entity.Y == pos.Y && dungeonMap.Tiles[entity.X, entity.Y].InView).Select(entity => entity.Name);

            return string.Join(", ", names);
        }

        private static void DrawEntity(Console console, Entity entity, DungeonMap dungeonMap)
        {
            if (dungeonMap.Tiles[entity.X, entity.Y].InView || (entity.Get<StairsComponent>() != null && dungeonMap.Tiles[entity.X, entity.Y].Explored))
            {
                console.Print(entity.X, entity.Y, $"{entity.Character}", entity.Color);
            }
        }

        private static void ClearEntity(Console console, Entity entity)
        {
            console.Print(entity.X, entity.Y, " ");
        }
    }
}
