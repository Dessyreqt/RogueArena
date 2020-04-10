namespace RogueArena.Commands.MainMenu
{
    using RogueArena.Data;

    public class NewGameCommand : Command
    {
        protected override void Handle(ProgramData data)
        {
            data.GameData = GameData.New();
            data.PreviousGameState = data.GameData.GameState;
            data.ShowMainMenu = false;
        }
    }
}
