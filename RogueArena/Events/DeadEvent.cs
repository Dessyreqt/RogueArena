namespace RogueArena.Events
{
    using RogueArena.Data;

    public class DeadEvent : Event
    {
        private readonly Entity _entity;

        public DeadEvent(Entity entity)
        {
            _entity = entity;
        }

        protected override void Handle(ProgramData data)
        {
            if (_entity == data.GameData.Player)
            {
                DeathFunctions.KillPlayer(_entity, data);
                data.GameData.GameState = GameState.PlayerDead;
            }
            else
            {
                DeathFunctions.KillMonster(_entity, data);
            }
        }
    }
}
