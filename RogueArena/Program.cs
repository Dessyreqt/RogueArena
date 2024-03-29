﻿namespace RogueArena
{
    using System;
    using System.Windows;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using RogueArena.Commands;
    using RogueArena.Data;
    using RogueArena.Data.Components;
    using RogueArena.Data.Lookup;
    using RogueArena.Events;
    using RogueArena.Graphics;
    using SadConsole;
    using SadConsole.Input;
    using Console = SadConsole.Console;
    using Game = SadConsole.Game;
    using Point = Microsoft.Xna.Framework.Point;

    class Program
    {
        private static ProgramData _data = new ProgramData();

        private static Console _panel;
        private static Texture2D _titleScreenTexture;
        private static BackgroundComponent _titleScreenBackground;

        private static MouseEventArgs _mouse;

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

            _data.DefaultConsole = new Console(Constants.ScreenWidth, Constants.ScreenHeight);
            _data.DefaultConsole.DefaultForeground = Color.White;
            _data.DefaultConsole.DefaultBackground = Color.Transparent;
            _data.DefaultConsole.IsCursorDisabled = true;
            _data.DefaultConsole.MouseMove += Console_MouseMove;

            _panel = new Console(Constants.ScreenWidth, Constants.PanelHeight) { Position = new Point(0, Constants.PanelY) };

            _data.DefaultConsole.Children.Add(_panel);

            _data.MenuManager = new MenuManager();

            Global.CurrentScreen = _data.DefaultConsole;
            Global.FocusedConsoles.Set(_data.DefaultConsole);

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
                _data.MenuManager.ShowMainMenu(_data.DefaultConsole, Constants.ScreenWidth, Constants.ScreenHeight);
                _data.DefaultConsole.Components.Add(_titleScreenBackground);
                HandleMainMenu();
            }
            else
            {
                _data.DefaultConsole.Components.Remove(_titleScreenBackground);
                _data.MenuManager.HideMainMenu(_data.DefaultConsole);
                _data.MenuManager.HideMessageBox(_data.DefaultConsole);
                PlayGame();
            }

            if (_data.ShowLoadErrorMessage)
            {
                _data.MenuManager.ShowMessageBox(_data.DefaultConsole, "No save game to load", 50, Constants.ScreenWidth, Constants.ScreenHeight);
            }
        }

        private static void HandleMainMenu()
        {
            var command = InputHandler.HandleMainMenuKeys(Global.KeyboardState.KeysPressed);

            if (command != null)
            {
                _data.ShowLoadErrorMessage = false;
            }

            Command.Run(command, _data);
            ProcessEvents();
        }

        private static void PlayGame()
        {
            RenderFunctions.ClearAll(_data.DefaultConsole, _data.GameData.Entities);

            if (Global.KeyboardState.KeysPressed.Count > 0)
            {
                var command = InputHandler.HandleGameKeys(Global.KeyboardState.KeysPressed, _data.GameData.GameState);

                Command.Run(command, _data);
                ProcessEvents();
            }

            if (_data.GameData.GameState == GameState.EnemyTurn)
            {
                _data.MenuManager.HideInventoryMenu(_data.DefaultConsole);
                _data.DefaultConsole.Clear(0, 46, 80);

                foreach (var entity in Entities.With<AiComponent>())
                {
                    entity.Get<AiComponent>().TakeTurn(_data.GameData.Player, _data);
                    ProcessEvents();

                    if (_data.GameData.GameState == GameState.PlayerDead)
                    {
                        break;
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
                _data.MenuManager.HideInventoryMenu(_data.DefaultConsole);

                if (Global.MouseState.LeftClicked)
                {
                    var targetPos = _mouse.MouseState.CellPosition;

                    _data.GameData.Player.Get<InventoryComponent>().Use(_data.TargetingItem, _data, targetPos.X, targetPos.Y);
                    ProcessEvents();
                }
                else if (Global.MouseState.RightButtonDown)
                {
                    _data.Events.Add(new TargetingCanceledEvent());
                    ProcessEvents();
                }
            }

            if (_data.FovRecompute)
            {
                _data.GameData.DungeonLevel.Map.ComputeFov(_data.GameData.Player.X, _data.GameData.Player.Y, Constants.FovRadius, Constants.FovLightWalls, Constants.FovAlgorithm);
            }

            RenderFunctions.RenderAll(
                _data.DefaultConsole,
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

        private static void ProcessEvents()
        {
            for (var index = 0; index < _data.Events.Count; index++)
            {
                var @event = _data.Events[index];
                Event.Process(@event, _data);
            }

            _data.Events.Clear();
        }
    }
}
