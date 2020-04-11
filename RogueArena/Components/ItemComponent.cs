namespace RogueArena.Components
{
    using RogueArena.Components.ItemFunctions;
    using RogueArena.Messages;

    public class ItemComponent : Component
    {
        public ItemFunction ItemFunction { get; set; }
        public bool Targeting { get; set; }
        public Message TargetingMessage { get; set; }
    }
}
