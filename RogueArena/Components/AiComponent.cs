namespace RogueArena.Components
{
    using System.Collections.Generic;
    using Map;
    using RogueArena.Data;

    public abstract class AiComponent : Component
    {
        public abstract void TakeTurn(Entity target, ProgramData data);
    }
}