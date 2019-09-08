namespace RogueArena.Events
{
    public class ItemDroppedEvent : Event
    {
        public ItemDroppedEvent(Entity entity, Entity item)
        {
            Entity = entity;
            Item = item;
        }

        public Entity Item { get; }
        public Entity Entity { get; set; }
    }
}