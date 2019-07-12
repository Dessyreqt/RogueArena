namespace RogueArena.Events
{
    using Microsoft.Xna.Framework;

    public class MessageEvent : Event
    {
        public MessageEvent(string message)
            : this(message, Color.White)
        {
        }

        public MessageEvent(string message, Color color)
        {
            Message = new Message(message, color);
        }

        public Message Message { get; }
    }
}