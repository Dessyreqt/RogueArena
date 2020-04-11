namespace RogueArena.Components.AI
{
    using RogueArena.Data;

    public class BasicMonster : AiComponent
    {
        private int lastPlayerX = -1;
        private int lastPlayerY = -1;

        public override void TakeTurn(Entity target, ProgramData data)
        {
            if (data.GameData.DungeonLevel.Map.Tiles[Owner.X, Owner.Y].InView || (lastPlayerX > -1 && lastPlayerY > -1))
            {
                if (data.GameData.DungeonLevel.Map.Tiles[Owner.X, Owner.Y].InView)
                {
                    lastPlayerX = target.X;
                    lastPlayerY = target.Y;
                }

                if (Owner.DistanceTo(target) >= 2)
                {
                    Owner.MoveAstar(target, data.GameData.DungeonLevel.Map, data.GameData.Entities);
                }
                else if (target.Get<FighterComponent>().Hp > 0)
                {
                    Owner.Get<FighterComponent>().Attack(target, data);
                }
            }
        }
    }
}
