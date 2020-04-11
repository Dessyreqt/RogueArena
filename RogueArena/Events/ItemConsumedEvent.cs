namespace RogueArena.Events
{
    using RogueArena.Data;

    public class ItemConsumedEvent : Event
    {
        protected override void Handle(ProgramData data)
        {
            if (data.GameData.GameState == GameState.LevelUp)
            {
                data.PreviousGameState = GameState.EnemyTurn;
            }
            else
            {
                data.GameData.GameState = GameState.EnemyTurn;
            }
        }
    }
}
