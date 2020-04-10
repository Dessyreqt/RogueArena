namespace RogueArena.Commands.MainMenu
{
    using RogueArena.Data;

    public class LoadSavedGameCommand : Command
    {
        protected override void Handle(ProgramData data)
        {
            data.GameData = GameData.Load();

            if (data.GameData == null)
            {
                data.ShowLoadErrorMessage = true;
            }
            else
            {
                data.PreviousGameState = data.GameData.GameState;
                data.ShowMainMenu = false;
            }
        }
    }
}
