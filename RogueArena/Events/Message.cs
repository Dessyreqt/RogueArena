namespace RogueArena.Events
{
    using Microsoft.Xna.Framework;

    public class Message
    {
        private Message()
        {
            // here for deserialization purposes
        }

        public Message(string text) : this(text, Color.White)
        {
        }

        public Message(string text, Color color)
        {
            Text = text;
            Color = color;
        }

        public string Text { get; set; }
        public Color Color { get; set; }
    }
}