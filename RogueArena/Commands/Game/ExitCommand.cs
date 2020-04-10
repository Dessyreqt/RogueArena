namespace RogueArena.Commands.Game
{
    using RogueArena.Data;
    using RogueArena.Events;
    using SadConsole;

    public class ExitCommand : Command
    {
        protected override void Handle(ProgramData data)
        {
            if (data.GameData.GameState == GameState.ShowInventory || data.GameData.GameState == GameState.DropInventory)
            {
                data.GameData.GameState = data.PreviousGameState;
                data.MenuManager.HideInventoryMenu(data.DefaultConsole);
            }
            else if (data.GameData.GameState == GameState.Targeting)
            {
                data.Events.Add(new TargetingCanceledEvent());
            }
            else if (data.GameData.GameState == GameState.CharacterScreen)
            {
                data.GameData.GameState = data.PreviousGameState;
                data.MenuManager.HideCharacterScreen(data.DefaultConsole);
            }
            else
            {
                data.GameData.Save();
                Game.Instance.Exit();
            }
        }
    }
}
