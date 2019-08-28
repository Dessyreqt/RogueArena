namespace RogueArena.Events
{
    using Microsoft.Xna.Framework;

    public class ItemConsumedEvent : Event
    {
        public ItemConsumedEvent(string message)
            : this(message, Color.White)
        {
        }

        public ItemConsumedEvent(string message, Color color)
        {
            Message = new Message(message, color);
        }

        public Message Message { get; }
    }
}
