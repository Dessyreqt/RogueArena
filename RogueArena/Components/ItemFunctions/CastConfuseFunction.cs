namespace RogueArena.Components.ItemFunctions
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Xna.Framework;
    using RogueArena.Components.AI;
    using RogueArena.Data;
    using RogueArena.Events;

    public class CastConfuseFunction : ItemFunction
    {
        public override List<Event> Execute(ProgramData data)
        {
            var events = new List<Event>();

            if (TargetX == null || TargetY == null)
            {
                data.GameData.MessageLog.AddMessage("You must pick a target before using this item.", Color.Yellow);
                return events;
            }

            var x = TargetX.Value;
            var y = TargetY.Value;

            if (!DungeonMap.Tiles[x, y].InView)
            {
                data.GameData.MessageLog.AddMessage("You cannot target a tile outside your field of view.", Color.Yellow);
                return events;
            }

            var targetedEntity = Entities.FirstOrDefault(entity => entity.X == x && entity.Y == y && entity.Get<AiComponent>() != null);

            if (targetedEntity == null)
            {
                data.GameData.MessageLog.AddMessage("There is no targetable enemy at that location.", Color.Yellow);
            }
            else
            {
                var confusedAi = new ConfusedMonster(targetedEntity.Get<AiComponent>(), 10);

                confusedAi.Owner = targetedEntity;
                targetedEntity.Add(confusedAi);

                data.GameData.MessageLog.AddMessage($"The eyes of the {targetedEntity.Name} look vacant, as he starts to stumble around!", Color.LightGreen);
                events.Add(new ItemConsumedEvent());
            }

            return events;
        }
    }
}
