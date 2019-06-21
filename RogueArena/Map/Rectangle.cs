namespace RogueArena.Map
{
    using System.Security.Cryptography.X509Certificates;

    public class Rectangle
    {
        public Rectangle(int x, int y, int width, int height)
        {
            X1 = x;
            Y1 = y;

            // I believe this part should be changed, but following tutorial for now.
            // Basically, in order to get a rectangle from (1, 1) to (6, 6) you have
            // to tell the constructor that you want a Rectangle at (1, 1) with width
            // and height 5. But if you're counting the walls as part of the width,
            // then that Rectangle should be height and width 6. Thus this should
            // probably say:
            // X2 = x + width - 1;
            // Y2 = y + height - 1;
            X2 = x + width;
            Y2 = y + height;
        }

        public int X1 { get; private set; }
        public int Y1 { get; private set; }
        public int X2 { get; private set; }
        public int Y2 { get; private set; }

        public Point Center => new Point((X1 + X2) / 2, (Y1 + Y2) / 2);

        public bool Intersects(Rectangle other)
        {
            return X1 <= other.X2 && X2 >= other.X1 && Y1 <= other.Y2 && Y2 >= other.Y1;
        }
    }
}