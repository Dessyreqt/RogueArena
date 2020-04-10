namespace RogueArena.Components.ItemFunctions
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using RogueArena.Data;
    using RogueArena.Events;

    public class CastFireballFunction : ItemFunction
    {
        public CastFireballFunction(int damage, int radius)
        {
            Damage = damage;
            Radius = radius;
        }

        public int Damage { get; set; }
        public int Radius { get; set; }

        public override List<Event> Execute()
        {
            var events = new List<Event>();

            if (TargetX == null || TargetY == null)
            {
                events.Add(new MessageEvent("You must pick a target before using this item.", Color.Yellow));
                return events;
            }

            var x = TargetX.Value;
            var y = TargetY.Value;

            if (!DungeonMap.Tiles[x, y].InView)
            {
                events.Add(new MessageEvent("You cannot target a tile outside your field of view.", Color.Yellow));
                return events;
            }

            events.Add(new ItemConsumedEvent($"The fireball explodes, burning everything within {Radius} tiles!", Color.Orange));

            foreach (var entity in Entities)
            {
                if (entity.DistanceTo(x, y) <= Radius && entity.FighterComponent != null)
                {
                    events.Add(new MessageEvent($"The {entity.Name} gets burned for {Damage} hit points.", Color.Orange));
                    entity.FighterComponent.TakeDamage(Damage, events);
                }
            }

            return events;
        }
    }
}
