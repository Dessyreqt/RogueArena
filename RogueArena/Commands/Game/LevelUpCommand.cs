namespace RogueArena.Commands.Game
{
    public enum LevelUpType
    {
        Hp,
        Str,
        Def,
    }

    public class LevelUpCommand : Command
    {
        public LevelUpCommand(LevelUpType levelUpType)
        {
            LevelUpType = levelUpType;
        }

        public LevelUpType LevelUpType { get; set; }
    }
}
