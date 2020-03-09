namespace RogueArena.Events
{
    using System;
    using System.Collections.Generic;

    public class EventLog
    {
        private static readonly Lazy<EventLog> Lazy = new Lazy<EventLog>(() => new EventLog());
        private List<Event> _log;

        private EventLog()
        {
        }

        public static List<Event> Instance => Lazy.Value._log ?? (Lazy.Value._log = new List<Event>());

        public static void Add(Event @event)
        {
            Instance.Add(@event);
        }
    }
}
