namespace RogueArena.Events
{
    using Microsoft.Xna.Framework;
    using RogueArena.Data;
    using RogueArena.Messages;

    public class ItemConsumedEvent : Event
    {
        private readonly Message _message;
        private readonly Entity _target;

        public ItemConsumedEvent(string message, Entity target = null)
            : this(message, Color.White, target)
        {
        }

        public ItemConsumedEvent(string message, Color color, Entity target = null)
        {
            _message = new Message(message, color);
            _target = target;
        }

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

            data.GameData.MessageLog.AddMessage(_message);
        }
    }
}
