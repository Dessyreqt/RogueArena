namespace RogueArena
{
    using System.Collections.Generic;
    using Map;
    using Microsoft.Xna.Framework;
    using RogueArena.Commands;
    using SadConsole;
    using Game = SadConsole.Game;

    class Program
    {
        private const int _width = 80;
        private const int _height = 50;
        private const int _mapWidth = 80;
        private const int _mapHeight = 45;
        private static Console _defaultConsole;
        private static Entity _player;
        private static GameMap _gameMap;

        private static readonly List<Entity> _entities = new List<Entity>();
        private static Dictionary<string, Color> _colors = new Dictionary<string, Color>
        {
            { "dark_wall", new Color(0, 0, 100) },
            { "dark_ground", new Color(50, 50 , 150) }
        };
        

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

            _defaultConsole = new Console(_width, _height);
            _defaultConsole.DefaultForeground = Color.White;
            _defaultConsole.IsCursorDisabled = true;

            Global.CurrentScreen = _defaultConsole;
            Global.FocusedConsoles.Set(_defaultConsole);

            _player = new Entity(_width / 2, _height / 2, '@', Color.White);
            _entities.Add(_player);
            _entities.Add(new Entity(_width / 2 - 5, _height / 2, '@', Color.Yellow));

            _gameMap = new GameMap(_mapWidth, _mapHeight);
        }

        private static void Update(GameTime gameTime)
        {
            RenderFunctions.ClearAll(_defaultConsole, _entities);

            if (Global.KeyboardState.KeysPressed.Count > 0)
            {
                var command = InputHandler.HandleKeys(Global.KeyboardState.KeysPressed);

                var move = command as MoveCommand;
                if (move != null)
                {
                    if (!_gameMap.IsBlocked(_player.X + move.X, _player.Y + move.Y))
                    {
                        _player.Move(move.X, move.Y);
                    }
                }

                var exit = command as ExitCommand;
                if (exit != null)
                {
                    Game.Instance.Exit();
                }
            }

            RenderFunctions.RenderAll(_defaultConsole, _entities, _gameMap, _colors);
        }
    }
}
