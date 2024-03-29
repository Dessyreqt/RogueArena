﻿namespace RogueArena.Data.Components
{
    using RogueArena.Events;

    public class FighterComponent : Component
    {
        public int MaxHp => BaseMaxHp + (Owner.Get<EquipmentComponent>()?.MaxHpBonus ?? 0);
        public int Defense => BaseDefense + (Owner.Get<EquipmentComponent>()?.DefenseBonus ?? 0);
        public int Power => BasePower + (Owner.Get<EquipmentComponent>()?.PowerBonus ?? 0);
        public int BaseMaxHp { get; set; }
        public int BaseDefense { get; set; }
        public int BasePower { get; set; }
        public int Hp { get; set; }
        public int Xp { get; set; }

        public void TakeDamage(int amount, ProgramData data)
        {
            Hp -= amount;

            if (Hp <= 0)
            {
                Hp = 0;
                data.Events.Add(new DeadEvent(Owner));

                if (Owner != data.GameData.Player)
                {
                    data.Events.Add(new XpEvent(Xp));
                }
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
