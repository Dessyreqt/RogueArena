namespace RogueArena.Data.Components
{
    public abstract class AiComponent : Component
    {
        public abstract void TakeTurn(Entity target, ProgramData data);
    }
}
