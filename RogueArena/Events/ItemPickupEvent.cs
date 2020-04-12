namespace RogueArena.Events
{
    using Microsoft.Xna.Framework;
    using RogueArena.Data;

    public class ItemPickupEvent : Event
    {
        private readonly Entity _item;
        private readonly Entity _entity;

        public ItemPickupEvent(Entity entity, Entity item)
        {
            _entity = entity;
            _item = item;
        }

        protected override void Handle(ProgramData data)
        {
            data.GameData.RemoveEntity(_item);

            if (_entity == data.GameData.Player)
            {
                data.GameData.MessageLog.AddMessage($"You pick up the {_item.Name}!", Color.Blue);
            }
            else
            {
                data.GameData.MessageLog.AddMessage($"The {_entity.Name} picks up the {_item.Name}.", Color.Beige);
            }
        }
    }
}
