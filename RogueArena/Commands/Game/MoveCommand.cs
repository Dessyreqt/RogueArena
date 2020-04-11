namespace RogueArena.Commands.Game
{
    using RogueArena.Components;
    using RogueArena.Data;

    public class MoveCommand : Command
    {
        private readonly int _x;
        private readonly int _y;

        public MoveCommand(int x, int y)
        {
            _x = x;
            _y = y;
        }

        protected override void Handle(ProgramData data)
        {
            data.DefaultConsole.Clear(0, 45, 80);

            if (data.GameData.GameState == GameState.PlayersTurn)
            {
                var destX = data.GameData.Player.X + _x;
                var destY = data.GameData.Player.Y + _y;

                if (!data.GameData.DungeonLevel.Map.IsBlocked(destX, destY))
                {
                    var target = Entity.GetBlockingEntityAtLocation(data.GameData.Entities, destX, destY);

                    if (target != null)
                    {
                        data.GameData.Player.Get<FighterComponent>().Attack(target, data);
                    }
                    else
                    {
                        data.GameData.Player.Move(_x, _y);
                        data.FovRecompute = true;
                    }

                    data.GameData.GameState = GameState.EnemyTurn;
                }
            }
        }
    }
}
