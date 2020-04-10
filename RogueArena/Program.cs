namespace RogueArena
{
    using System;
    using System.Windows;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using RogueArena.Commands;
    using RogueArena.Commands.Game;
    using RogueArena.Commands.MainMenu;
    using RogueArena.Data;
    using RogueArena.Events;
    using RogueArena.Graphics;
    using SadConsole;
    using SadConsole.Input;
    using Console = SadConsole.Console;
    using Game = SadConsole.Game;

    class Program
    {
        private static ProgramData _data = new ProgramData();

        private static Console _defaultConsole;
        private static Console _panel;
        private static Texture2D _titleScreenTexture;
        private static BackgroundComponent _titleScreenBackground;

        private static MenuManager _menuManager;

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
            LoadWindowPosition();
            Game.Instance.Window.ClientSizeChanged += (sender, e) => { SaveWindowPosition(); };

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

            _data.FovRecompute = true;
            _data.ShowMainMenu = true;
            _data.ShowLoadErrorMessage = false;
        }

        private static void LoadContent()
        {
            _titleScreenTexture = Game.Instance.Content.Load<Texture2D>(@"Textures\menu_background");
            _titleScreenBackground = new BackgroundComponent(_titleScreenTexture);
        }

        private static void LoadWindowPosition()
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

        private static void SaveWindowPosition()
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
            if (_data.ShowMainMenu)
            {
                _menuManager.ShowMainMenu(_defaultConsole, Constants.ScreenWidth, Constants.ScreenHeight);
                _defaultConsole.Components.Add(_titleScreenBackground);
                HandleMainMenu();
            }
            else
            {
                _defaultConsole.Components.Remove(_titleScreenBackground);
                _menuManager.HideMainMenu(_defaultConsole);
                _menuManager.HideMessageBox(_defaultConsole);
                PlayGame();
            }

            if (_data.ShowLoadErrorMessage)
            {
                _menuManager.ShowMessageBox(_defaultConsole, "No save game to load", 50, Constants.ScreenWidth, Constants.ScreenHeight);
            }
        }

        private static void HandleMainMenu()
        {
            var command = InputHandler.HandleMainMenuKeys(Global.KeyboardState.KeysPressed);

            if (command != null)
            {
                _data.ShowLoadErrorMessage = false;
            }

            HandleCommand(command);
            ProcessEvents();
        }

        private static void PlayGame()
        {
            RenderFunctions.ClearAll(_defaultConsole, _data.GameData.Entities);

            if (Global.KeyboardState.KeysPressed.Count > 0)
            {
                var command = InputHandler.HandleGameKeys(Global.KeyboardState.KeysPressed, _data.GameData.GameState);

                HandleCommand(command);
                ProcessEvents();
            }

            if (_data.GameData.GameState == GameState.EnemyTurn)
            {
                _menuManager.HideInventoryMenu(_defaultConsole);
                _defaultConsole.Clear(0, 46, 80);

                foreach (var entity in _data.GameData.Entities)
                {
                    if (entity.AiComponent != null)
                    {
                        entity.AiComponent.TakeTurn(_data.GameData.Player, _data.GameData.DungeonLevel.Map, _data.GameData.Entities);
                        ProcessEvents();

                        if (_data.GameData.GameState == GameState.PlayerDead)
                        {
                            break;
                        }
                    }
                }

                if (_data.GameData.GameState != GameState.PlayerDead)
                {
                    _data.GameData.GameState = GameState.PlayersTurn;
                }

                ProcessEvents();
            }

            if (_data.GameData.GameState == GameState.Targeting)
            {
                _menuManager.HideInventoryMenu(_defaultConsole);

                if (Global.MouseState.LeftClicked)
                {
                    var targetPos = _mouse.MouseState.CellPosition;

                    _data.GameData.Player.InventoryComponent.Use(_targetingItem, _data.GameData.Entities, _data.GameData.DungeonLevel.Map, targetPos.X, targetPos.Y);
                    ProcessEvents();
                }
                else if (Global.MouseState.RightButtonDown)
                {
                    EventLog.Add(new TargetingCanceledEvent());
                    ProcessEvents();
                }
            }

            if (_data.FovRecompute)
            {
                _data.GameData.DungeonLevel.Map.ComputeFov(_data.GameData.Player.X, _data.GameData.Player.Y, Constants.FovRadius, Constants.FovLightWalls, Constants.FovAlgorithm);
            }

            RenderFunctions.RenderAll(
                _defaultConsole,
                _panel,
                _data.GameData.Entities,
                _data.GameData.Player,
                _data.GameData.DungeonLevel.Map,
                _data.FovRecompute,
                _data.GameData.MessageLog,
                Constants.BarWidth,
                _mouse);
            _data.FovRecompute = false;
        }

        private static void HandleCommand(Command command)
        {
            switch (command)
            {
                case NewGameCommand _:
                    _data.GameData = GameData.New();
                    _data.PreviousGameState = _data.GameData.GameState;
                    _data.ShowMainMenu = false;
                    break;
                case LoadSavedGameCommand _:
                    _data.GameData = GameData.Load();

                    if (_data.GameData == null)
                    {
                        _data.ShowLoadErrorMessage = true;
                    }
                    else
                    {
                        _data.PreviousGameState = _data.GameData.GameState;
                        _data.ShowMainMenu = false;
                    }

                    break;
                case ExitGameCommand _:
                    Game.Instance.Exit();
                    break;
                case DropInventoryCommand _:
                    _data.PreviousGameState = _data.GameData.GameState;
                    _data.GameData.GameState = GameState.DropInventory;
                    _menuManager.ShowInventoryMenu(
                        _defaultConsole,
                        "Press the key next to an item to drop it, or Esc to cancel.",
                        _data.GameData.Player,
                        50,
                        Constants.ScreenWidth,
                        Constants.ScreenHeight);

                    break;
                case ExitCommand _:
                    if (_data.GameData.GameState == GameState.ShowInventory || _data.GameData.GameState == GameState.DropInventory)
                    {
                        _data.GameData.GameState = _data.PreviousGameState;
                        _menuManager.HideInventoryMenu(_defaultConsole);
                    }
                    else if (_data.GameData.GameState == GameState.Targeting)
                    {
                        EventLog.Add(new TargetingCanceledEvent());
                    }
                    else if (_data.GameData.GameState == GameState.CharacterScreen)
                    {
                        _data.GameData.GameState = _data.PreviousGameState;
                        _menuManager.HideCharacterScreen(_defaultConsole);
                    }
                    else
                    {
                        _data.GameData.Save();
                        Game.Instance.Exit();
                    }

                    break;
                case InventoryIndexCommand inv:
                    if (_data.PreviousGameState != GameState.PlayerDead && inv.Index < _data.GameData.Player.InventoryComponent.Items.Count)
                    {
                        var item = _data.GameData.Player.InventoryComponent.Items[inv.Index];

                        if (_data.GameData.GameState == GameState.ShowInventory)
                        {
                            _data.GameData.Player.InventoryComponent.Use(item, _data.GameData.Entities, _data.GameData.DungeonLevel.Map);
                        }
                        else if (_data.GameData.GameState == GameState.DropInventory)
                        {
                            _data.GameData.Player.InventoryComponent.Drop(item);
                        }
                    }

                    break;
                case LevelUpCommand levelUp:
                    switch (levelUp.LevelUpType)
                    {
                        case LevelUpType.Hp:
                            _data.GameData.Player.FighterComponent.BaseMaxHp += 20;
                            _data.GameData.Player.FighterComponent.Hp += 20;
                            break;
                        case LevelUpType.Str:
                            _data.GameData.Player.FighterComponent.BasePower += 1;
                            break;
                        case LevelUpType.Def:
                            _data.GameData.Player.FighterComponent.BaseDefense += 1;
                            break;
                    }

                    _data.GameData.GameState = _data.PreviousGameState;
                    _menuManager.HideLevelUpMenu(_defaultConsole);
                    break;
                case MoveCommand move:
                    _defaultConsole.Clear(0, 45, 80);

                    if (_data.GameData.GameState == GameState.PlayersTurn)
                    {
                        var destX = _data.GameData.Player.X + move.X;
                        var destY = _data.GameData.Player.Y + move.Y;

                        if (!_data.GameData.DungeonLevel.Map.IsBlocked(destX, destY))
                        {
                            var target = Entity.GetBlockingEntityAtLocation(_data.GameData.Entities, destX, destY);

                            if (target != null)
                            {
                                _data.GameData.Player.FighterComponent.Attack(target);
                            }
                            else
                            {
                                _data.GameData.Player.Move(move.X, move.Y);
                                _data.FovRecompute = true;
                            }

                            _data.GameData.GameState = GameState.EnemyTurn;
                        }
                    }

                    break;
                case PickupCommand _:
                    if (_data.GameData.GameState == GameState.PlayersTurn)
                    {
                        Entity itemEntity = null;

                        foreach (var entity in _data.GameData.Entities)
                        {
                            if (entity.ItemComponent != null && entity.X == _data.GameData.Player.X && entity.Y == _data.GameData.Player.Y)
                            {
                                _data.GameData.Player.InventoryComponent.AddItem(entity);
                                itemEntity = entity;
                                break;
                            }
                        }

                        if (itemEntity != null)
                        {
                            _data.GameData.GameState = GameState.EnemyTurn;
                        }
                        else
                        {
                            EventLog.Add(new MessageEvent("There is nothing here to pick up.", Color.Yellow));
                        }
                    }

                    break;
                case RestCommand _:
                    if (_data.GameData.GameState == GameState.PlayersTurn)
                    {
                        _data.GameData.GameState = GameState.EnemyTurn;
                    }

                    break;
                case ShowCharacterScreenCommand _:
                    _data.PreviousGameState = _data.GameData.GameState;
                    _data.GameData.GameState = GameState.CharacterScreen;
                    _menuManager.ShowCharacterScreen(_defaultConsole, _data.GameData.Player, 30, 10, Constants.ScreenWidth, Constants.ScreenHeight);

                    break;
                case ShowInventoryCommand _:
                    _data.PreviousGameState = _data.GameData.GameState;
                    _data.GameData.GameState = GameState.ShowInventory;
                    _menuManager.ShowInventoryMenu(
                        _defaultConsole,
                        "Press the key next to an item to use it, or Esc to cancel.",
                        _data.GameData.Player,
                        50,
                        Constants.ScreenWidth,
                        Constants.ScreenHeight);

                    break;
                case TakeStairsCommand _:
                    Entity stairsEntity = null;

                    foreach (var entity in _data.GameData.Entities)
                    {
                        if (entity.StairsComponent != null && entity.X == _data.GameData.Player.X && entity.Y == _data.GameData.Player.Y)
                        {
                            stairsEntity = entity;
                            break;
                        }
                    }

                    if (stairsEntity != null)
                    {
                        _data.GameData.DungeonLevel = _data.GameData.DungeonLevel.GoToFloor(stairsEntity.StairsComponent.ToFloor, _data.GameData.Player, _data.GameData.MessageLog);
                        _data.GameData.Entities = _data.GameData.DungeonLevel.Entities;
                        _data.FovRecompute = true;
                        _defaultConsole.Clear();
                    }
                    else
                    {
                        EventLog.Add(new MessageEvent("There are no stairs here.", Color.Yellow));
                    }

                    break;
            }
        }

        private static void ProcessEvents()
        {
            for (var index = 0; index < EventLog.Count; index++)
            {
                var @event = EventLog.Event(index);

                switch (@event)
                {
                    case DeadEvent dead:
                        if (dead.Entity == _data.GameData.Player)
                        {
                            DeathFunctions.KillPlayer(dead.Entity);
                            _data.GameData.GameState = GameState.PlayerDead;
                        }
                        else
                        {
                            DeathFunctions.KillMonster(dead.Entity);
                        }

                        break;
                    case EquippedEvent equipped:
                        _data.GameData.MessageLog.AddMessage($"You equipped the {equipped.EquippedEntity.Name}.");
                        break;
                    case ItemConsumedEvent consumed:
                        if (_data.GameData.GameState == GameState.LevelUp)
                        {
                            _data.PreviousGameState = GameState.EnemyTurn;
                        }
                        else
                        {
                            _data.GameData.GameState = GameState.EnemyTurn;
                        }

                        _data.GameData.MessageLog.AddMessage(consumed.Message);

                        break;
                    case ItemDroppedEvent dropped:
                        _data.GameData.Entities.Add(dropped.Item);

                        if (dropped.Entity == _data.GameData.Player)
                        {
                            _data.GameData.MessageLog.AddMessage($"You dropped the {dropped.Item.Name}!", Color.Yellow);
                        }
                        else
                        {
                            _data.GameData.MessageLog.AddMessage($"The {dropped.Entity.Name} dropped the {dropped.Item.Name}.", Color.Beige);
                        }

                        _data.GameData.GameState = GameState.EnemyTurn;

                        break;
                    case ItemPickupEvent pickup:
                        _data.GameData.Entities.Remove(pickup.Item);

                        if (pickup.Entity == _data.GameData.Player)
                        {
                            _data.GameData.MessageLog.AddMessage($"You pick up the {pickup.Item.Name}!", Color.Blue);
                        }
                        else
                        {
                            _data.GameData.MessageLog.AddMessage($"The {pickup.Entity.Name} picks up the {pickup.Item.Name}.", Color.Beige);
                        }

                        break;
                    case MessageEvent message:
                        _data.GameData.MessageLog.AddMessage(message.Message);
                        break;
                    case TargetingCanceledEvent _:
                        _data.GameData.GameState = _data.PreviousGameState;
                        _data.GameData.MessageLog.AddMessage("Targeting canceled.");
                        break;
                    case TargetingStartEvent targeting:
                        _data.PreviousGameState = GameState.PlayersTurn;
                        _data.GameData.GameState = GameState.Targeting;

                        _targetingItem = targeting.ItemEntity;

                        _data.GameData.MessageLog.AddMessage(_targetingItem.ItemComponent.TargetingMessage);
                        break;
                    case ToggleEquipEvent equip:
                        _data.GameData.Player.EquipmentComponent.ToggleEquip(equip.EquippableEntity);
                        _data.GameData.GameState = GameState.EnemyTurn;
                        break;
                    case UnequippedEvent unequipped:
                        _data.GameData.MessageLog.AddMessage($"You unequipped the {unequipped.UnequippedEntity.Name}.");
                        break;
                    case XpEvent xp:
                        var leveledUp = _data.GameData.Player.LevelComponent.AddXp(xp.Xp);
                        _data.GameData.MessageLog.AddMessage($"You gain {xp.Xp} experience points.");

                        if (leveledUp)
                        {
                            _data.GameData.MessageLog.AddMessage($"Your battle skills grow stronger! You reached level {_data.GameData.Player.LevelComponent.CurrentLevel}!", Color.Yellow);
                            _data.PreviousGameState = _data.GameData.GameState;
                            _data.GameData.GameState = GameState.LevelUp;
                            _menuManager.ShowLevelUpMenu(_defaultConsole, "Level up! Choose a stat to raise:", _data.GameData.Player, 40, Constants.ScreenWidth, Constants.ScreenHeight);
                        }

                        break;
                }
            }

            EventLog.Clear();
        }
    }
}
