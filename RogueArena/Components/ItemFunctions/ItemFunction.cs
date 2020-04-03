namespace RogueArena.Components.ItemFunctions
{
    using System.Collections.Generic;
    using Events;
    using RogueArena.Map;

    public abstract class ItemFunction
    {
        public Entity Target { get; set; }
        public List<Entity> Entities { get; set; }
        public DungeonMap DungeonMap { get; set; }
        public abstract List<Event> Execute();
        public int? TargetX { get; set; }
        public int? TargetY { get; set; }
    }
}