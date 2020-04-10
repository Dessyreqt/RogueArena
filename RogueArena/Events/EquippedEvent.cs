namespace RogueArena.Events
{
    using RogueArena.Data;

    public class EquippedEvent : Event
    {
        private readonly Entity _equippedEntity;

        public EquippedEvent(Entity equippedEntity)
        {
            _equippedEntity = equippedEntity;
        }

        protected override void Handle(ProgramData data)
        {
            data.GameData.MessageLog.AddMessage($"You equipped the {_equippedEntity.Name}.");
        }
    }
}
