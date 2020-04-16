namespace RogueArena.Events
{
    using RogueArena.Data;
    using RogueArena.Data.Components;

    public class ToggleEquipEvent : Event
    {
        private readonly Entity _equippableEntity;

        public ToggleEquipEvent(Entity equippableEntity)
        {
            _equippableEntity = equippableEntity;
        }

        protected override void Handle(ProgramData data)
        {
            data.GameData.Player.Get<EquipmentComponent>().ToggleEquip(_equippableEntity, data);
            data.GameData.GameState = GameState.EnemyTurn;
        }
    }
}
