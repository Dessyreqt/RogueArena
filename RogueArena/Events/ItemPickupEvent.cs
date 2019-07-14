namespace RogueArena.Events
{
    public class ItemPickupEvent : Event
    {
        public ItemPickupEvent(Entity entity, Entity item)
        {
            Entity = entity;
            Item = item;
        }

        public Entity Item { get; }
        public Entity Entity { get; set; }
    }
}