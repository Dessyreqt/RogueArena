namespace RogueArena
{
    using System.Collections.Generic;
    using System.Linq;
    using Events;
    using Map;
    using Microsoft.Xna.Framework;
    using SadConsole;
    using SadConsole.Input;
    using Rectangle = Microsoft.Xna.Framework.Rectangle;

    public class RenderOrder
    {
        public const int Corpse = 1;
        public const int Item = 2;
        public const int Actor = 3;
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
            GameMap gameMap,
            bool fovRecompute,
            MessageLog messageLog,
            Dictionary<string, Color> colors,
            int barWidth,
            MouseEventArgs mouse)
        {
            if (fovRecompute)
            {
                for (int x = 0; x < gameMap.Width; x++)
                {
                    for (int y = 0; y < gameMap.Height; y++)
                    {
                        var wall = !gameMap.Tiles[x, y].Transparent;
                        if (gameMap.Tiles[x, y].InView)
                        {
                            if (wall)
                            {
                                console.SetBackground(x, y, colors["light_wall"]);
                            }
                            else
                            {
                                console.SetBackground(x, y, colors["light_ground"]);
                            }

                            gameMap.Tiles[x, y].Explored = true;
                        }
                        else if (gameMap.Tiles[x, y].Explored)
                        {
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
                }
            }

            var entitiesInRenderOrder = entities.OrderBy(x => x.RenderOrder);

            foreach (var entity in entitiesInRenderOrder)
            {
                DrawEntity(console, entity, gameMap);
            }

            panel.Clear();

            var panelY = 1;

            foreach (var message in messageLog.Messages)
            {
                panel.Print(messageLog.X, panelY, message.Text, message.Color);
                panelY++;
            }

            RenderBar(panel, 1, 1, barWidth, "HP", player.Fighter.Hp, player.Fighter.MaxHp, Color.Red, Color.DarkRed);

            panel.Print(1, 0, GetNamesUnderMouse(mouse, entities, gameMap));
        }

        public static void ClearAll(Console console, List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                ClearEntity(console, entity);
            }
        }

        public static void RenderLog(Console console, List<string> messageLog, int startingLine, int count)
        {
            var lineNo = 0;

            for (var index = messageLog.Count - 1; index >= 0; index--)
            {
                var log = messageLog[index];
                console.Clear(0, startingLine + lineNo, console.Width);
                console.Print(0, startingLine + lineNo, log);

                lineNo++;
                if (lineNo >= count)
                {
                    break;
                }
            }
        }

        private static string GetNamesUnderMouse(MouseEventArgs mouse, List<Entity> entities, GameMap gameMap)
        {
            var pos = mouse.MouseState.CellPosition;
            var names = entities.Where(entity => entity.X == pos.X && entity.Y == pos.Y && gameMap.Tiles[entity.X, entity.Y].InView).Select(entity => entity.Name);

            return string.Join(", ", names);
        }

        private static void DrawEntity(Console console, Entity entity, GameMap gameMap)
        {
            if (gameMap.Tiles[entity.X, entity.Y].InView)
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