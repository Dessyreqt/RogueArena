namespace RogueArena.Commands.Game
{
    using RogueArena.Components;
    using RogueArena.Data;

    public class InventoryIndexCommand : Command
    {
        private readonly int _index;

        public InventoryIndexCommand(int index)
        {
            _index = index;
        }

        protected override void Handle(ProgramData data)
        {
            var inventory = data.GameData.Player.Get<InventoryComponent>();

            if (data.PreviousGameState != GameState.PlayerDead && _index < inventory.Items.Count)
            {
                var item = inventory.Items[_index];

                if (data.GameData.GameState == GameState.ShowInventory)
                {
                    inventory.Use(item, data);
                }
                else if (data.GameData.GameState == GameState.DropInventory)
                {
                    inventory.Drop(item, data);
                }
            }
        }
    }
}
