namespace RogueArena.Data
{
    using System;
    using Microsoft.Xna.Framework;
    using RogueArena.Components;
    using RogueArena.Components.AI;

    public static class Monsters
    {
        public const string Orc = nameof(Orc);
        public const string Troll = nameof(Troll);

        public static Entity Get(string monsterName, int x, int y)
        {
            Fighter fighter = null;
            AI ai = null;

            switch (monsterName)
            {
                case Orc:
                    fighter = new Fighter(10, 0, 3);
                    ai = new BasicMonster();
                    return new Entity(x, y, 'o', Color.LightGreen, "Orc", true, RenderOrder.Actor, fighter, ai);
                case Troll:
                    fighter = new Fighter(16, 1, 4);
                    ai = new BasicMonster();
                    return new Entity(x, y, 'T', Color.DarkGreen, "Troll", true, RenderOrder.Actor, fighter, ai);
                default:
                    throw new ArgumentException($"{monsterName} is not a valid monster type.", nameof(monsterName));
            }
        }
    }
}
