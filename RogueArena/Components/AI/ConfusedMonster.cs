namespace RogueArena.Components.AI
{
    using Microsoft.Xna.Framework;
    using RogueArena.Data;
    using RogueArena.Events;
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

        public override void TakeTurn(Entity target, ProgramData data)
        {
            if (NumberOfTurns > 0)
            {
                var randomX = Owner.X + Rng.Next(3) - 1;
                var randomY = Owner.Y + Rng.Next(3) - 1;

                if (randomX != Owner.X && randomY != Owner.Y)
                {
                    Owner.MoveTowards(randomX, randomY, data.GameData.DungeonLevel.Map, data.GameData.Entities);
                }

                NumberOfTurns -= 1;
            }
            else
            {
                Owner.AiComponent = PreviousAi;
                data.Events.Add(new MessageEvent($"The {Owner.Name} is no longer confused!", Color.Red));
            }
        }
    }
}
