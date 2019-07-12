namespace RogueArena.Events
{
    using Microsoft.Xna.Framework;

    public class Message
    {
        public Message(string text, Color color)
        {
            Text = text;
            Color = color;
        }

        public string Text { get; }
        public Color Color { get; }
    }
}