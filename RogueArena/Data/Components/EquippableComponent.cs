namespace RogueArena.Data.Components
{
    public class EquippableComponent : Component
    {
        public EquipmentSlot Slot { get; set; }
        public int PowerBonus { get; set; }
        public int DefenseBonus { get; set; }
        public int MaxHpBonus { get; set; }
    }
}
