namespace RogueArena.Components
{
    using System.Collections.Generic;
    using Map;

    public abstract class AiComponent : Component
    {
        public abstract void TakeTurn(Entity target, GameMap gameMap, List<Entity> entities);
    }
}