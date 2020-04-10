namespace RogueArena.Events
{
    using RogueArena.Data;

    public abstract class Event
    {
        public static void Process(Event @event, ProgramData data)
        {
            @event?.Handle(data);
        }

        protected abstract void Handle(ProgramData data);
    }
}