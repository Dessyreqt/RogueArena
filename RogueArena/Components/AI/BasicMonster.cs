namespace RogueArena.Components.AI
{
    using RogueArena.Data;

    public class BasicMonster : AiComponent
    {
        public override void TakeTurn(Entity target, ProgramData data)
        {
            if (data.GameData.DungeonLevel.Map.Tiles[Owner.X, Owner.Y].InView)
            {
                if (Owner.DistanceTo(target) >= 2)
                {
                    Owner.MoveAstar(target, data.GameData.DungeonLevel.Map, data.GameData.Entities);
                }
                else if (target.FighterComponent.Hp > 0)
                {
                    Owner.FighterComponent.Attack(target, data.Events);
                }
            }
        }
    }
}
