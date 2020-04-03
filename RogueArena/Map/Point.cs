namespace RogueArena.Map
{
    public class Point
    {
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; private set; }
        public int Y { get; private set; }

        public void Set(Point otherPoint)
        {
            X = otherPoint.X;
            Y = otherPoint.Y;
        }
    }
}