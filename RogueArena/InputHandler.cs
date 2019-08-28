namespace RogueArena
{
    using System.Collections.ObjectModel;
    using Commands;
    using Microsoft.Xna.Framework.Input;
    using SadConsole;
    using SadConsole.Input;

    public static class InputHandler
    {
        public static Command HandleKeys(ReadOnlyCollection<AsciiKey> keys, GameState gameState)
        {
            switch (gameState)
            {
                case GameState.PlayerDead:
                    return HandlePlayerDeadKeys(keys);
                case GameState.PlayersTurn:
                    return HandlePlayersTurnKeys(keys);
                case GameState.ShowInventory:
                    return HandleShowInventoryKeys(keys);
                default:
                    return null;
            }
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
                    case Keys.W:
                    case Keys.K:
                    case Keys.NumPad8:
                        return new MoveCommand(0, -1);
                    case Keys.Down:
                    case Keys.S:
                    case Keys.J:
                    case Keys.NumPad2:
                        return new MoveCommand(0, 1);
                    case Keys.Left:
                    case Keys.A:
                    case Keys.H:
                    case Keys.NumPad4:
                        return new MoveCommand(-1, 0);
                    case Keys.Right:
                    case Keys.D:
                    case Keys.L:
                    case Keys.NumPad6:
                        return new MoveCommand(1, 0);
                    case Keys.Y:
                    case Keys.NumPad7:
                        return new MoveCommand(-1, -1);
                    case Keys.U:
                    case Keys.NumPad9:
                        return new MoveCommand(1, -1);
                    case Keys.B:
                    case Keys.NumPad1:
                        return new MoveCommand(-1, 1);
                    case Keys.N:
                    case Keys.NumPad3:
                        return new MoveCommand(1, 1);
                    case Keys.G:
                        return new PickupCommand();
                    case Keys.F5:
                        Settings.ToggleFullScreen();
                        return null;
                    case Keys.Escape:
                        return new ExitCommand();
                    case Keys.R:
                        return new RestCommand();
                    case Keys.I:
                        return new ShowInventoryCommand();
                }
            }

            return null;
        }
    }
}