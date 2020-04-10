namespace RogueArena.Data
{
    public class ProgramData
    {
        public GameData GameData { get; set; }
        public bool ShowMainMenu { get; set; }
        public bool ShowLoadErrorMessage { get; set; }
        public bool FovRecompute { get; set; }
        public GameState PreviousGameState { get; set; }
    }
}
