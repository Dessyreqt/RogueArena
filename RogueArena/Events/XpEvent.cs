namespace RogueArena.Events
{
    using Microsoft.Xna.Framework;
    using RogueArena.Data;
    using RogueArena.Data.Components;

    public class XpEvent : Event
    {
        private readonly int _xp;

        public XpEvent(int xp)
        {
            _xp = xp;
        }

        protected override void Handle(ProgramData data)
        {
            var leveledUp = data.GameData.Player.Get<LevelComponent>().AddXp(_xp);
            data.GameData.MessageLog.AddMessage($"You gain {_xp} experience points.");

            if (leveledUp)
            {
                data.GameData.MessageLog.AddMessage(
                    $"Your battle skills grow stronger! You reached level {data.GameData.Player.Get<LevelComponent>().CurrentLevel}!",
                    Color.Yellow);
                data.PreviousGameState = data.GameData.GameState;
                data.GameData.GameState = GameState.LevelUp;
                data.MenuManager.ShowLevelUpMenu(data.DefaultConsole, "Level up! Choose a stat to raise:", data.GameData.Player, 40, Constants.ScreenWidth, Constants.ScreenHeight);
            }
        }
    }
}
