namespace RogueArena.Components
{
    using ItemFunctions;
    using RogueArena.Events;

    public class ItemComponent
    {
        public ItemComponent()
        {
            // here for deserialization purposes
        }

        public ItemComponent(ItemFunction itemFunction = null, bool targeting = false, Message targetingMessage = null)
        {
            ItemFunction = itemFunction;
            Targeting = targeting;
            TargetingMessage = targetingMessage;
        }

        public ItemFunction ItemFunction { get; set; }
        public Entity Owner { get; set; }
        public bool Targeting { get; set; }
        public Message TargetingMessage { get; set; }
    }
}