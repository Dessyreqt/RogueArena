namespace RogueArena.Commands.Game
{
    using RogueArena.Data;
    using RogueArena.Data.Lookup;

    public class ShowInventoryCommand : Command
    {
        protected override void Handle(ProgramData data)
        {
            data.PreviousGameState = data.GameData.GameState;
            data.GameData.GameState = GameState.ShowInventory;
            data.MenuManager.ShowInventoryMenu(
                data.DefaultConsole,
                "Press the key next to an item to use it, or Esc to cancel.",
                data.GameData.Player,
                50,
                Constants.ScreenWidth,
                Constants.ScreenHeight);
        }
    }
}
