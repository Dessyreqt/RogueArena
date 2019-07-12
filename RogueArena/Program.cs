namespace RogueArena
{
    using System;
    using System.Collections.Generic;
    using Commands;
    using Components;
    using Events;
    using Map;
    using Microsoft.Xna.Framework;
    using SadConsole;
    using SadConsole.Input;
    using Console = SadConsole.Console;
    using Game = SadConsole.Game;

    class Program
    {
        private const int _width = 80;
        private const int _height = 50;

        private const int _barWidth = 20;
        private const int _panelHeight = 7;
        private const int _panelY = _height - _panelHeight;

        private const int _messageX = _barWidth + 2;
        private const int _messageWidth = _width - _barWidth - 2;
        private const int _messageHeight = _panelHeight - 1;

        private const int _mapWidth = 80;
        private const int _mapHeight = 43;

        private const int _minRoomSize = 6;
        private const int _maxRoomSize = 10;
        private const int _maxRooms = 30;

        private const int _fovAlgorithm = GameMap.FovBasic;
        private const bool _fovLightWalls = true;
        private const int _fovRadius = 10;

        private const int _maxMonstersPerRoom = 3;

        private static readonly List<Entity> _entities = new List<Entity>();
        private static readonly Random _random = new Random();

        private static Console _defaultConsole;
        private static Console _panel;
        private static Entity _player;
        private static GameMap _gameMap;
        private static bool _fovRecompute;
        private static GameState _gameState;
        private static MessageLog _messageLog;
        private static MouseEventArgs _mouse;

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

            EventLog.Initialize();
            _messageLog = new MessageLog(_messageX, _messageWidth, _messageHeight);

            _defaultConsole = new Console(_width, _height);
            _defaultConsole.DefaultForeground = Color.White;
            _defaultConsole.IsCursorDisabled = true;
            _defaultConsole.MouseMove += Console_MouseMove;

            _panel = new Console(_width, _panelHeight) { Position = new Microsoft.Xna.Framework.Point(0, _panelY) };

            _defaultConsole.Children.Add(_panel);

            Global.CurrentScreen = _defaultConsole;
            Global.FocusedConsoles.Set(_defaultConsole);

            _player = new Entity(0, 0, '@', Color.White, "Player", true, RenderOrder.Actor, new Fighter(30, 2, 5));
            _entities.Add(_player);

            _gameMap = new GameMap(_mapWidth, _mapHeight, _random);
            _gameMap.MakeMap(_maxRooms, _minRoomSize, _maxRoomSize, _mapWidth, _mapHeight, _player, _entities, _maxMonstersPerRoom);
            _fovRecompute = true;
            _gameState = GameState.PlayersTurn;
        }

        private static void Console_MouseMove(object sender, MouseEventArgs e)
        {
            _mouse = e;
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
                        _defaultConsole.Clear(0, 45, 80);

                        if (_gameState == GameState.PlayersTurn)
                        {
                            var destX = _player.X + move.X;
                            var destY = _player.Y + move.Y;

                            if (!_gameMap.IsBlocked(destX, destY))
                            {
                                var target = Entity.GetBlockingEntityAtLocation(_entities, destX, destY);

                                if (target != null)
                                {
                                    _player.Fighter.Attack(target);
                                }
                                else
                                {
                                    _player.Move(move.X, move.Y);
                                    _fovRecompute = true;
                                }

                                _gameState = GameState.EnemyTurn;
                            }
                        }

                        break;
                    case RestCommand _:
                        _gameState = GameState.EnemyTurn;
                        break;
                    case ExitCommand _:
                        Game.Instance.Exit();
                        break;
                }

                ProcessEvents();

                if (_gameState == GameState.EnemyTurn)
                {
                    _defaultConsole.Clear(0, 46, 80);

                    foreach (var entity in _entities)
                    {
                        if (entity.AI != null)
                        {
                            entity.AI.TakeTurn(_player, _gameMap, _entities);
                            ProcessEvents();

                            if (_gameState == GameState.PlayerDead)
                            {
                                break;
                            }
                        }
                    }

                    if (_gameState != GameState.PlayerDead)
                    {
                        _gameState = GameState.PlayersTurn;
                    }
                }
            }

            if (_fovRecompute)
            {
                _gameMap.ComputeFov(_player.X, _player.Y, _fovRadius, _fovLightWalls, _fovAlgorithm);
            }

            RenderFunctions.RenderAll(_defaultConsole, _panel, _entities, _player, _gameMap, _fovRecompute, _messageLog, _colors, _barWidth, _mouse);
            _fovRecompute = false;
        }

        private static void ProcessEvents()
        {
            for (var index = 0; index < EventLog.Instance.Count; index++)
            {
                var @event = EventLog.Instance[index];

                switch (@event)
                {
                    case MessageEvent message:
                        _messageLog.AddMessage(message.Message);
                        break;
                    case DeadEvent dead:
                        if (dead.Entity == _player)
                        {
                            DeathFunctions.KillPlayer(dead.Entity);
                            _gameState = GameState.PlayerDead;
                        }
                        else
                        {
                            DeathFunctions.KillMonster(dead.Entity);
                        }

                        break;
                }
            }

            EventLog.Instance.Clear();
        }
    }
}