namespace RogueArena.Commands.Game
{
    using Microsoft.Xna.Framework;
    using RogueArena.Components;
    using RogueArena.Data;

    public class TakeStairsCommand : Command
    {
        protected override void Handle(ProgramData data)
        {
            Entity stairsEntity = null;
            StairsComponent stairsComponent = null;

            foreach (var entity in Ecs.EntitiesWithComponent[typeof(StairsComponent)])
            {
                stairsComponent = entity.Get<StairsComponent>();

                if (entity.X == data.GameData.Player.X && entity.Y == data.GameData.Player.Y)
                {
                    stairsEntity = entity;
                    break;
                }
            }

            if (stairsEntity != null)
            {
                data.GameData.DungeonLevel = data.GameData.DungeonLevel.GoToFloor(stairsComponent.ToFloor, data.GameData.Player, data.GameData.MessageLog);
                data.GameData.SetEntities(data.GameData.DungeonLevel.Entities);
                data.FovRecompute = true;
                data.DefaultConsole.Clear();
            }
            else
            {
                data.GameData.MessageLog.AddMessage("There are no stairs here.", Color.Yellow);
            }
        }
    }
}
