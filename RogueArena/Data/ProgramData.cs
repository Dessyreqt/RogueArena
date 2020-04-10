namespace RogueArena.Data
{
    using System.Collections.Generic;
    using RogueArena.Events;
    using SadConsole;

    public class ProgramData
    {
        public ProgramData()
        {
            Events = new List<Event>();
        }

        public GameData GameData { get; set; }
        public bool ShowMainMenu { get; set; }
        public bool ShowLoadErrorMessage { get; set; }
        public bool FovRecompute { get; set; }
        public GameState PreviousGameState { get; set; }
        public MenuManager MenuManager { get; set; }
        public Console DefaultConsole { get; set; }
        public List<Event> Events { get; set; }
    }
}
