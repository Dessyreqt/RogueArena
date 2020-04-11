﻿namespace RogueArena.Events
{
    using Microsoft.Xna.Framework;
    using RogueArena.Data;

    public class DeadEvent : Event
    {
        private readonly Entity _entity;

        public DeadEvent(Entity entity)
        {
            _entity = entity;
        }

        protected override void Handle(ProgramData data)
        {
            if (_entity == data.GameData.Player)
            {
                _entity.Character = '%';
                _entity.Color = Color.DarkRed;
                data.GameData.MessageLog.AddMessage("You died!", Color.Red);
                data.GameData.GameState = GameState.PlayerDead;
            }
            else
            {
                data.GameData.MessageLog.AddMessage($"{_entity.Name} is dead!", Color.Orange);

                _entity.Character = '%';
                _entity.Color = Color.DarkRed;
                _entity.Blocks = false;
                _entity.FighterComponent = null;
                _entity.AiComponent = null;
                _entity.Name = $"remains of {_entity.Name}";
                _entity.RenderOrder = RenderOrder.Corpse;
            }
        }
    }
}
