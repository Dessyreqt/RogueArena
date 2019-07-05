namespace RogueArena.Events
{
    public class MessageEvent : Event
    {
        public MessageEvent(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
}