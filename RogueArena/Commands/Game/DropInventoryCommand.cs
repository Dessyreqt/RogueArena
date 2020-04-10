namespace RogueArena.Commands.Game
{
    using RogueArena.Data;

    public class DropInventoryCommand : Command
    {
        protected override void Handle(ProgramData data)
        {
            data.PreviousGameState = data.GameData.GameState;
            data.GameData.GameState = GameState.DropInventory;
            data.MenuManager.ShowInventoryMenu(
                data.DefaultConsole,
                "Press the key next to an item to drop it, or Esc to cancel.",
                data.GameData.Player,
                50,
                Constants.ScreenWidth,
                Constants.ScreenHeight);
        }
    }
}
