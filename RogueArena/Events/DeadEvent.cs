namespace RogueArena.Events
{
    public class DeadEvent : Event
    {
        public DeadEvent(Entity entity)
        {
            Entity = entity;
        }

        public Entity Entity { get; }
    }
}