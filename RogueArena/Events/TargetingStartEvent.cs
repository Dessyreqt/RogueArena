﻿namespace RogueArena.Events
{
    using RogueArena.Data;
    using RogueArena.Data.Components;

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
            data.GameData.MessageLog.AddMessage(data.TargetingItem.Get<ItemComponent>().TargetingMessage);
        }
    }
}
