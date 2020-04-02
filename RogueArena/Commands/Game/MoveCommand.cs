namespace RogueArena.Commands.Game
{
    public class MoveCommand : Command
    {
        public MoveCommand(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }
    }
}