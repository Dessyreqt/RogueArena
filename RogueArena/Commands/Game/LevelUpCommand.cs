namespace RogueArena.Commands.Game
{
    using RogueArena.Data;

    public enum LevelUpType
    {
        Hp,
        Str,
        Def,
    }

    public class LevelUpCommand : Command
    {
        private readonly LevelUpType _levelUpType;

        public LevelUpCommand(LevelUpType levelUpType)
        {
            _levelUpType = levelUpType;
        }

        protected override void Handle(ProgramData data)
        {
            switch (_levelUpType)
            {
                case LevelUpType.Hp:
                    data.GameData.Player.FighterComponent.BaseMaxHp += 20;
                    data.GameData.Player.FighterComponent.Hp += 20;
                    break;
                case LevelUpType.Str:
                    data.GameData.Player.FighterComponent.BasePower += 1;
                    break;
                case LevelUpType.Def:
                    data.GameData.Player.FighterComponent.BaseDefense += 1;
                    break;
            }

            data.GameData.GameState = data.PreviousGameState;
            data.MenuManager.HideLevelUpMenu(data.DefaultConsole);
        }
    }
}
