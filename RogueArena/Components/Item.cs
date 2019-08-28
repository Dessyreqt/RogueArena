namespace RogueArena.Components
{
    using ItemFunctions;

    public class Item
    {
        public Item(ItemFunction itemFunction = null)
        {
            ItemFunction = itemFunction;
        }

        public ItemFunction ItemFunction { get; set; }
        public Entity Owner { get; set; }
    }
}