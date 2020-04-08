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
            FighterComponent fighterComponent = null;
            AiComponent aiComponent = null;

            switch (monsterName)
            {
                case Orc:
                    fighterComponent = new FighterComponent(10, 0, 3, 35);
                    aiComponent = new BasicMonster();
                    return new Entity(x, y, 'o', Color.LightGreen, "Orc", true, RenderOrder.Actor, fighterComponent, aiComponent);
                case Troll:
                    fighterComponent = new FighterComponent(16, 1, 4, 100);
                    aiComponent = new BasicMonster();
                    return new Entity(x, y, 'T', Color.DarkGreen, "Troll", true, RenderOrder.Actor, fighterComponent, aiComponent);
                default:
                    throw new ArgumentException($"{monsterName} is not a valid monster type.", nameof(monsterName));
            }
        }
    }
}
