namespace RogueArena
{
    using Microsoft.Xna.Framework;

    public class Entity
    {
        public Entity(int x, int y, char character, Color color)
        {
            X = x;
            Y = y;
            Character = character;
            Color = color;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public char Character { get; private set; }
        public Color Color { get; private set; }

        public void Move(int xDelta, int yDelta)
        {
            X += xDelta;
            Y += yDelta;
        }
    }
}