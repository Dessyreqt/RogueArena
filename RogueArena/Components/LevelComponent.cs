namespace RogueArena.Components
{
    public class LevelComponent : Component
    {
        public LevelComponent() : this(1)
        {
            
        }

        public LevelComponent(int currentLevel = 1, int currentXp = 0, int levelUpBase = 200, int levelUpFactor = 150)
        {
            CurrentLevel = currentLevel;
            CurrentXp = currentXp;
            LevelUpBase = levelUpBase;
            LevelUpFactor = levelUpFactor;
        }

        public int CurrentLevel { get; set; }
        public int CurrentXp { get; set; }
        public int LevelUpBase { get; set; }
        public int LevelUpFactor { get; set; }

        public int ExperienceToNextLevel => LevelUpBase + CurrentLevel * LevelUpFactor;

        public bool AddXp(int xp)
        {
            CurrentXp += xp;

            if (CurrentXp > ExperienceToNextLevel)
            {
                CurrentXp -= ExperienceToNextLevel;
                CurrentLevel += 1;

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

