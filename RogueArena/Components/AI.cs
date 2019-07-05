namespace RogueArena.Components
{
    using System.Collections.Generic;
    using Map;

    public interface AI
    {
        Entity Owner { get; set; }
        void TakeTurn(Entity target, GameMap gameMap, List<Entity> entities);
    }

    public class BasicMonster : AI
    {
        public Entity Owner { get; set; }

        public void TakeTurn(Entity target, GameMap gameMap, List<Entity> entities)
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