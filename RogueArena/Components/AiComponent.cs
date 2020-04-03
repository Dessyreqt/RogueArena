namespace RogueArena.Components
{
    using System.Collections.Generic;
    using Map;

    public abstract class AiComponent
    {
        public Entity Owner { get; set; }

        public abstract void TakeTurn(Entity target, GameMap gameMap, List<Entity> entities);
    }
}