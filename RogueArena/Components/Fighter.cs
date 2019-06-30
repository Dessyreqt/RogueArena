namespace RogueArena.Components
{
    public class Fighter
    {
        public Fighter(int hp, int defense, int power)
        {
            MaxHp = hp;
            Hp = hp;
            Defense = defense;
            Power = power;
        }

        public Entity Owner { get; set; }
        public int MaxHp { get; private set; }
        public int Hp { get; private set; }
        public int Defense { get; private set; }
        public int Power { get; private set; }
    }
}