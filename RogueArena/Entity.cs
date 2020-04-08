namespace RogueArena
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Components;
    using Map;
    using Microsoft.Xna.Framework;
    using Util;

    [Serializable]
    public class Entity
    {
        public Entity()
        {
            // here for deserialization purposes
        }

        public Entity(
            int x,
            int y,
            char character,
            Color color,
            string name,
            bool blocks = false,
            RenderOrder renderOrder = RenderOrder.Corpse,
            FighterComponent fighterComponent = null,
            AiComponent aiComponent = null,
            ItemComponent itemComponent = null,
            InventoryComponent inventoryComponent = null,
            StairsComponent stairsComponent = null,
            LevelComponent levelComponent = null)
        {
            X = x;
            Y = y;
            Character = character;
            Color = color;
            Name = name;
            Blocks = blocks;
            RenderOrder = renderOrder;
            FighterComponent = fighterComponent;
            AiComponent = aiComponent;
            ItemComponent = itemComponent;
            InventoryComponent = inventoryComponent;
            StairsComponent = stairsComponent;
            LevelComponent = levelComponent;

            if (FighterComponent != null)
            {
                FighterComponent.Owner = this;
            }

            if (AiComponent != null)
            {
                AiComponent.Owner = this;
            }

            if (ItemComponent != null)
            {
                ItemComponent.Owner = this;
            }

            if (InventoryComponent != null)
            {
                InventoryComponent.Owner = this;
            }

            if (StairsComponent != null)
            {
                StairsComponent.Owner = this;
            }

            if (LevelComponent != null)
            {
                LevelComponent.Owner = this;
            }
        }

        public int X { get; set; }
        public int Y { get; set; }
        public char Character { get; set; }
        public Color Color { get; set; }
        public string Name { get; set; }
        public bool Blocks { get; set; }
        public RenderOrder RenderOrder { get; set; }
        public FighterComponent FighterComponent { get; set; }
        public AiComponent AiComponent { get; set; }
        public ItemComponent ItemComponent { get; set; }
        public InventoryComponent InventoryComponent { get; set; }
        public StairsComponent StairsComponent { get; set; }
        public LevelComponent LevelComponent { get; set; }

        public static Entity GetBlockingEntityAtLocation(List<Entity> entities, int destX, int destY)
        {
            return entities.FirstOrDefault(entity => entity.Blocks && entity.X == destX && entity.Y == destY);
        }

        public void Move(int xDelta, int yDelta)
        {
            X += xDelta;
            Y += yDelta;
        }

        public void MoveTowards(int targetX, int targetY, DungeonMap dungeonMap, List<Entity> entities)
        {
            var dx = targetX - X;
            var dy = targetY - Y;
            var distance = Math.Sqrt(dx * dx + dy * dy);

            dx = (int)Math.Round(dx / distance);
            dy = (int)Math.Round(dy / distance);

            if (!(dungeonMap.IsBlocked(X + dx, Y + dy) || GetBlockingEntityAtLocation(entities, X + dx, Y + dy) != null))
            {
                Move(dx, dy);
            }
        }

        public void MoveAstar(Entity target, DungeonMap dungeonMap, List<Entity> entities)
        {
            var newMap = dungeonMap.DeepClone();

            foreach (var entity in entities)
            {
                if (entity == this || entity == target || !entity.Blocks)
                {
                    continue;
                }

                newMap.Tiles[entity.X, entity.Y].Walkable = false;
            }

            var path = new Path(newMap, 1.41);

            var pathExists = path.Compute(X, Y, target.X, target.Y);

            if (pathExists && path.Nodes.Count > 0 && path.Nodes.Count < 25)
            {
                var newLocation = path.Nodes[1];

                X = newLocation.X;
                Y = newLocation.Y;
            }
            else
            {
                MoveTowards(target.X, target.Y, dungeonMap, entities);
            }
        }

        public double DistanceTo(int x, int y)
        {
            var dx = x - X;
            var dy = y - Y;

            return Math.Sqrt(dx * dx + dy * dy);
        }

        public double DistanceTo(Entity other)
        {
            var dx = other.X - X;
            var dy = other.Y - Y;

            return Math.Sqrt(dx * dx + dy * dy);
        }
    }
}