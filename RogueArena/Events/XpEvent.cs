namespace RogueArena.Events
{
    public class XpEvent : Event
    {
        public XpEvent(int xp)
        {
            Xp = xp;
        }

        public int Xp { get; set; }
    }
}
