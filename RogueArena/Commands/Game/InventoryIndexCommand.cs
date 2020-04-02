namespace RogueArena.Commands.Game
{
    public class InventoryIndexCommand : Command
    {
        public int Index { get; }

        public InventoryIndexCommand(int index)
        {
            Index = index;
        }
    }
}
