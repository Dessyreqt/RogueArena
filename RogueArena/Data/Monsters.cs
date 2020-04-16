namespace RogueArena.Data
{
    using System;
    using Microsoft.Xna.Framework;
    using RogueArena.Data.Components;
    using RogueArena.Data.Components.AI;

    public static class Monsters
    {
        public const string Orc = nameof(Orc);
        public const string Troll = nameof(Troll);

        public static Entity Get(string monsterName, int x, int y)
        {
            FighterComponent fighterComponent;
            AiComponent aiComponent;
            Entity entity;

            switch (monsterName)
            {
                case Orc:
                    fighterComponent = new FighterComponent { BaseMaxHp = 20, Hp = 20, BaseDefense = 0, BasePower = 4, Xp = 35 };
                    aiComponent = new BasicMonster();
                    entity = new Entity { X = x, Y = y, Character = 'o', Color = Color.LightGreen, Name = "Orc", Blocks = true, RenderOrder = RenderOrder.Actor };
                    entity.Add(fighterComponent);
                    entity.Add(aiComponent);
                    return entity;
                case Troll:
                    fighterComponent = new FighterComponent { BaseMaxHp = 30, Hp = 30, BaseDefense = 2, BasePower = 8, Xp = 100 };
                    aiComponent = new BasicMonster();
                    entity = new Entity { X = x, Y = y, Character = 'T', Color = Color.DarkGreen, Name = "Troll", Blocks = true, RenderOrder = RenderOrder.Actor };
                    entity.Add(fighterComponent);
                    entity.Add(aiComponent);
                    return entity;
                default:
                    throw new ArgumentException($"{monsterName} is not a valid monster type.", nameof(monsterName));
            }
        }
    }
}
