namespace RogueArena.Components
{
    using System.Collections.Generic;
    using Events;
    using Microsoft.Xna.Framework;
    using RogueArena.Data;
    using RogueArena.Map;

    public class InventoryComponent : Component
    {
        public InventoryComponent()
        {
            // here for deserialization purposes
        }

        public InventoryComponent(int capacity)
        {
            Capacity = capacity;
            Items = new List<Entity>();
        }

        public int Capacity { get; set; }
        public List<Entity> Items { get; set; }

        public void AddItem(Entity entity, ProgramData data)
        {
            if (Items.Count >= Capacity)
            {
                data.Events.Add(new MessageEvent("You cannot carry any more, your inventory is full.", Color.Yellow));
            }
            else
            {
                data.Events.Add(new ItemPickupEvent(Owner, entity));
                Items.Add(entity);
            }
        }

        public void RemoveItem(Entity entity)
        {
            Items.Remove(entity);
        }

        public void Use(Entity itemEntity, ProgramData data, int? targetX = null, int? targetY = null)
        {
            var item = itemEntity?.Get<ItemComponent>();

            if (item == null)
            {
                return;
            }

            if (item.ItemFunction == null)
            {
                if (itemEntity.Get<EquippableComponent>() != null)
                {
                    data.Events.Add(new ToggleEquipEvent(itemEntity));
                }
                else
                {
                    data.Events.Add(new MessageEvent($"The {itemEntity.Name} cannot be used", Color.Yellow));
                }
            }
            else
            {
                if (item.Targeting && (targetX == null || targetY == null))
                {
                    data.Events.Add(new TargetingStartEvent(itemEntity));
                }
                else
                {
                    item.ItemFunction.Target = Owner;
                    item.ItemFunction.Entities = data.GameData.Entities;
                    item.ItemFunction.DungeonMap = data.GameData.DungeonLevel.Map;
                    item.ItemFunction.TargetX = targetX;
                    item.ItemFunction.TargetY = targetY;

                    var events = item.ItemFunction.Execute();

                    foreach (var @event in events)
                    {
                        if (@event is ItemConsumedEvent)
                        {
                            Items.Remove(itemEntity);
                        }

                        data.Events.Add(@event);
                    }
                }
            }
        }

        public void Drop(Entity itemEntity, ProgramData data)
        {
            if (Owner.Get<EquipmentComponent>().MainHand == itemEntity || Owner.Get<EquipmentComponent>().OffHand == itemEntity)
            {
                Owner.Get<EquipmentComponent>().ToggleEquip(itemEntity, data);
            }

            itemEntity.X = Owner.X;
            itemEntity.Y = Owner.Y;

            RemoveItem(itemEntity);
            data.Events.Add(new ItemDroppedEvent(Owner, itemEntity));
        }
    }
}