using System;
using SadConsole;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Console = SadConsole.Console;
using Game = SadConsole.Game;
using Microsoft.Xna.Framework.Input;

namespace RogueArena
{
    class Program
    {

        public const int Width = 80;
        public const int Height = 50;

        static void Main(string[] args)
        {
            // Setup the engine and create the main window.
            Game.Create("fonts\\C64.font", Width, Height);

            // Hook the start event so we can add consoles to the system.
            Game.OnInitialize = Init;
            Game.OnUpdate = Update;

            // Start the game.
            Game.Instance.Run();
            Game.Instance.Dispose();
        }

        private static void Init()
        {
        }

        private static void Update(GameTime gameTime)
        {
            var console = Global.CurrentScreen;
            console.DefaultForeground = Color.White;
            console.Print(1, 1, "@");

            if (SadConsole.Global.KeyboardState.KeysPressed.Count > 0)
            {
                var key = Global.KeyboardState.KeysPressed[0].Key;

                if (key == Keys.Escape)
                {
                    Game.Instance.Exit();
                }
            }

        }
    }
}
