namespace RogueArena.Components
{
    using System.Collections.Generic;
    using Events;
    using Microsoft.Xna.Framework;

    public class Inventory
    {
        public Inventory(int capacity)
        {
            Capacity = capacity;
            Items = new List<Entity>();
        }

        public int Capacity { get; }
        public List<Entity> Items { get; }
        public Entity Owner { get; set; }

        public void AddItem(Entity entity)
        {
            if (Items.Count >= Capacity)
            {
                EventLog.Instance.Add(new MessageEvent("You cannot carry any more, your inventory is full.", Color.Yellow));
            }
            else
            {
                EventLog.Instance.Add(new ItemPickupEvent(Owner, entity));
                Items.Add(entity);
            }
        }

        public void Use(Entity itemEntity)
        {
            var item = itemEntity?.Item;

            if (item == null)
            {
                return;
            }

            if (item.ItemFunction == null)
            {
                EventLog.Add(new MessageEvent($"The {itemEntity.Name} cannot be used", Color.Yellow));
            }
            else
            {
                item.ItemFunction.Target = Owner;
                var events = item.ItemFunction.Execute();

                foreach (var @event in events)
                {
                    if (@event is ItemConsumedEvent)
                    {
                        Items.Remove(itemEntity);
                    }

                    EventLog.Add(@event);
                }
            }
        }
    }
}