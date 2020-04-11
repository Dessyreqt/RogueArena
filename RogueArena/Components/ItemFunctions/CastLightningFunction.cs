namespace RogueArena.Components.ItemFunctions
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using RogueArena.Data;
    using RogueArena.Events;

    public class CastLightningFunction : ItemFunction
    {
        public Entity Caster { get; set; }
        public int Damage { get; set; }
        public int MaximumRange { get; set; }

        public override List<Event> Execute(ProgramData data)
        {
            var events = new List<Event>();

            Caster = Target;
            Target = null;
            double closestDistance = MaximumRange + 1;

            foreach (var entity in Entities)
            {
                if (entity.Get<FighterComponent>() != null && entity != Caster && DungeonMap.Tiles[entity.X, entity.Y].InView)
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
                data.GameData.MessageLog.AddMessage($"A lighting bolt strikes the {Target.Name} with a loud thunder! The damage is {Damage}");
                events.Add(new ItemConsumedEvent());
                Target.Get<FighterComponent>().TakeDamage(Damage, data);
            }
            else
            {
                data.GameData.MessageLog.AddMessage("No enemy is close enough to strike.", Color.Red);
            }

            return events;
        }
    }
}
