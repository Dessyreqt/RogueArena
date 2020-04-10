namespace RogueArena
{
    using Events;
    using Microsoft.Xna.Framework;
    using RogueArena.Data;

    public static class DeathFunctions
    {
        public static void KillPlayer(Entity player, ProgramData data)
        {
            player.Character = '%';
            player.Color = Color.DarkRed;

            data.Events.Add(new MessageEvent("You died!", Color.Red));
        }

        public static void KillMonster(Entity monster, ProgramData data)
        {
            data.Events.Add(new MessageEvent($"{monster.Name} is dead!", Color.Orange));

            monster.Character = '%';
            monster.Color = Color.DarkRed;
            monster.Blocks = false;
            monster.FighterComponent = null;
            monster.AiComponent = null;
            monster.Name = $"remains of {monster.Name}";
            monster.RenderOrder = RenderOrder.Corpse;
        }
    }
}