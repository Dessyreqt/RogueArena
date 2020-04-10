namespace RogueArena.Events
{
    using RogueArena.Data;

    public class TargetingCanceledEvent : Event
    {
        protected override void Handle(ProgramData data)
        {
            data.GameData.GameState = data.PreviousGameState;
            data.GameData.MessageLog.AddMessage("Targeting canceled.");
        }
    }
}
