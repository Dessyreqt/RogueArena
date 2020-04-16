namespace RogueArena.Data.Components.ItemFunctions
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using RogueArena.Events;

    public class HealFunction : ItemFunction
    {
        public int Amount { get; set; }

        public override List<Event> Execute(ProgramData data)
        {
            var events = new List<Event>();

            if (Target?.Get<FighterComponent>() == null)
            {
                return events;
            }

            if (Target.Get<FighterComponent>().Hp == Target.Get<FighterComponent>().MaxHp)
            {
                data.GameData.MessageLog.AddMessage("You are already at full health", Color.Yellow);
            }
            else
            {
                Target.Get<FighterComponent>().Heal(Amount);
                data.GameData.MessageLog.AddMessage("Your wounds start to feel better!", Color.Green);
                events.Add(new ItemConsumedEvent());
            }

            return events;
        }
    }
}
