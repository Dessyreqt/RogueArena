namespace RogueArena
{
    using System;
    using System.Collections.Generic;
    using Commands;
    using Map;
    using Microsoft.Xna.Framework;
    using SadConsole;
    using Console = SadConsole.Console;
    using Game = SadConsole.Game;

    class Program
    {
        private const int _width = 80;
        private const int _height = 50;
        private const int _mapWidth = 80;
        private const int _mapHeight = 45;
        private const int _minRoomSize = 6;
        private const int _maxRoomSize = 10;
        private const int _maxRooms = 30;
        private const int _fovAlgorithm = GameMap.FovBasic;
        private const bool _fovLightWalls = true;
        private const int _fovRadius = 10;
        
        private static readonly List<Entity> _entities = new List<Entity>();
        private static readonly Random _random = new Random();

        private static Console _defaultConsole;
        private static Entity _player;
        private static GameMap _gameMap;
        private static bool _fovRecompute;

        private static Dictionary<string, Color> _colors = new Dictionary<string, Color>
                                                           {
                                                               { "dark_wall", new Color(0, 0, 100) },
                                                               { "dark_ground", new Color(50, 50, 150) },
                                                               { "light_wall", new Color(130, 110, 50) },
                                                               { "light_ground", new Color(200, 180, 50) }
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

            _gameMap = new GameMap(_mapWidth, _mapHeight, _random);
            _gameMap.MakeMap(_maxRooms, _minRoomSize, _maxRoomSize, _mapWidth, _mapHeight, _player);
            _fovRecompute = true;
        }

        private static void Update(GameTime gameTime)
        {
            RenderFunctions.ClearAll(_defaultConsole, _entities);

            if (Global.KeyboardState.KeysPressed.Count > 0)
            {
                var command = InputHandler.HandleKeys(Global.KeyboardState.KeysPressed);

                switch (command)
                {
                    case MoveCommand move:
                        if (!_gameMap.IsBlocked(_player.X + move.X, _player.Y + move.Y))
                        {
                            _player.Move(move.X, move.Y);
                            _fovRecompute = true;
                        }

                        break;
                    case ExitCommand _:
                        Game.Instance.Exit();
                        break;
                }
            }

            if (_fovRecompute)
            {
                _gameMap.ComputeFov(_player.X, _player.Y, _fovRadius, _fovLightWalls, _fovAlgorithm);
            }

            RenderFunctions.RenderAll(_defaultConsole, _entities, _gameMap, _fovRecompute, _colors);
            _fovRecompute = false;
        }
    }
}