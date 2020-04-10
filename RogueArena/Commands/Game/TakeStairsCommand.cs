namespace RogueArena.Commands.Game
{
    using Microsoft.Xna.Framework;
    using RogueArena.Data;
    using RogueArena.Events;

    public class TakeStairsCommand : Command
    {
        protected override void Handle(ProgramData data)
        {
            Entity stairsEntity = null;

            foreach (var entity in data.GameData.Entities)
            {
                if (entity.StairsComponent != null && entity.X == data.GameData.Player.X && entity.Y == data.GameData.Player.Y)
                {
                    stairsEntity = entity;
                    break;
                }
            }

            if (stairsEntity != null)
            {
                data.GameData.DungeonLevel = data.GameData.DungeonLevel.GoToFloor(stairsEntity.StairsComponent.ToFloor, data.GameData.Player, data.GameData.MessageLog);
                data.GameData.Entities = data.GameData.DungeonLevel.Entities;
                data.FovRecompute = true;
                data.DefaultConsole.Clear();
            }
            else
            {
                EventLog.Add(new MessageEvent("There are no stairs here.", Color.Yellow));
            }
        }
    }
}
