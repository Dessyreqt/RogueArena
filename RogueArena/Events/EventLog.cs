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

        private static List<Event> _instance => Lazy.Value._log ?? (Lazy.Value._log = new List<Event>());

        public static void Add(Event @event)
        {
            _instance.Add(@event);
        }

        public static int Count => _instance.Count;

        public static Event Event(int index) => _instance[index];

        public static void Clear()
        {
            _instance.Clear();
        }
    }
}
