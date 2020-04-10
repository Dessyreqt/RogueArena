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

            switch (itemName)
            {
                case ConfusionScroll:
                    itemFunction = new CastConfuseFunction();
                    targetingMessage = new Message("Left-click an enemy to confuse it, or right-click to cancel.", Color.LightCyan);
                    itemComponent = new ItemComponent(itemFunction, true, targetingMessage);
                    return new Entity(x, y, '#', Color.LightPink, "Confusion Scroll", renderOrder: RenderOrder.Item, itemComponent: itemComponent);
                case Dagger:
                    equippableComponent = new EquippableComponent { Slot = EquipmentSlot.MainHand, PowerBonus = 2 };
                    return new Entity(x, y, '-', Color.SkyBlue, "Dagger", equippableComponent: equippableComponent);
                case FireballScroll:
                    itemFunction = new CastFireballFunction(25, 3);
                    targetingMessage = new Message("Left-click a target tile for the fireball, or right-click to cancel.", Color.LightCyan);
                    itemComponent = new ItemComponent(itemFunction, true, targetingMessage);
                    return new Entity(x, y, '#', Color.Red, "Fireball Scroll", renderOrder: RenderOrder.Item, itemComponent: itemComponent);
                case HealingPotion:
                    itemFunction = new HealFunction(40);
                    itemComponent = new ItemComponent(itemFunction);
                    return new Entity(x, y, '!', Color.DarkViolet, "Healing Potion", renderOrder: RenderOrder.Item, itemComponent: itemComponent);
                case LightningScroll:
                    itemFunction = new CastLightningFunction(40, 5);
                    itemComponent = new ItemComponent(itemFunction);
                    return new Entity(x, y, '#', Color.Yellow, "Lightning Scroll", renderOrder: RenderOrder.Item, itemComponent: itemComponent);
                case Shield:
                    equippableComponent = new EquippableComponent { Slot = EquipmentSlot.OffHand, DefenseBonus = 1 };
                    return new Entity(x, y, '[', Color.DarkOrange, "Shield", equippableComponent: equippableComponent);
                case Sword:
                    equippableComponent = new EquippableComponent { Slot = EquipmentSlot.MainHand, PowerBonus = 3 };
                    return new Entity(x, y, '/', Color.SkyBlue, "Sword", equippableComponent: equippableComponent);
                default:
                    throw new ArgumentException($"{itemName} is not a valid monster type.", nameof(itemName));
            }
        }
    }
}
