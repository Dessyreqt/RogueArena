namespace RogueArena
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using Commands;
    using Components;
    using Events;
    using Map;
    using Microsoft.Xna.Framework;
    using RogueArena.Components.ItemFunctions;
    using SadConsole;
    using SadConsole.Input;
    using Console = SadConsole.Console;
    using Game = SadConsole.Game;
    using Point = Microsoft.Xna.Framework.Point;

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
        private const int _maxItemsPerRoom = 2;

        private static readonly List<Entity> _entities = new List<Entity>();
        private static readonly Random _random = new Random();

        private static Console _defaultConsole;
        private static Console _panel;
        private static Console _inventoryMenu;
        private static Entity _player;
        private static GameMap _gameMap;
        private static bool _fovRecompute;
        private static GameState _gameState;
        private static GameState _previousGameState;
        private static MessageLog _messageLog;
        private static MouseEventArgs _mouse;
        private static Entity _targetingItem;

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
            LoadPosition();
            Game.Instance.Window.ClientSizeChanged += (sender, e) => { SavePosition(); };

            EventLog.Initialize();
            _messageLog = new MessageLog(_messageX, _messageWidth, _messageHeight);

            _defaultConsole = new Console(_width, _height);
            _defaultConsole.DefaultForeground = Color.White;
            _defaultConsole.IsCursorDisabled = true;
            _defaultConsole.MouseMove += Console_MouseMove;

            _panel = new Console(_width, _panelHeight) { Position = new Point(0, _panelY) };

            _defaultConsole.Children.Add(_panel);

            Global.CurrentScreen = _defaultConsole;
            Global.FocusedConsoles.Set(_defaultConsole);

            _player = new Entity(0, 0, '@', Color.White, "Player", true, RenderOrder.Actor, new Fighter(30, 2, 5), inventory: new Inventory(26));
            _entities.Add(_player);

            _gameMap = new GameMap(_mapWidth, _mapHeight, _random);
            _gameMap.MakeMap(_maxRooms, _minRoomSize, _maxRoomSize, _mapWidth, _mapHeight, _player, _entities, _maxMonstersPerRoom, _maxItemsPerRoom);
            _fovRecompute = true;
            _gameState = GameState.PlayersTurn;

            InitializeInventory();
        }

        private static void InitializeInventory()
        {
            //_player.Inventory.AddItem(new Entity(0, 0, '#', Color.Red, "Fireball Scroll", renderOrder:RenderOrder.Item, item: new Item(new CastFireballFunction(12, 3), true, new Message("Left-click a target tile for the fireball, or right-click to cancel.", Color.LightCyan))));
        }

        private static void LoadPosition()
        {
            var x = Math.Max(Properties.Settings.Default.PositionX, 0);
            var y = Math.Max(Properties.Settings.Default.PositionY, 0);

            if (x + Game.Instance.Window.ClientBounds.Width / 2 > SystemParameters.VirtualScreenWidth)
            {
                x = (int)SystemParameters.VirtualScreenWidth - Game.Instance.Window.ClientBounds.Width;
            }

            if (y + Game.Instance.Window.ClientBounds.Height / 2 > SystemParameters.VirtualScreenHeight)
            {
                y = (int)SystemParameters.VirtualScreenHeight - Game.Instance.Window.ClientBounds.Height;
            }

            Game.Instance.Window.Position = new Point(x, y);
        }

        private static void SavePosition()
        {
            Properties.Settings.Default.PositionX = Game.Instance.Window.Position.X;
            Properties.Settings.Default.PositionY = Game.Instance.Window.Position.Y;

            Properties.Settings.Default.Save();
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
                var command = InputHandler.HandleKeys(Global.KeyboardState.KeysPressed, _gameState);

                switch (command)
                {
                    case DropInventoryCommand _:
                        _previousGameState = _gameState;
                        _gameState = GameState.DropInventory;
                        _inventoryMenu = Menus.InventoryMenu(
                            _defaultConsole,
                            "Press the key next to an item to drop it, or Esc to cancel.",
                            _player.Inventory,
                            50,
                            _width,
                            _height);
                        _defaultConsole.Children.Add(_inventoryMenu);

                        break;
                    case ExitCommand _:
                        if (_gameState == GameState.ShowInventory || _gameState == GameState.DropInventory)
                        {
                            _gameState = _previousGameState;
                            RemoveInventoryMenu();
                        }
                        else if (_gameState == GameState.Targeting)
                        {
                            EventLog.Add(new TargetingCanceledEvent());
                        }
                        else
                        {
                            Game.Instance.Exit();
                        }

                        break;
                    case InventoryIndexCommand inv:
                        if (_previousGameState != GameState.PlayerDead && inv.Index < _player.Inventory.Items.Count)
                        {
                            var item = _player.Inventory.Items[inv.Index];

                            if (_gameState == GameState.ShowInventory)
                            {
                                _player.Inventory.Use(item, _entities, _gameMap);
                            }
                            else if (_gameState == GameState.DropInventory)
                            {
                                _player.Inventory.Drop(item);
                            }
                        }

                        break;
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
                    case PickupCommand _:
                        if (_gameState == GameState.PlayersTurn)
                        {
                            Entity foundEntity = null;

                            foreach (var entity in _entities)
                            {
                                if (entity.Item != null && entity.X == _player.X && entity.Y == _player.Y)
                                {
                                    _player.Inventory.AddItem(entity);
                                    foundEntity = entity;
                                    break;
                                }
                            }

                            if (foundEntity != null)
                            {
                                _gameState = GameState.EnemyTurn;
                            }
                            else
                            {
                                EventLog.Add(new MessageEvent("There is nothing here to pick up.", Color.Yellow));
                            }
                        }

                        break;
                    case RestCommand _:
                        if (_gameState == GameState.PlayersTurn)
                        {
                            _gameState = GameState.EnemyTurn;
                        }

                        break;
                    case ShowInventoryCommand _:
                        _previousGameState = _gameState;
                        _gameState = GameState.ShowInventory;
                        _inventoryMenu = Menus.InventoryMenu(
                            _defaultConsole,
                            "Press the key next to an item to use it, or Esc to cancel.",
                            _player.Inventory,
                            50,
                            _width,
                            _height);
                        _defaultConsole.Children.Add(_inventoryMenu);

                        break;
                }

                ProcessEvents();

                if (_gameState == GameState.EnemyTurn)
                {
                    RemoveInventoryMenu();
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

            if (_gameState == GameState.Targeting)
            {
                RemoveInventoryMenu();

                if (Global.MouseState.LeftButtonDown)
                {
                    var targetPos = _mouse.MouseState.CellPosition;

                    _player.Inventory.Use(_targetingItem, _entities, _gameMap, targetPos.X, targetPos.Y);
                    ProcessEvents();
                }
                else if (Global.MouseState.RightButtonDown)
                {
                    EventLog.Add(new TargetingCanceledEvent());
                    ProcessEvents();
                }
            }

            if (_fovRecompute)
            {
                _gameMap.ComputeFov(_player.X, _player.Y, _fovRadius, _fovLightWalls, _fovAlgorithm);
            }

            RenderFunctions.RenderAll(_defaultConsole, _panel, _entities, _player, _gameMap, _fovRecompute, _messageLog, _colors, _barWidth, _mouse);
            _fovRecompute = false;
        }

        private static void RemoveInventoryMenu()
        {
            if (_inventoryMenu != null)
            {
                _defaultConsole.Children.Remove(_inventoryMenu);
                _inventoryMenu = null;
            }
        }

        private static void ProcessEvents()
        {
            for (var index = 0; index < EventLog.Instance.Count; index++)
            {
                var @event = EventLog.Instance[index];

                switch (@event)
                {
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
                    case ItemConsumedEvent consumed:
                        _gameState = GameState.EnemyTurn;

                        _messageLog.AddMessage(consumed.Message);

                        break;
                    case ItemDroppedEvent dropped:
                        _entities.Add(dropped.Item);

                        if (dropped.Entity == _player)
                        {
                            _messageLog.AddMessage($"You dropped the {dropped.Item.Name}!", Color.Yellow);
                        }
                        else
                        {
                            _messageLog.AddMessage($"The {dropped.Entity.Name} dropped the {dropped.Item.Name}.", Color.Beige);
                        }

                        _gameState = GameState.EnemyTurn;

                        break;
                    case ItemPickupEvent pickup:
                        _entities.Remove(pickup.Item);

                        if (pickup.Entity == _player)
                        {
                            _messageLog.AddMessage($"You pick up the {pickup.Item.Name}!", Color.Blue);
                        }
                        else
                        {
                            _messageLog.AddMessage($"The {pickup.Entity.Name} picks up the {pickup.Item.Name}.", Color.Beige);
                        }

                        break;
                    case MessageEvent message:
                        _messageLog.AddMessage(message.Message);
                        break;
                    case TargetingCanceledEvent _:
                        _gameState = _previousGameState;
                        _messageLog.AddMessage("Targeting canceled.");
                        break;
                    case TargetingStartEvent targeting:
                        _previousGameState = GameState.PlayersTurn;
                        _gameState = GameState.Targeting;

                        _targetingItem = targeting.ItemEntity;

                        _messageLog.AddMessage(_targetingItem.Item.TargetingMessage);
                        break;
                }
            }

            EventLog.Instance.Clear();
        }
    }
}