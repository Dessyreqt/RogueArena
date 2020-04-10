namespace RogueArena.Commands.Game
{
    using RogueArena.Data;

    public class RestCommand : Command
    {
        protected override void Handle(ProgramData data)
        {
            if (data.GameData.GameState == GameState.PlayersTurn)
            {
                data.GameData.GameState = GameState.EnemyTurn;
            }
        }
    }
}
