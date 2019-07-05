namespace RogueArena.Components
{
    using Events;

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

        public void TakeDamage(int amount)
        {
            Hp -= amount;

            if (Hp <= 0)
            {
                EventLog.Add(new DeadEvent(Owner));
            }
        }

        public void Attack(Entity target)
        {
            var damage = Power - target.Fighter.Defense;

            if (damage > 0)
            {
                target.Fighter.TakeDamage(damage);
                EventLog.Add(new MessageEvent($"{Owner.Name} attacks {target.Name} for {damage} hit points."));
            }
            else
            {
                EventLog.Add(new MessageEvent($"{Owner.Name} attacks {target.Name} but does no damage."));
            }
        }
    }
}