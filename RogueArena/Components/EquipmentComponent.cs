namespace RogueArena.Components
{
    using RogueArena.Data;
    using RogueArena.Events;

    public class EquipmentComponent : Component
    {
        public int MaxHpBonus => (MainHand?.Get<EquippableComponent>()?.MaxHpBonus ?? 0) + (OffHand?.Get<EquippableComponent>()?.MaxHpBonus ?? 0);
        public int PowerBonus => (MainHand?.Get<EquippableComponent>()?.PowerBonus ?? 0) + (OffHand?.Get<EquippableComponent>()?.PowerBonus ?? 0);
        public int DefenseBonus => (MainHand?.Get<EquippableComponent>()?.DefenseBonus ?? 0) + (OffHand?.Get<EquippableComponent>()?.DefenseBonus ?? 0);
        public Entity MainHand { get; set; }
        public Entity OffHand { get; set; }

        public void ToggleEquip(Entity equippableEntity, ProgramData data)
        {
            if (equippableEntity.Get<EquippableComponent>() == null)
            {
                return;
            }

            var slot = equippableEntity.Get<EquippableComponent>().Slot;
            switch (slot)
            {
                case EquipmentSlot.MainHand:
                    if (MainHand == equippableEntity)
                    {
                        MainHand = null;
                        data.Events.Add(new UnequippedEvent(equippableEntity));
                    }
                    else
                    {
                        if (MainHand != null)
                        {
                            data.Events.Add(new UnequippedEvent(MainHand));
                        }

                        MainHand = equippableEntity;
                        data.Events.Add(new EquippedEvent(equippableEntity));
                    }

                    break;
                case EquipmentSlot.OffHand:
                    if (OffHand == equippableEntity)
                    {
                        OffHand = null;
                        data.Events.Add(new UnequippedEvent(equippableEntity));
                    }
                    else
                    {
                        if (OffHand != null)
                        {
                            data.Events.Add(new UnequippedEvent(OffHand));
                        }

                        OffHand = equippableEntity;
                        data.Events.Add(new EquippedEvent(equippableEntity));
                    }

                    break;
            }
        }
    }
}
