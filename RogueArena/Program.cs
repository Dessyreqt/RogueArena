namespace RogueArena
{
    using System;
    using System.Windows;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using RogueArena.Commands.Game;
    using RogueArena.Commands.MainMenu;
    using RogueArena.Data;
    using RogueArena.Events;
    using SadConsole;
    using SadConsole.DrawCalls;
    using SadConsole.Input;
    using Console = SadConsole.Console;
    using Game = SadConsole.Game;

    class Program
    {
        private static GameData _gameData;

        private static Console _defaultConsole;
        private static Console _panel;
        private static Texture2D _titleScreen;

        private static MenuManager _menuManager;

        private static bool _showMainMenu;
        private static bool _showLoadErrorMessage;
        private static bool _fovRecompute;
        private static GameState _previousGameState;
        private static MouseEventArgs _mouse;
        private static Entity _targetingItem;

        static void Main(string[] args)
        {
            // Setup the engine and create the main window.
            Game.Create("fonts\\C64.font", Constants.ScreenWidth, Constants.ScreenHeight);

            // Hook the start event so we can add consoles to the system.
            Game.OnInitialize = Init;
            Game.OnUpdate = Update;

            // Start the game.
            Game.Instance.Run();
            Game.Instance.Dispose();
        }

        private static void Init()
        {
            Game.Instance.Window.Title = Constants.WindowTitle;
            LoadPosition();
            Game.Instance.Window.ClientSizeChanged += (sender, e) => { SavePosition(); };

            LoadContent();

            _defaultConsole = new Console(Constants.ScreenWidth, Constants.ScreenHeight);
            _defaultConsole.DefaultForeground = Color.White;
            _defaultConsole.DefaultBackground = Color.Transparent;
            _defaultConsole.IsCursorDisabled = true;
            _defaultConsole.MouseMove += Console_MouseMove;

            _panel = new Console(Constants.ScreenWidth, Constants.PanelHeight) { Position = new Point(0, Constants.PanelY) };

            _defaultConsole.Children.Add(_panel);

            _menuManager = new MenuManager();

            Global.CurrentScreen = _defaultConsole;
            Global.FocusedConsoles.Set(_defaultConsole);

            _fovRecompute = true;
            _showMainMenu = true;
            _showLoadErrorMessage = false;

            InitializeInventory();
        }

        private static void LoadContent()
        {
            _titleScreen = Game.Instance.Content.Load<Texture2D>(@"Textures\menu_background");
        }

        private static void InitializeInventory()
        {
            //_gameData.Player.Inventory.AddItem(Items.Get(Items.FireballScroll, 0, 0));
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
            if (_showMainMenu)
            { 
                //_menuManager.ShowMainMenu(_defaultConsole, Constants.ScreenWidth, Constants.ScreenHeight);
                Global.DrawCalls.Add(new DrawCallTexture(_titleScreen, _defaultConsole.Position.ToVector2()));
                HandleMainMenu();
            }
            else
            {
                _menuManager.HideMainMenu(_defaultConsole);
                _menuManager.HideMessageBox(_defaultConsole);
                PlayGame();
            }

            if (_showLoadErrorMessage)
            {
                _menuManager.ShowMessageBox(_defaultConsole, "No save game to load", 50, Constants.ScreenWidth, Constants.ScreenHeight);
            }
        }

        private static void HandleMainMenu()
        {
            var action = InputHandler.HandleMainMenuKeys(Global.KeyboardState.KeysPressed);

            if (action != null)
            {
                _showLoadErrorMessage = false;
            }

            switch (action)
            {
                case NewGameCommand _:
                    _gameData = GameData.New();
                    _previousGameState = _gameData.GameState;
                    _showMainMenu = false;
                    break;
                case LoadSavedGameCommand _:
                    _gameData = GameData.Load();

                    if (_gameData == null)
                    {
                        _showLoadErrorMessage = true;
                    }
                    else
                    {
                        _previousGameState = _gameData.GameState;
                        _showMainMenu = false;
                    }

                    break;
                case ExitGameCommand _:
                    Game.Instance.Exit();
                    break;
            }
        }

        private static void PlayGame()
        {
            RenderFunctions.ClearAll(_defaultConsole, _gameData.Entities);

            if (Global.KeyboardState.KeysPressed.Count > 0)
            {
                var command = InputHandler.HandleGameKeys(Global.KeyboardState.KeysPressed, _gameData.GameState);

                switch (command)
                {
                    case DropInventoryCommand _:
                        _previousGameState = _gameData.GameState;
                        _gameData.GameState = GameState.DropInventory;
                        _menuManager.ShowInventoryMenu(
                            _defaultConsole,
                            "Press the key next to an item to drop it, or Esc to cancel.",
                            _gameData.Player.Inventory,
                            50,
                            Constants.ScreenWidth,
                            Constants.ScreenHeight);

                        break;
                    case ExitCommand _:
                        if (_gameData.GameState == GameState.ShowInventory || _gameData.GameState == GameState.DropInventory)
                        {
                            _gameData.GameState = _previousGameState;
                            _menuManager.HideInventoryMenu(_defaultConsole);
                        }
                        else if (_gameData.GameState == GameState.Targeting)
                        {
                            EventLog.Add(new TargetingCanceledEvent());
                        }
                        else
                        {
                            _gameData.Save();
                            Game.Instance.Exit();
                        }

                        break;
                    case InventoryIndexCommand inv:
                        if (_previousGameState != GameState.PlayerDead && inv.Index < _gameData.Player.Inventory.Items.Count)
                        {
                            var item = _gameData.Player.Inventory.Items[inv.Index];

                            if (_gameData.GameState == GameState.ShowInventory)
                            {
                                _gameData.Player.Inventory.Use(item, _gameData.Entities, _gameData.GameMap);
                            }
                            else if (_gameData.GameState == GameState.DropInventory)
                            {
                                _gameData.Player.Inventory.Drop(item);
                            }
                        }

                        break;
                    case MoveCommand move:
                        _defaultConsole.Clear(0, 45, 80);

                        if (_gameData.GameState == GameState.PlayersTurn)
                        {
                            var destX = _gameData.Player.X + move.X;
                            var destY = _gameData.Player.Y + move.Y;

                            if (!_gameData.GameMap.IsBlocked(destX, destY))
                            {
                                var target = Entity.GetBlockingEntityAtLocation(_gameData.Entities, destX, destY);

                                if (target != null)
                                {
                                    _gameData.Player.Fighter.Attack(target);
                                }
                                else
                                {
                                    _gameData.Player.Move(move.X, move.Y);
                                    _fovRecompute = true;
                                }

                                _gameData.GameState = GameState.EnemyTurn;
                            }
                        }

                        break;
                    case PickupCommand _:
                        if (_gameData.GameState == GameState.PlayersTurn)
                        {
                            Entity foundEntity = null;

                            foreach (var entity in _gameData.Entities)
                            {
                                if (entity.Item != null && entity.X == _gameData.Player.X && entity.Y == _gameData.Player.Y)
                                {
                                    _gameData.Player.Inventory.AddItem(entity);
                                    foundEntity = entity;
                                    break;
                                }
                            }

                            if (foundEntity != null)
                            {
                                _gameData.GameState = GameState.EnemyTurn;
                            }
                            else
                            {
                                EventLog.Add(new MessageEvent("There is nothing here to pick up.", Color.Yellow));
                            }
                        }

                        break;
                    case RestCommand _:
                        if (_gameData.GameState == GameState.PlayersTurn)
                        {
                            _gameData.GameState = GameState.EnemyTurn;
                        }

                        break;
                    case ShowInventoryCommand _:
                        _previousGameState = _gameData.GameState;
                        _gameData.GameState = GameState.ShowInventory;
                        _menuManager.ShowInventoryMenu(
                            _defaultConsole,
                            "Press the key next to an item to use it, or Esc to cancel.",
                            _gameData.Player.Inventory,
                            50,
                            Constants.ScreenWidth,
                            Constants.ScreenHeight);

                        break;
                }

                ProcessEvents();
            }

            if (_gameData.GameState == GameState.EnemyTurn)
            {
                _menuManager.HideInventoryMenu(_defaultConsole);
                _defaultConsole.Clear(0, 46, 80);

                foreach (var entity in _gameData.Entities)
                {
                    if (entity.AI != null)
                    {
                        entity.AI.TakeTurn(_gameData.Player, _gameData.GameMap, _gameData.Entities);
                        ProcessEvents();

                        if (_gameData.GameState == GameState.PlayerDead)
                        {
                            break;
                        }
                    }
                }

                if (_gameData.GameState != GameState.PlayerDead)
                {
                    _gameData.GameState = GameState.PlayersTurn;
                }

                ProcessEvents();
            }

            if (_gameData.GameState == GameState.Targeting)
            {
                _menuManager.HideInventoryMenu(_defaultConsole);

                if (Global.MouseState.LeftClicked)
                {
                    var targetPos = _mouse.MouseState.CellPosition;

                    _gameData.Player.Inventory.Use(_targetingItem, _gameData.Entities, _gameData.GameMap, targetPos.X, targetPos.Y);
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
                _gameData.GameMap.ComputeFov(_gameData.Player.X, _gameData.Player.Y, Constants.FovRadius, Constants.FovLightWalls, Constants.FovAlgorithm);
            }

            RenderFunctions.RenderAll(_defaultConsole, _panel, _gameData.Entities, _gameData.Player, _gameData.GameMap, _fovRecompute, _gameData.MessageLog, Constants.BarWidth, _mouse);
            _fovRecompute = false;
        }

        private static void ProcessEvents()
        {
            for (var index = 0; index < EventLog.Count; index++)
            {
                var @event = EventLog.Event(index);

                switch (@event)
                {
                    case DeadEvent dead:
                        if (dead.Entity == _gameData.Player)
                        {
                            DeathFunctions.KillPlayer(dead.Entity);
                            _gameData.GameState = GameState.PlayerDead;
                        }
                        else
                        {
                            DeathFunctions.KillMonster(dead.Entity);
                        }

                        break;
                    case ItemConsumedEvent consumed:
                        _gameData.GameState = GameState.EnemyTurn;

                        _gameData.MessageLog.AddMessage(consumed.Message);

                        break;
                    case ItemDroppedEvent dropped:
                        _gameData.Entities.Add(dropped.Item);

                        if (dropped.Entity == _gameData.Player)
                        {
                            _gameData.MessageLog.AddMessage($"You dropped the {dropped.Item.Name}!", Color.Yellow);
                        }
                        else
                        {
                            _gameData.MessageLog.AddMessage($"The {dropped.Entity.Name} dropped the {dropped.Item.Name}.", Color.Beige);
                        }

                        _gameData.GameState = GameState.EnemyTurn;

                        break;
                    case ItemPickupEvent pickup:
                        _gameData.Entities.Remove(pickup.Item);

                        if (pickup.Entity == _gameData.Player)
                        {
                            _gameData.MessageLog.AddMessage($"You pick up the {pickup.Item.Name}!", Color.Blue);
                        }
                        else
                        {
                            _gameData.MessageLog.AddMessage($"The {pickup.Entity.Name} picks up the {pickup.Item.Name}.", Color.Beige);
                        }

                        break;
                    case MessageEvent message:
                        _gameData.MessageLog.AddMessage(message.Message);
                        break;
                    case TargetingCanceledEvent _:
                        _gameData.GameState = _previousGameState;
                        _gameData.MessageLog.AddMessage("Targeting canceled.");
                        break;
                    case TargetingStartEvent targeting:
                        _previousGameState = GameState.PlayersTurn;
                        _gameData.GameState = GameState.Targeting;

                        _targetingItem = targeting.ItemEntity;

                        _gameData.MessageLog.AddMessage(_targetingItem.Item.TargetingMessage);
                        break;
                }
            }

            EventLog.Clear();
        }
    }
}
