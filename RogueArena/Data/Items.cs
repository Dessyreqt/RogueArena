namespace RogueArena.Data
{
    using System;
    using Microsoft.Xna.Framework;
    using RogueArena.Components;
    using RogueArena.Components.ItemFunctions;
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
            ItemFunction itemFunction;
            Message targetingMessage;
            ItemComponent itemComponent;
            EquippableComponent equippableComponent;
            Entity entity;

            switch (itemName)
            {
                case ConfusionScroll:
                    itemFunction = new CastConfuseFunction();
                    targetingMessage = new Message("Left-click an enemy to confuse it, or right-click to cancel.", Color.LightCyan);
                    itemComponent = new ItemComponent(itemFunction, true, targetingMessage);
                    entity = new Entity { X = x, Y = y, Character = '#', Color = Color.LightPink, Name = "Confusion Scroll", RenderOrder = RenderOrder.Item };
                    entity.Add(itemComponent);
                    return entity;
                case Dagger:
                    equippableComponent = new EquippableComponent { Slot = EquipmentSlot.MainHand, PowerBonus = 2 };
                    entity = new Entity { X = x, Y = y, Character = '-', Color = Color.SkyBlue, Name = "Dagger" };
                    entity.Add(equippableComponent);
                    entity.Add(new ItemComponent());
                    return entity;
                case FireballScroll:
                    itemFunction = new CastFireballFunction(25, 3);
                    targetingMessage = new Message("Left-click a target tile for the fireball, or right-click to cancel.", Color.LightCyan);
                    itemComponent = new ItemComponent(itemFunction, true, targetingMessage);
                    entity = new Entity { X = x, Y = y, Character = '#', Color = Color.Red, Name = "Fireball Scroll", RenderOrder = RenderOrder.Item };
                    entity.Add(itemComponent);
                    return entity;
                case HealingPotion:
                    itemFunction = new HealFunction(40);
                    itemComponent = new ItemComponent(itemFunction);
                    entity = new Entity { X = x, Y = y, Character = '!', Color = Color.DarkViolet, Name = "Healing Potion", RenderOrder = RenderOrder.Item };
                    entity.Add(itemComponent);
                    return entity;
                case LightningScroll:
                    itemFunction = new CastLightningFunction(40, 5);
                    itemComponent = new ItemComponent(itemFunction);
                    entity = new Entity { X = x, Y = y, Character = '#', Color = Color.Yellow, Name = "Lightning Scroll", RenderOrder = RenderOrder.Item };
                    entity.Add(itemComponent);
                    return entity;
                case Shield:
                    entity = new Entity { X = x, Y = y, Character = '[', Color = Color.DarkOrange, Name = "Shield" };
                    entity.Add(new EquippableComponent { Slot = EquipmentSlot.OffHand, DefenseBonus = 1 });
                    entity.Add(new ItemComponent());
                    return entity;
                case Sword:
                    equippableComponent = new EquippableComponent { Slot = EquipmentSlot.MainHand, PowerBonus = 3 };
                    entity = new Entity { X = x, Y = y, Character = '/', Color = Color.SkyBlue, Name = "Sword" };
                    entity.Add(equippableComponent);
                    entity.Add(new ItemComponent());
                    return entity;
                default:
                    throw new ArgumentException($"{itemName} is not a valid monster type.", nameof(itemName));
            }
        }
    }
}
