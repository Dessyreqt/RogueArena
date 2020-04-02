namespace RogueArena.Graphics
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SadConsole;
    using SadConsole.Components;
    using SadConsole.DrawCalls;
    using Console = SadConsole.Console;
    using Game = SadConsole.Game;

    public class BackgroundComponent : DrawConsoleComponent
    {
        private readonly Texture2D _texture;

        public BackgroundComponent(Texture2D texture)
        {
            _texture = texture;
        }

        public override void Draw(Console console, TimeSpan delta)
        {
            var texture = new DrawCallTexture(_texture, new Point(Game.Instance.Window.ClientBounds.Width / 2 - _texture.Width / 2, Game.Instance.Window.ClientBounds.Height / 2 - _texture.Height / 2).ToVector2());
            Global.DrawCalls.Add(texture);
        }
    }
}
