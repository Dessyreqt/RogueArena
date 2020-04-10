namespace RogueArena.Events
{
    using RogueArena.Data;

    public class TargetingStartEvent : Event
    {
        public TargetingStartEvent(Entity itemEntity)
        {
            ItemEntity = itemEntity;
        }

        public Entity ItemEntity { get; set; }
        protected override void Handle(ProgramData data)
        {
            data.PreviousGameState = GameState.PlayersTurn;
            data.GameData.GameState = GameState.Targeting;
            data.TargetingItem = ItemEntity;
            data.GameData.MessageLog.AddMessage(data.TargetingItem.ItemComponent.TargetingMessage);
        }
    }
}
