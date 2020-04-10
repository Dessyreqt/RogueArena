namespace RogueArena.Components.ItemFunctions
{
    using System.Collections.Generic;
    using Events;
    using Microsoft.Xna.Framework;
    using RogueArena.Data;

    public class HealFunction : ItemFunction
    {
        public HealFunction()
        {
            // here for deserialization purposes
        }

        public HealFunction(int amount)
        {
            Amount = amount;
        }

        public int Amount { get; set; }

        public override List<Event> Execute()
        {
            var events = new List<Event>();

            if (Target?.FighterComponent == null)
            {
                return events;
            }

            if (Target.FighterComponent.Hp == Target.FighterComponent.MaxHp)
            {
                events.Add(new MessageEvent("You are already at full health", Color.Yellow));
            }
            else
            {
                Target.FighterComponent.Heal(Amount);
                events.Add(new ItemConsumedEvent("Your wounds start to feel better!", Color.Green));
            }

            return events;
        }
    }
}