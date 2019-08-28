namespace RogueArena.Commands
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
