namespace RogueArena.Components.ItemFunctions
{
    using System.Collections.Generic;
    using Events;

    public abstract class ItemFunction
    {
        public Entity Target { get; set; }
        public abstract List<Event> Execute();
    }
}