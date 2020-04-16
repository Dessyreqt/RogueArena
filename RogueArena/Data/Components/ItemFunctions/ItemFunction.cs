namespace RogueArena.Data.Components.ItemFunctions
{
    using System.Collections.Generic;
    using RogueArena.Events;
    using RogueArena.Map;

    public abstract class ItemFunction
    {
        public Entity Target { get; set; }
        public List<Entity> Entities { get; set; }
        public DungeonMap DungeonMap { get; set; }
        public int? TargetX { get; set; }
        public int? TargetY { get; set; }
        public abstract List<Event> Execute(ProgramData data);
    }
}
