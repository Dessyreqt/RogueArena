using SadConsole;
using Microsoft.Xna.Framework;
using Game = SadConsole.Game;
using RogueArena.Commands;

namespace RogueArena
{
    class Program
    {
        private const int _width = 80;
        private const int _height = 50;
        private static int _playerX;
        private static int _playerY;
        private static Console _defaultConsole;

        static void Main(string[] args)
        {
            // Setup the engine and create the main window.
            Game.Create("fonts\\C64.font", _width, _height);

            // Hook the start event so we can add consoles to the system.
            Game.OnInitialize = Init;
            Game.OnUpdate = Update;

            // Start the game.
            Game.Instance.Run();
            Game.Instance.Dispose();
        }

        private static void Init()
        {
            Game.Instance.Window.Title = "RogueArena";

            _defaultConsole = new SadConsole.Console(_width, _height);
            _defaultConsole.DefaultForeground = Color.White;
            _defaultConsole.IsCursorDisabled = true;

            Global.CurrentScreen = _defaultConsole;
            Global.FocusedConsoles.Set(_defaultConsole);

            _playerX = _width / 2;
            _playerY = _height / 2;
        }

        private static void Update(GameTime gameTime)
        {
            _defaultConsole.Clear();
            _defaultConsole.Print(_playerX, _playerY, "@");

            if (Global.KeyboardState.KeysPressed.Count > 0)
            {
                var command = InputHandler.HandleKeys(Global.KeyboardState.KeysPressed);

                var move = command as MoveCommand;
                if (move != null)
                {
                    _playerX += move.X;
                    _playerY += move.Y;
                }

                var exit = command as ExitCommand;
                if (exit != null)
                {
                    Game.Instance.Exit();
                }
            }
        }
    }
}
