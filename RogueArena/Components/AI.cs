namespace RogueArena.Components
{
    using System.Collections.Generic;
    using Map;

    public interface AI
    {
        Entity Owner { get; set; }
        string TakeTurn(Entity target, GameMap gameMap, List<Entity> entities);
    }

    public class BasicMonster : AI
    {
        public Entity Owner { get; set; }

        public string TakeTurn(Entity target, GameMap gameMap, List<Entity> entities)
        {
            if (gameMap.Tiles[Owner.X, Owner.Y].InView)
            {
                if (Owner.DistanceTo(target) >= 2)
                {
                    Owner.MoveAstar(target, gameMap, entities);
                    return string.Empty;
                }
                else if (target.Fighter.Hp > 0)
                {
                    return $"The {Owner.Name} insults you! Your ego is damaged!";
                }
            }

            return string.Empty;
        }
    }
}