namespace RogueArena.Components.AI
{
    using System.Collections.Generic;
    using RogueArena.Map;

    public class BasicMonster : AiComponent
    {
        public override void TakeTurn(Entity target, DungeonMap dungeonMap, List<Entity> entities)
        {
            if (dungeonMap.Tiles[Owner.X, Owner.Y].InView)
            {
                if (Owner.DistanceTo(target) >= 2)
                {
                    Owner.MoveAstar(target, dungeonMap, entities);
                }
                else if (target.FighterComponent.Hp > 0)
                {
                    Owner.FighterComponent.Attack(target);
                }
            }
        }
    }
}
