namespace RogueArena.Entities
{
    using Microsoft.Xna.Framework;

    public class Entity
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public char Character { get; private set; }
        public Color Color { get; private set; }

        public Entity(int x, int y, char character, Color color)
        {
            X = x;
            Y = y;
            Character = character;
            Color = color;
        }

        public void Move(int xDelta, int yDelta)
        {
            X += xDelta;
            Y += yDelta;
        }
    }
}
