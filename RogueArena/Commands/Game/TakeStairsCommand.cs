namespace RogueArena.Commands.Game
{
    using Microsoft.Xna.Framework;
    using RogueArena.Components;
    using RogueArena.Data;
    using RogueArena.Events;

    public class TakeStairsCommand : Command
    {
        protected override void Handle(ProgramData data)
        {
            Entity stairsEntity = null;
            StairsComponent stairsComponent = null;

            foreach (var entity in data.GameData.Entities)
            {
                stairsComponent = entity.Get<StairsComponent>();

                if (stairsComponent != null && entity.X == data.GameData.Player.X && entity.Y == data.GameData.Player.Y)
                {
                    stairsEntity = entity;
                    break;
                }
            }

            if (stairsEntity != null)
            {
                data.GameData.DungeonLevel = data.GameData.DungeonLevel.GoToFloor(stairsComponent.ToFloor, data.GameData.Player, data.GameData.MessageLog);
                data.GameData.Entities = data.GameData.DungeonLevel.Entities;
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
