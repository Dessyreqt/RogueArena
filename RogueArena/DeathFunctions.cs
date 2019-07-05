namespace RogueArena
{
    using Events;
    using Microsoft.Xna.Framework;

    public static class DeathFunctions
    {
        public static void KillPlayer(Entity player)
        {
            player.Character = '%';
            player.Color = Color.DarkRed;

            EventLog.Add(new MessageEvent("You died!"));
        }

        public static void KillMonster(Entity monster)
        {
            EventLog.Add(new MessageEvent($"{monster.Name} is dead!"));

            monster.Character = '%';
            monster.Color = Color.DarkRed;
            monster.Blocks = false;
            monster.Fighter = null;
            monster.AI = null;
            monster.Name = $"remains of {monster.Name}";
            monster.RenderOrder = RenderOrder.Corpse;
        }
    }
}