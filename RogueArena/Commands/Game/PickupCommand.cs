namespace RogueArena.Commands.Game
{
    using Microsoft.Xna.Framework;
    using RogueArena.Data;
    using RogueArena.Events;

    public class PickupCommand : Command
    {
        protected override void Handle(ProgramData data)
        {
            if (data.GameData.GameState == GameState.PlayersTurn)
            {
                Entity itemEntity = null;

                foreach (var entity in data.GameData.Entities)
                {
                    if (entity.ItemComponent != null && entity.X == data.GameData.Player.X && entity.Y == data.GameData.Player.Y)
                    {
                        data.GameData.Player.InventoryComponent.AddItem(entity, data);
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
                    data.Events.Add(new MessageEvent("There is nothing here to pick up.", Color.Yellow));
                }
            }
        }
    }
}
