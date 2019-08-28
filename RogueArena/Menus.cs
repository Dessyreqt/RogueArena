namespace RogueArena
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Components;
    using Microsoft.Xna.Framework;
    using SharpDX.DXGI;
    using Console = SadConsole.Console;

    public static class Menus
    {
        public static Console Menu(Console console, string header, List<string> options, int width, int screenWidth, int screenHeight)
        {
            if (options.Count > 26)
            {
                throw new ArgumentException("Cannot have a menu with more than 26 options");
            }

            var wrappedHeader = WrapText(header, width);
            var headerHeight = (wrappedHeader.Length + 49) / 50;
            var height = options.Count + headerHeight;

            var window = new Console(width, height);

            window.DefaultBackground = Color.DarkGray;
            window.DefaultForeground = Color.White;
            window.Print(0, 0, wrappedHeader, window.DefaultForeground);

            var y = headerHeight;
            var letterIndex = (int)'a';

            foreach (var option in options)
            {
                var text = $"({(char)letterIndex}) {option}";
                window.Print(0, y, text, window.DefaultForeground);

                y++;
                letterIndex++;
            }

            var windowX = screenWidth / 2 - width / 2;
            var windowY = screenHeight / 2 - height / 2;

            window.Position = new Point(windowX, windowY);

            return window;
        }

        private static string WrapText(string header, int width)
        {
            var words = header.Split(new []{' ', '\r', '\n'}, StringSplitOptions.None);
            var lineWords = new List<string>();
            var wrappedText = new StringBuilder();

            foreach (var word in words)
            {
                var lineWordsLength = lineWords.Sum(x => x.Length) + lineWords.Count;

                if (lineWordsLength == 0)
                {
                    lineWords.Add(word);
                }
                else if (lineWordsLength + word.Length > width)
                {
                    wrappedText.Append(string.Join(" ", lineWords).PadRight(width));
                    lineWords.Clear();
                    lineWords.Add(word);
                }
                else
                {
                    lineWords.Add(word);
                }
            }

            wrappedText.Append(string.Join(" ", lineWords).PadRight(width));

            return wrappedText.ToString();
        }

        public static Console InventoryMenu(Console console, string header, Inventory inventory, int inventoryWidth, int screenWidth, int screenHeight)
        {
            List<string> options;

            if (inventory.Items.Count == 0)
            {
                options = new List<string> { "Inventory is empty." };
            }
            else
            {
                options = inventory.Items.Select(x => x.Name).ToList();
            }

            return Menu(console, header, options, inventoryWidth, screenWidth, screenHeight);
        }

        private static int GetHeaderHeight(Console console, int i, int i1, int width, int screenHeight, string header)
        {
            return 2;
        }
    }
}