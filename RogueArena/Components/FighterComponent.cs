namespace RogueArena.Components
{
    using System.Collections.Generic;
    using Events;
    using RogueArena.Data;

    public class FighterComponent : Component
    {
        public FighterComponent()
        {
            // here for deserialization purposes
        }

        public FighterComponent(int hp, int defense, int power, int xp = 0)
        {
            BaseMaxHp = hp;
            Hp = hp;
            BaseDefense = defense;
            BasePower = power;
            Xp = xp;
        }

        public int BaseMaxHp { get; set; }
        public int BaseDefense { get; set; }
        public int BasePower { get; set; }
        public int MaxHp => BaseMaxHp + (Owner.Get<EquipmentComponent>()?.MaxHpBonus ?? 0);
        public int Hp { get; set; }
        public int Defense => BaseDefense + (Owner.Get<EquipmentComponent>()?.DefenseBonus ?? 0);
        public int Power => BasePower + (Owner.Get<EquipmentComponent>()?.PowerBonus ?? 0);
        public int Xp { get; set; }

        public void TakeDamage(int amount, ProgramData data)
        {
            Hp -= amount;

            if (Hp <= 0)
            {
                Hp = 0;
                data.Events.Add(new DeadEvent(Owner));
                data.Events.Add(new XpEvent(Xp));
            }
        }

        public void Attack(Entity target, ProgramData data)
        {
            var damage = Power - target.Get<FighterComponent>().Defense;

            if (damage > 0)
            {
                target.Get<FighterComponent>().TakeDamage(damage, data);
                data.GameData.MessageLog.AddMessage($"{Owner.Name} attacks {target.Name} for {damage} hit points.");
            }
            else
            {
                data.GameData.MessageLog.AddMessage($"{Owner.Name} attacks {target.Name} but does no damage.");
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