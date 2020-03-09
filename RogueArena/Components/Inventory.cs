namespace RogueArena.Components
{
    using System.Collections.Generic;
    using Events;
    using Microsoft.Xna.Framework;
    using RogueArena.Map;

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
                EventLog.Add(new MessageEvent("You cannot carry any more, your inventory is full.", Color.Yellow));
            }
            else
            {
                EventLog.Add(new ItemPickupEvent(Owner, entity));
                Items.Add(entity);
            }
        }

        public void RemoveItem(Entity entity)
        {
            Items.Remove(entity);
        }

        public void Use(Entity itemEntity, List<Entity> entities, GameMap gameMap, int? targetX = null, int? targetY = null)
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
                if (item.Targeting && (targetX == null || targetY == null))
                {
                    EventLog.Add(new TargetingStartEvent(itemEntity));
                }
                else
                {
                    item.ItemFunction.Target = Owner;
                    item.ItemFunction.Entities = entities;
                    item.ItemFunction.GameMap = gameMap;
                    item.ItemFunction.TargetX = targetX;
                    item.ItemFunction.TargetY = targetY;

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

        public void Drop(Entity itemEntity)
        {
            itemEntity.X = Owner.X;
            itemEntity.Y = Owner.Y;

            RemoveItem(itemEntity);
            EventLog.Add(new ItemDroppedEvent(Owner, itemEntity));
        }
    }
}