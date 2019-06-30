namespace RogueArena
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Components;
    using Map;
    using Microsoft.Xna.Framework;
    using SharpDX.MediaFoundation;

    public class Entity
    {
        public Entity(int x, int y, char character, Color color, string name, bool blocks = false, Fighter fighter = null, AI ai = null)
        {
            X = x;
            Y = y;
            Character = character;
            Color = color;
            Name = name;
            Fighter = fighter;
            AI = ai;
            Blocks = blocks;

            if (Fighter != null)
            {
                Fighter.Owner = this;
            }

            if (AI != null)
            {
                AI.Owner = this;
            }
        }

        public int X { get; set; }
        public int Y { get; set; }
        public char Character { get; private set; }
        public Color Color { get; private set; }
        public string Name { get; private set; }
        public bool Blocks { get; private set; }
        public Fighter Fighter { get; private set; }
        public AI AI { get; private set; }

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

        public double DistanceTo(Entity other)
        {
            var dx = other.X - X;
            var dy = other.Y - Y;

            return Math.Sqrt(dx * dx + dy * dy);
        }
    }
}