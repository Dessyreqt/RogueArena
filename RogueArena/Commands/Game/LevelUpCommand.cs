namespace RogueArena.Commands.Game
{
    using RogueArena.Data;
    using RogueArena.Data.Components;

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
            var fighter = data.GameData.Player.Get<FighterComponent>();

            switch (_levelUpType)
            {
                case LevelUpType.Hp:
                    fighter.BaseMaxHp += 20;
                    fighter.Hp += 20;
                    break;
                case LevelUpType.Str:
                    fighter.BasePower += 1;
                    break;
                case LevelUpType.Def:
                    fighter.BaseDefense += 1;
                    break;
            }

            data.GameData.GameState = data.PreviousGameState;
            data.MenuManager.HideLevelUpMenu(data.DefaultConsole);
        }
    }
}
