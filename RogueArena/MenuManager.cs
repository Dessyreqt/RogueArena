namespace RogueArena
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using RogueArena.Data;
    using RogueArena.Data.Components;
    using Console = SadConsole.Console;

    public class MenuManager
    {
        private Console _mainMenu;
        private Console _inventoryMenu;
        private Console _messageBox;
        private Console _levelUpMenu;
        private Console _characterScreen;

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

        public void ShowInventoryMenu(Console console, string header, Entity player, int inventoryWidth, int screenWidth, int screenHeight)
        {
            if (_inventoryMenu == null)
            {
                List<string> options;

                if (player.Get<InventoryComponent>().Items.Count == 0)
                {
                    options = new List<string> { "Inventory is empty." };
                }
                else
                {
                    options = player.Get<InventoryComponent>().Items.Select(
                        x =>
                        {
                            if (player.Get<EquipmentComponent>().MainHand == x)
                            {
                                return $"{x.Name} (in main hand)";
                            }

                            if (player.Get<EquipmentComponent>().OffHand == x)
                            {
                                return $"{x.Name} (in off hand)";
                            }

                            return x.Name;
                        }).ToList();
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
                    $"Constitution (+20 HP, from {player.Get<FighterComponent>().MaxHp})",
                    $"Strength (+1 attack, from {player.Get<FighterComponent>().Power})",
                    $"Agility (+1 defense, from {player.Get<FighterComponent>().Defense})"
                };

                _levelUpMenu = ShowMenu(console, header, options, width, screenWidth, screenHeight, Color.DarkGreen);
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

        public void ShowCharacterScreen(Console console, Entity player, int characterScreenWidth, int characterScreenHeight, int screenWidth, int screenHeight)
        {
            if (_characterScreen == null)
            {
                _characterScreen = new Console(characterScreenWidth, characterScreenHeight);
                _characterScreen.DefaultBackground = Color.DarkGray;
                _characterScreen.DefaultForeground = Color.White;

                _characterScreen.Print(0, 1, "Character Information");
                _characterScreen.Print(0, 2, $"Level: {player.Get<LevelComponent>().CurrentLevel}");
                _characterScreen.Print(0, 3, $"Experience: {player.Get<LevelComponent>().CurrentXp}");
                _characterScreen.Print(0, 4, $"Experience to Level: {player.Get<LevelComponent>().ExperienceToNextLevel}");
                _characterScreen.Print(0, 6, $"Maximum HP: {player.Get<FighterComponent>().MaxHp}");
                _characterScreen.Print(0, 7, $"Attack: {player.Get<FighterComponent>().Power}");
                _characterScreen.Print(0, 8, $"Defense: {player.Get<FighterComponent>().Defense}");


                var windowX = screenWidth / 2 - characterScreenWidth / 2;
                var windowY = screenHeight / 2 - characterScreenHeight / 2;

                _characterScreen.Position = new Point(windowX, windowY);
                console.Children.Add(_characterScreen);
            }
        }

        public void HideCharacterScreen(Console console)
        {
            if (_characterScreen != null)
            {
                console.Children.Remove(_characterScreen);
                _characterScreen = null;
            }
        }

        private static Console ShowMenu(Console console, string header, List<string> options, int width, int screenWidth, int screenHeight)
        {
            return ShowMenu(console, header, options, width, screenWidth, screenHeight, Color.DarkGray);
        }

        private static Console ShowMenu(Console console, string header, List<string> options, int width, int screenWidth, int screenHeight, Color backgroundColor)
        {
            if (options.Count > 26)
            {
                throw new ArgumentException("Cannot have a menu with more than 26 options");
            }

            var wrappedHeader = WrapText(header, width);
            var headerHeight = (wrappedHeader.Length + 49) / 50;
            var height = options.Count + headerHeight;

            var window = new Console(width, height);

            window.DefaultBackground = backgroundColor;
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
