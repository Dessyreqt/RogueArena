namespace RogueArena
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Xna.Framework;

    public class Entity
    {
        public Entity(int x, int y, char character, Color color, string name, bool blocks =false)
        {
            X = x;
            Y = y;
            Character = character;
            Color = color;
            Name = name;
            Blocks = blocks;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public char Character { get; private set; }
        public Color Color { get; private set; }
        public string Name { get; private set; }
        public bool Blocks { get; private set; }

        public void Move(int xDelta, int yDelta)
        {
            X += xDelta;
            Y += yDelta;
        }

        public static Entity GetBlockingEntityAtLocation(List<Entity> entities, int destX, int destY)
        {
            return entities.FirstOrDefault(entity => entity.Blocks && entity.X == destX && entity.Y == destY);
        }
    }
}