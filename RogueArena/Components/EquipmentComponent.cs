namespace RogueArena.Components
{
    using RogueArena.Data;
    using RogueArena.Events;

    public class EquipmentComponent : Component
    {
        public int MaxHpBonus => (MainHand?.EquippableComponent?.MaxHpBonus ?? 0) + (OffHand?.EquippableComponent?.MaxHpBonus ?? 0);
        public int PowerBonus => (MainHand?.EquippableComponent?.PowerBonus ?? 0) + (OffHand?.EquippableComponent?.PowerBonus ?? 0);
        public int DefenseBonus => (MainHand?.EquippableComponent?.DefenseBonus ?? 0) + (OffHand?.EquippableComponent?.DefenseBonus ?? 0);
        public Entity MainHand { get; set; }
        public Entity OffHand { get; set; }

        public void ToggleEquip(Entity equippableEntity)
        {
            if (equippableEntity.EquippableComponent == null)
            {
                return;
            }

            var slot = equippableEntity.EquippableComponent.Slot;
            switch (slot)
            {
                case EquipmentSlot.MainHand:
                    if (MainHand == equippableEntity)
                    {
                        MainHand = null;
                        EventLog.Add(new UnequippedEvent { UnequippedEntity = equippableEntity });
                    }
                    else
                    {
                        if (MainHand != null)
                        {
                            EventLog.Add(new UnequippedEvent { UnequippedEntity = MainHand });
                        }

                        MainHand = equippableEntity;
                        EventLog.Add(new EquippedEvent { EquippedEntity = equippableEntity });
                    }

                    break;
                case EquipmentSlot.OffHand:
                    if (OffHand == equippableEntity)
                    {
                        OffHand = null;
                        EventLog.Add(new UnequippedEvent { UnequippedEntity = equippableEntity });
                    }
                    else
                    {
                        if (OffHand != null)
                        {
                            EventLog.Add(new UnequippedEvent { UnequippedEntity = OffHand });
                        }

                        OffHand = equippableEntity;
                        EventLog.Add(new EquippedEvent { EquippedEntity = equippableEntity });
                    }

                    break;
            }
        }
    }
}
