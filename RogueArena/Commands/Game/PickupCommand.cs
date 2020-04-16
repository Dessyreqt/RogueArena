namespace RogueArena.Commands.Game
{
    using Microsoft.Xna.Framework;
    using RogueArena.Data;
    using RogueArena.Data.Components;

    public class PickupCommand : Command
    {
        protected override void Handle(ProgramData data)
        {
            if (data.GameData.GameState == GameState.PlayersTurn)
            {
                Entity itemEntity = null;

                foreach (var entity in Entities.With<ItemComponent>())
                {
                    if (entity.X == data.GameData.Player.X && entity.Y == data.GameData.Player.Y)
                    {
                        data.GameData.Player.Get<InventoryComponent>().AddItem(entity, data);
                        itemEntity = entity;
                        break;
                    }
                }

                if (itemEntity != null)
                {
                    data.GameData.GameState = GameState.EnemyTurn;
                }
                else
                {
                    data.GameData.MessageLog.AddMessage("There is nothing here to pick up.", Color.Yellow);
                }
            }
        }
    }
}
