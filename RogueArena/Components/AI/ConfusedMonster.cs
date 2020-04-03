namespace RogueArena.Components.AI
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using RogueArena.Events;
    using RogueArena.Map;
    using RogueArena.Rng;

    public class ConfusedMonster : AiComponent
    {
        public ConfusedMonster(AiComponent previousAi, int numberOfTurns)
        {
            PreviousAi = previousAi;
            NumberOfTurns = numberOfTurns;
        }

        public AiComponent PreviousAi { get; set; }
        public int NumberOfTurns { get; set; }

        public override void TakeTurn(Entity target, DungeonMap dungeonMap, List<Entity> entities)
        {
            if (NumberOfTurns > 0)
            {
                var randomX = Owner.X + Rng.Next(2) - 1;
                var randomY = Owner.Y + Rng.Next(2) - 1;

                if (randomX != Owner.X && randomY != Owner.Y)
                {
                    Owner.MoveTowards(randomX, randomY, dungeonMap, entities);
                }

                NumberOfTurns -= 1;
            }
            else
            {
                Owner.AiComponent = PreviousAi;
                EventLog.Add(new MessageEvent($"The {Owner.Name} is no longer confused!", Color.Red));
            }
        }
    }
}
