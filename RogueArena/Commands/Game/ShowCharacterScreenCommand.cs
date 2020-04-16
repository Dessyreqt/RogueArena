namespace RogueArena.Commands.Game
{
    using RogueArena.Data;
    using RogueArena.Data.Lookup;

    public class ShowCharacterScreenCommand : Command
    {
        protected override void Handle(ProgramData data)
        {
            data.PreviousGameState = data.GameData.GameState;
            data.GameData.GameState = GameState.CharacterScreen;
            data.MenuManager.ShowCharacterScreen(data.DefaultConsole, data.GameData.Player, 30, 10, Constants.ScreenWidth, Constants.ScreenHeight);
        }
    }
}
