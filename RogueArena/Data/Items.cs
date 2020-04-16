namespace RogueArena.Data
{
    using System;
    using Microsoft.Xna.Framework;
    using RogueArena.Data.Components;
    using RogueArena.Data.Components.ItemFunctions;
    using RogueArena.Messages;

    public class Items
    {
        public const string ConfusionScroll = nameof(ConfusionScroll);
        public const string Dagger = nameof(Dagger);
        public const string FireballScroll = nameof(FireballScroll);
        public const string HealingPotion = nameof(HealingPotion);
        public const string LightningScroll = nameof(LightningScroll);
        public const string Shield = nameof(Shield);
        public const string Sword = nameof(Sword);

        public static Entity Get(string itemName, int x, int y)
        {
            Entity entity;

            switch (itemName)
            {
                case ConfusionScroll:
                    entity = new Entity { X = x, Y = y, Character = '#', Color = Color.LightPink, Name = "Confusion Scroll", RenderOrder = RenderOrder.Item };
                    entity.Add(
                        new ItemComponent
                        {
                            ItemFunction = new CastConfuseFunction(),
                            Targeting = true,
                            TargetingMessage = new Message("Left-click an enemy to confuse it, or right-click to cancel.", Color.LightCyan)
                        });
                    return entity;
                case Dagger:
                    entity = new Entity { X = x, Y = y, Character = '-', Color = Color.SkyBlue, Name = "Dagger" };
                    entity.Add(new EquippableComponent { Slot = EquipmentSlot.MainHand, PowerBonus = 2 });
                    entity.Add(new ItemComponent());
                    return entity;
                case FireballScroll:
                    entity = new Entity { X = x, Y = y, Character = '#', Color = Color.Red, Name = "Fireball Scroll", RenderOrder = RenderOrder.Item };
                    entity.Add(
                        new ItemComponent
                        {
                            ItemFunction = new CastFireballFunction { Damage = 25, Radius = 3 },
                            Targeting = true,
                            TargetingMessage = new Message("Left-click a target tile for the fireball, or right-click to cancel.", Color.LightCyan)
                        });
                    return entity;
                case HealingPotion:
                    entity = new Entity { X = x, Y = y, Character = '!', Color = Color.DarkViolet, Name = "Healing Potion", RenderOrder = RenderOrder.Item };
                    entity.Add(new ItemComponent { ItemFunction = new HealFunction { Amount = 40 } });
                    return entity;
                case LightningScroll:
                    entity = new Entity { X = x, Y = y, Character = '#', Color = Color.Yellow, Name = "Lightning Scroll", RenderOrder = RenderOrder.Item };
                    entity.Add(new ItemComponent { ItemFunction = new CastLightningFunction { Damage = 40, MaximumRange = 5 } });
                    return entity;
                case Shield:
                    entity = new Entity { X = x, Y = y, Character = '[', Color = Color.DarkOrange, Name = "Shield" };
                    entity.Add(new EquippableComponent { Slot = EquipmentSlot.OffHand, DefenseBonus = 1 });
                    entity.Add(new ItemComponent());
                    return entity;
                case Sword:
                    entity = new Entity { X = x, Y = y, Character = '/', Color = Color.SkyBlue, Name = "Sword" };
                    entity.Add(new EquippableComponent { Slot = EquipmentSlot.MainHand, PowerBonus = 3 });
                    entity.Add(new ItemComponent());
                    return entity;
                default:
                    throw new ArgumentException($"{itemName} is not a valid monster type.", nameof(itemName));
            }
        }
    }
}
