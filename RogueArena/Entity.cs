namespace RogueArena
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Components;
    using Map;
    using Microsoft.Xna.Framework;
    using RogueArena.Components.AI;
    using Util;

    public class Entity
    {
        public Entity(
            int x,
            int y,
            char character,
            Color color,
            string name,
            bool blocks = false,
            int renderOrder = RogueArena.RenderOrder.Corpse,
            Fighter fighter = null,
            AI ai = null,
            Item item = null,
            Inventory inventory = null)
        {
            X = x;
            Y = y;
            Character = character;
            Color = color;
            Name = name;
            Fighter = fighter;
            AI = ai;
            Item = item;
            Inventory = inventory;
            Blocks = blocks;
            RenderOrder = renderOrder;

            if (Fighter != null)
            {
                Fighter.Owner = this;
            }

            if (AI != null)
            {
                AI.Owner = this;
            }

            if (Item != null)
            {
                Item.Owner = this;
            }

            if (Inventory != null)
            {
                Inventory.Owner = this;
            }
        }

        public int X { get; set; }
        public int Y { get; set; }
        public char Character { get; set; }
        public Color Color { get; set; }
        public string Name { get; set; }
        public bool Blocks { get; set; }
        public int RenderOrder { get; set; }
        public Fighter Fighter { get; set; }
        public AI AI { get; set; }
        public Item Item { get; set; }
        public Inventory Inventory { get; set; }

        public static Entity GetBlockingEntityAtLocation(List<Entity> entities, int destX, int destY)
        {
            return entities.FirstOrDefault(entity => entity.Blocks && entity.X == destX && entity.Y == destY);
        }

        public void Move(int xDelta, int yDelta)
        {
            X += xDelta;
            Y += yDelta;
        }

        public void MoveTowards(int targetX, int targetY, GameMap gameMap, List<Entity> entities)
        {
            var dx = targetX - X;
            var dy = targetY - Y;
            var distance = Math.Sqrt(dx * dx + dy * dy);

            dx = (int)Math.Round(dx / distance);
            dy = (int)Math.Round(dy / distance);

            if (!(gameMap.IsBlocked(X + dx, Y + dy) || GetBlockingEntityAtLocation(entities, X + dx, Y + dy) != null))
            {
                Move(dx, dy);
            }
        }

        public void MoveAstar(Entity target, GameMap gameMap, List<Entity> entities)
        {
            var newMap = gameMap.DeepClone();

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
                MoveTowards(target.X, target.Y, gameMap, entities);
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