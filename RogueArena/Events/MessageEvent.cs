namespace RogueArena.Events
{
    using Microsoft.Xna.Framework;
    using RogueArena.Data;
    using RogueArena.Messages;

    public class MessageEvent : Event
    {
        private readonly Message _message;

        public MessageEvent(string message)
            : this(message, Color.White)
        {
        }

        public MessageEvent(string message, Color color)
        {
            _message = new Message(message, color);
        }

        public MessageEvent(Message message)
        {
            _message = message;
        }

        protected override void Handle(ProgramData data)
        {
            data.GameData.MessageLog.AddMessage(_message);
        }
    }
}
