namespace RogueArena.Events
{
    using RogueArena.Data;

    public class UnequippedEvent : Event
    {
        private readonly Entity _unequippedEntity;

        public UnequippedEvent(Entity unequippedEntity)
        {
            _unequippedEntity = unequippedEntity;
        }

        protected override void Handle(ProgramData data)
        {
            data.GameData.MessageLog.AddMessage($"You unequipped the {_unequippedEntity.Name}.");
        }
    }
}
