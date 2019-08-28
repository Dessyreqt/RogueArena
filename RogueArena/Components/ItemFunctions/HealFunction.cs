namespace RogueArena.Components.ItemFunctions
{
    using System.Collections.Generic;
    using Events;
    using Microsoft.Xna.Framework;

    public class HealFunction : ItemFunction
    {
        public HealFunction(int amount)
        {
            Amount = amount;
        }

        public int Amount { get; }

        public override List<Event> Execute()
        {
            var events = new List<Event>();

            if (Target?.Fighter == null)
            {
                return events;
            }

            if (Target.Fighter.Hp == Target.Fighter.MaxHp)
            {
                events.Add(new MessageEvent("You are already at full health", Color.Yellow));
            }
            else
            {
                Target.Fighter.Heal(Amount);
                events.Add(new ItemConsumedEvent("Your wounds start to feel better!", Color.Green));
            }

            return events;
        }
    }
}