namespace RogueArena.Events
{
    public class ToggleEquipEvent : Event
    {
        public ToggleEquipEvent(Entity equippableEntity)
        {
            EquippableEntity = equippableEntity;
        }

        public Entity EquippableEntity { get; set; }
    }
}
