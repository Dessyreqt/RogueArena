namespace RogueArena.Events
{
    using Microsoft.Xna.Framework;
    using RogueArena.Messages;

    public class ItemConsumedEvent : Event
    {
        public ItemConsumedEvent(string message, Entity target = null)
            : this(message, Color.White, target)
        {
        }

        public ItemConsumedEvent(string message, Color color, Entity target = null)
        {
            Message = new Message(message, color);
            Target = target;
        }

        public Message Message { get; }
        public Entity Target { get; }
    }
}
