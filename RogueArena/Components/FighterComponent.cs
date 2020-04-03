namespace RogueArena.Components
{
    using Events;

    public class FighterComponent : Component
    {
        public FighterComponent()
        {
            // here for deserialization purposes
        }

        public FighterComponent(int hp, int defense, int power)
        {
            MaxHp = hp;
            Hp = hp;
            Defense = defense;
            Power = power;
        }

        public int MaxHp { get; set; }
        public int Hp { get; set; }
        public int Defense { get; set; }
        public int Power { get; set; }

        public void TakeDamage(int amount)
        {
            Hp -= amount;

            if (Hp <= 0)
            {
                Hp = 0;
                EventLog.Add(new DeadEvent(Owner));
            }
        }

        public void Attack(Entity target)
        {
            var damage = Power - target.FighterComponent.Defense;

            if (damage > 0)
            {
                target.FighterComponent.TakeDamage(damage);
                EventLog.Add(new MessageEvent($"{Owner.Name} attacks {target.Name} for {damage} hit points."));
            }
            else
            {
                EventLog.Add(new MessageEvent($"{Owner.Name} attacks {target.Name} but does no damage."));
            }
        }

        public void Heal(int amount)
        {
            Hp += amount;

            if (Hp > MaxHp)
            {
                Hp = MaxHp;
            }
        }
    }
}