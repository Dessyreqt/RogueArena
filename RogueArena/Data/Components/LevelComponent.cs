namespace RogueArena.Data.Components
{
    public class LevelComponent : Component
    {
        public LevelComponent()
        {
            CurrentLevel = 1;
            CurrentXp = 0;
            LevelUpBase = 200;
            LevelUpFactor = 150;
        }

        public int ExperienceToNextLevel => LevelUpBase + CurrentLevel * LevelUpFactor;

        public int CurrentLevel { get; set; }
        public int CurrentXp { get; set; }
        public int LevelUpBase { get; set; }
        public int LevelUpFactor { get; set; }

        public bool AddXp(int xp)
        {
            CurrentXp += xp;

            if (CurrentXp > ExperienceToNextLevel)
            {
                CurrentXp -= ExperienceToNextLevel;
                CurrentLevel += 1;

                return true;
            }

            return false;
        }
    }
}
