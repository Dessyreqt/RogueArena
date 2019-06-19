namespace RogueArena
{
    using System.Collections.ObjectModel;
    using Microsoft.Xna.Framework.Input;
    using RogueArena.Commands;
    using SadConsole.Input;

    public static class InputHandler
    {
        public static Command HandleKeys(ReadOnlyCollection<AsciiKey> keys)
        {
            if (keys.Count > 0)
            {
                var key = keys[0].Key;

                switch (key)
                {
                    case Keys.Up:
                        return new MoveCommand { X = 0, Y = -1 };
                    case Keys.Down:
                        return new MoveCommand { X = 0, Y = 1 };
                    case Keys.Left:
                        return new MoveCommand { X = -1, Y = 0 };
                    case Keys.Right:
                        return new MoveCommand { X = 1, Y = 0 };
                    case Keys.Escape:
                        return new ExitCommand();
                }
            }

            return null;
        }
    }
}
