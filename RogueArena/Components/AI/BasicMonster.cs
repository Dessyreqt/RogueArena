namespace RogueArena.Components.AI
{
    using System.Collections.Generic;
    using RogueArena.Map;

    public class BasicMonster : AI
    {
        public override void TakeTurn(Entity target, GameMap gameMap, List<Entity> entities)
        {
            if (gameMap.Tiles[Owner.X, Owner.Y].InView)
            {
                if (Owner.DistanceTo(target) >= 2)
                {
                    Owner.MoveAstar(target, gameMap, entities);
                }
                else if (target.Fighter.Hp > 0)
                {
                    Owner.Fighter.Attack(target);
                }
            }
        }
    }
}
