namespace RogueArena
{
    using System.Collections.Generic;
    using SadConsole;

    public static class RenderFunctions
    {
        public static void RenderAll(Console console, List<Entity> entities)
        {
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
