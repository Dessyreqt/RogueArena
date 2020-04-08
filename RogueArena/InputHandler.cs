namespace RogueArena
{
    using System.Collections.ObjectModel;
    using Commands;
    using Microsoft.Xna.Framework.Input;
    using RogueArena.Commands.Game;
    using RogueArena.Commands.MainMenu;
    using SadConsole;
    using SadConsole.Input;

    public static class InputHandler
    {
        public static Command HandleGameKeys(ReadOnlyCollection<AsciiKey> keys, GameState gameState)
        {
            switch (gameState)
            {
                case GameState.PlayerDead:
                    return HandlePlayerDeadKeys(keys);
                case GameState.PlayersTurn:
                    return HandlePlayersTurnKeys(keys);
                case GameState.ShowInventory:
                case GameState.DropInventory:
                    return HandleShowInventoryKeys(keys);
                case GameState.Targeting:
                    return HandleTargetingKeys(keys);
                case GameState.LevelUp:
                    return HandleLevelUpKeys(keys);
                default:
                    return null;
            }
        }

        public static Command HandleMainMenuKeys(ReadOnlyCollection<AsciiKey> keys)
        {
            if (keys.Count > 0)
            {
                var key = keys[0].Key;

                switch (key)
                {
                    case Keys.A:
                        return new NewGameCommand();
                    case Keys.B:
                        return new LoadSavedGameCommand();
                    case Keys.C:
                        return new ExitGameCommand();
                }
            }

            return null;
        }

        private static Command HandleTargetingKeys(ReadOnlyCollection<AsciiKey> keys)
        {
            if (keys.Count > 0)
            {
                var key = keys[0].Key;
             
                switch (key)
                {
                    case Keys.F5:
                        Settings.ToggleFullScreen();
                        return null;
                    case Keys.Escape:
                        return new ExitCommand();
                }
            }

            return null;
        }

        private static Command HandleShowInventoryKeys(ReadOnlyCollection<AsciiKey> keys)
        {
            if (keys.Count > 0)
            {
                var key = keys[0].Key;
                var index = (int)key - 65;

                if (index >= 0)
                {
                    return new InventoryIndexCommand(index);
                }

                switch (key)
                {
                    case Keys.F5:
                        Settings.ToggleFullScreen();
                        return null;
                    case Keys.Escape:
                        return new ExitCommand();
                }
            }

            return null;
        }

        private static Command HandleLevelUpKeys(ReadOnlyCollection<AsciiKey> keys)
        {
            if (keys.Count > 0)
            {
                var key = keys[0].Key;

                switch (key)
                {
                    case Keys.A:
                        return new LevelUpCommand(LevelUpType.Hp);
                    case Keys.B:
                        return new LevelUpCommand(LevelUpType.Str);
                    case Keys.C:
                        return new LevelUpCommand(LevelUpType.Def);
                }
            }

            return null;
        }

        private static Command HandlePlayerDeadKeys(ReadOnlyCollection<AsciiKey> keys)
        {
            if (keys.Count > 0)
            {
                var key = keys[0].Key;

                switch (key)
                {
                    case Keys.F5:
                        Settings.ToggleFullScreen();
                        return null;
                    case Keys.Escape:
                        return new ExitCommand();
                    case Keys.I:
                        return new ShowInventoryCommand();
                }
            }

            return null;
        }

        private static Command HandlePlayersTurnKeys(ReadOnlyCollection<AsciiKey> keys)
        {
            if (keys.Count > 0)
            {
                var key = keys[0].Key;

                switch (key)
                {
                    case Keys.Up:
                    case Keys.NumPad8:
                        return new MoveCommand(0, -1);
                    case Keys.Down:
                    case Keys.NumPad2:
                        return new MoveCommand(0, 1);
                    case Keys.Left:
                    case Keys.NumPad4:
                        return new MoveCommand(-1, 0);
                    case Keys.Right:
                    case Keys.NumPad6:
                        return new MoveCommand(1, 0);
                    case Keys.NumPad7:
                        return new MoveCommand(-1, -1);
                    case Keys.NumPad9:
                        return new MoveCommand(1, -1);
                    case Keys.NumPad1:
                        return new MoveCommand(-1, 1);
                    case Keys.NumPad3:
                        return new MoveCommand(1, 1);
                    case Keys.G:
                        return new PickupCommand();
                    case Keys.F5:
                        Settings.ToggleFullScreen();
                        return null;
                    case Keys.Escape:
                        return new ExitCommand();
                    case Keys.Z:
                        return new RestCommand();
                    case Keys.I:
                    case Keys.B:
                        return new ShowInventoryCommand();
                    case Keys.D:
                        return new DropInventoryCommand();
                    case Keys.S:
                        return new TakeStairsCommand();
                }
            }

            return null;
        }
    }
}