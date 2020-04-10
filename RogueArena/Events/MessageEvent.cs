namespace RogueArena.Events
{
    using Microsoft.Xna.Framework;
    using RogueArena.Messages;

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

        public MessageEvent(Message message)
        {
            Message = message;
        }

        public Message Message { get; }
    }
}