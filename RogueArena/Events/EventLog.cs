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

        public static void Add(Event @event)
        {
            Instance.Add(@event);
        }

        public static List<Event> Instance
        {
            get
            {
                var instance = Lazy.Value;

                if (instance.WasInitialized)
                {
                    return instance._log;
                }

                throw new InvalidOperationException("Logger must be initialized before it can be used.");
            }
        }

        private bool WasInitialized => _log != null;

        public static void Initialize()
        {
            Lazy.Value._log = new List<Event>();
        }
    }
}
