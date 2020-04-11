namespace RogueArena.Messages
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;

    [Serializable]
    public class MessageLog
    {
        public MessageLog()
        {
            Messages = new List<Message>();
        }

        public List<Message> Messages { get; set; }
        public int X { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public void AddMessage(string text)
        {
            AddMessage(new Message(text));
        }

        public void AddMessage(string text, Color color)
        {
            AddMessage(new Message(text, color));
        }

        public void AddMessage(Message message)
        {
            var messageLines = WrapText(message.Text, Width);

            foreach (var line in messageLines)
            {
                if (Messages.Count == Height)
                {
                    Messages.RemoveAt(0);
                }

                Messages.Add(new Message(line, message.Color));
            }
        }

        private List<string> WrapText(string messageText, int width)
        {
            var textLines = new List<string>();

            for (int i = 0; i < messageText.Length; i += width)
            {
                if (i + width > messageText.Length)
                {
                    textLines.Add(messageText.Substring(i));
                }
                else
                {
                    textLines.Add(messageText.Substring(i, width));
                }
            }

            return textLines;
        }
    }
}