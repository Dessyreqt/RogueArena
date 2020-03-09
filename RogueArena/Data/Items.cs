﻿namespace RogueArena.Data
{
    using System;
    using Microsoft.Xna.Framework;
    using RogueArena.Components;
    using RogueArena.Components.ItemFunctions;
    using RogueArena.Events;

    public class Items
    {
        public const string ConfusionScroll = nameof(ConfusionScroll);
        public const string FireballScroll = nameof(FireballScroll);
        public const string HealingPotion = nameof(HealingPotion);
        public const string LightningScroll = nameof(LightningScroll);

        public static Entity Get(string itemName, int x, int y)
        {
            ItemFunction itemFunction = null;
            Message targetingMessage = null;
            Item item = null;

            switch (itemName)
            {
                case ConfusionScroll:
                    itemFunction = new CastConfuseFunction();
                    targetingMessage = new Message("Left-click an enemy to confuse it, or right-click to cancel.", Color.LightCyan);
                    item = new Item(itemFunction, true, targetingMessage);
                    return new Entity(x, y, '#', Color.LightPink, "Confusion Scroll", renderOrder: RenderOrder.Item, item: item);
                case FireballScroll:
                    itemFunction = new CastFireballFunction(12, 3);
                    targetingMessage = new Message("Left-click a target tile for the fireball, or right-click to cancel.", Color.LightCyan);
                    item = new Item(itemFunction, true, targetingMessage);
                    return new Entity(x, y, '#', Color.Red, "Fireball Scroll", renderOrder: RenderOrder.Item, item: item);
                case HealingPotion:
                    itemFunction = new HealFunction(4);
                    item = new Item(itemFunction);
                    return new Entity(x, y, '!', Color.DarkViolet, "Healing Potion", renderOrder: RenderOrder.Item, item: item);
                case LightningScroll:
                    itemFunction = new CastLightningFunction(20, 5);
                    item = new Item(itemFunction);
                    return new Entity(x, y, '#', Color.Yellow, "Lightning Scroll", renderOrder: RenderOrder.Item, item: item);
                default:
                    throw new ArgumentException($"{itemName} is not a valid monster type.", nameof(itemName));
            }
        }
    }
}
