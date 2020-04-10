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
            FighterComponent fighterComponent;
            AiComponent aiComponent;

            switch (monsterName)
            {
                case Orc:
                    fighterComponent = new FighterComponent(20, 0, 4, 35);
                    aiComponent = new BasicMonster();
                    return new Entity(x, y, 'o', Color.LightGreen, "Orc", true, RenderOrder.Actor, fighterComponent, aiComponent);
                case Troll:
                    fighterComponent = new FighterComponent(30, 2, 8, 100);
                    aiComponent = new BasicMonster();
                    return new Entity(x, y, 'T', Color.DarkGreen, "Troll", true, RenderOrder.Actor, fighterComponent, aiComponent);
                default:
                    throw new ArgumentException($"{monsterName} is not a valid monster type.", nameof(monsterName));
            }
        }
    }
}
