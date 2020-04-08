namespace RogueArena
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using RogueArena.Components;
    using Console = SadConsole.Console;

    public class MenuManager
    {
        private Console _mainMenu = null;
        private Console _inventoryMenu = null;
        private Console _messageBox = null;
        private Console _levelUpMenu = null;

        public void ShowMessageBox(Console console, string header, int width, int screenWidth, int screenHeight)
        {
            if (_messageBox == null)
            {
                _messageBox = ShowMenu(console, header, new List<string>(), width, screenWidth, screenHeight);
            }
        }

        public void HideMessageBox(Console console)
        {
            if (_messageBox != null)
            {
                console.Children.Remove(_messageBox);
                _messageBox = null;
            }
        }

        public void ShowInventoryMenu(Console console, string header, InventoryComponent inventoryComponent, int inventoryWidth, int screenWidth, int screenHeight)
        {
            if (_inventoryMenu == null)
            {
                List<string> options;

                if (inventoryComponent.Items.Count == 0)
                {
                    options = new List<string> { "Inventory is empty." };
                }
                else
                {
                    options = inventoryComponent.Items.Select(x => x.Name).ToList();
                }

                _inventoryMenu = ShowMenu(console, header, options, inventoryWidth, screenWidth, screenHeight);
            }
        }

        public void HideInventoryMenu(Console console)
        {
            if (_inventoryMenu != null)
            {
                console.Children.Remove(_inventoryMenu);
                _inventoryMenu = null;
            }
        }

        public void ShowMainMenu(Console console, int screenWidth, int screenHeight)
        {
            if (_mainMenu == null)
            {
                _mainMenu = ShowMenu(console, "", new List<string> { "Play a new game", "Continue last game", "Quit" }, 24, screenWidth, screenHeight);
            }
        }

        public void HideMainMenu(Console console)
        {
            if (_mainMenu != null)
            {
                console.Children.Remove(_mainMenu);
                _mainMenu = null;
            }
        }

        public void ShowLevelUpMenu(Console console, string header, Entity player, int width, int screenWidth, int screenHeight)
        {
            if (_levelUpMenu == null)
            {
                var options = new List<string>
                {
                    $"Constitution (+20 HP, from {player.FighterComponent.MaxHp})",
                    $"Strength (+1 attack, from {player.FighterComponent.Power})",
                    $"Agility (+1 defense, from {player.FighterComponent.Defense})"
                };

                _levelUpMenu = ShowMenu(console, header, options, width, screenWidth, screenHeight);
            }
        }

        public void HideLevelUpMenu(Console console)
        {
            if (_levelUpMenu != null)
            {
                console.Children.Remove(_levelUpMenu);
                _levelUpMenu = null;
            }
        }

        private static Console ShowMenu(Console console, string header, List<string> options, int width, int screenWidth, int screenHeight)
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
            console.Children.Add(window);

            return window;
        }

        private static string WrapText(string header, int width)
        {
            var words = header.Split(new[] { ' ', '\r', '\n' }, StringSplitOptions.None);
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
    }
}
