namespace RogueArena.Events
{
    using Microsoft.Xna.Framework;
    using RogueArena.Data;

    public class ItemDroppedEvent : Event
    {
        private readonly Entity _item;
        private readonly Entity _entity;

        public ItemDroppedEvent(Entity entity, Entity item)
        {
            _entity = entity;
            _item = item;
        }

        protected override void Handle(ProgramData data)
        {
            data.GameData.Entities.Add(_item);

            if (_entity == data.GameData.Player)
            {
                data.GameData.MessageLog.AddMessage($"You dropped the {_item.Name}!", Color.Yellow);
            }
            else
            {
                data.GameData.MessageLog.AddMessage($"The {_entity.Name} dropped the {_item.Name}.", Color.Beige);
            }

            data.GameData.GameState = GameState.EnemyTurn;
        }
    }
}
