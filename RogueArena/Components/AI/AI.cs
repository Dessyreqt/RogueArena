namespace RogueArena.Components.AI
{
    using System.Collections.Generic;
    using Map;

    public abstract class AI
    {
        public Entity Owner { get; set; }

        public abstract void TakeTurn(Entity target, GameMap gameMap, List<Entity> entities);
    }
}