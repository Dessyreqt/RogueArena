namespace RogueArena.Components.AI
{
    using RogueArena.Data;

    public class BasicMonster : AiComponent
    {
        public BasicMonster()
        {
            LastPlayerX = -1;
            LastPlayerY = -1;
        }

        public int LastPlayerX { get; set; }
        public int LastPlayerY { get; set; }

        public override void TakeTurn(Entity target, ProgramData data)
        {
            if (data.GameData.DungeonLevel.Map.Tiles[Owner.X, Owner.Y].InView || (LastPlayerX > -1 && LastPlayerY > -1))
            {
                if (data.GameData.DungeonLevel.Map.Tiles[Owner.X, Owner.Y].InView)
                {
                    LastPlayerX = target.X;
                    LastPlayerY = target.Y;
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
