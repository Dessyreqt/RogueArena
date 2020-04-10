namespace RogueArena.Commands.Game
{
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
            if (data.PreviousGameState != GameState.PlayerDead && _index < data.GameData.Player.InventoryComponent.Items.Count)
            {
                var item = data.GameData.Player.InventoryComponent.Items[_index];

                if (data.GameData.GameState == GameState.ShowInventory)
                {
                    data.GameData.Player.InventoryComponent.Use(item, data.GameData.Entities, data.GameData.DungeonLevel.Map);
                }
                else if (data.GameData.GameState == GameState.DropInventory)
                {
                    data.GameData.Player.InventoryComponent.Drop(item);
                }
            }
        }
    }
}
