namespace RogueArena.Components.ItemFunctions
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using RogueArena.Data;
    using RogueArena.Events;
    using RogueArena.Map;

    public class CastLightningFunction : ItemFunction
    {
        public CastLightningFunction(int damage, int maximumRange)
        {
            Damage = damage;
            MaximumRange = maximumRange;
        }

        public Entity Caster { get; set; }
        public int Damage { get; set; }
        public int MaximumRange { get; set; }


        public override List<Event> Execute()
        {
            var events = new List<Event>();

            Caster = Target;
            Target = null;
            double closestDistance = MaximumRange + 1;

            foreach (var entity in Entities)
            {
                if (entity.FighterComponent != null && entity != Caster && DungeonMap.Tiles[entity.X, entity.Y].InView)
                {
                    var distance = Caster.DistanceTo(entity);

                    if (distance < closestDistance)
                    {
                        Target = entity;
                        closestDistance = distance;
                    }
                }
            }

            if (Target != null)
            {
                events.Add(new ItemConsumedEvent($"A lighting bolt strikes the {Target.Name} with a loud thunder! The damage is {Damage}", Target));
                Target.FighterComponent.TakeDamage(Damage, events);
            }
            else
            {
                events.Add(new MessageEvent("No enemy is close enough to strike.", Color.Red));
            }

            return events;
        }
    }
}
