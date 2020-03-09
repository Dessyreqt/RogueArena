namespace RogueArena.Events
{
    public class TargetingStartEvent : Event
    {
        public TargetingStartEvent(Entity itemEntity)
        {
            ItemEntity = itemEntity;
        }

        public Entity ItemEntity { get; set; }
    }
}
