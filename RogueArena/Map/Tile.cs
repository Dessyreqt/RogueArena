namespace RogueArena.Map
{
    using System;

    [Serializable]
    public class Tile
    {
        public Tile(bool walkable, bool? transparent = null)
        {
            Walkable = walkable;

            if (transparent == null)
            {
                transparent = walkable;
            }

            Transparent = transparent.Value;
            Explored = false;
        }

        public bool Walkable { get; set; }
        public bool Transparent { get; set; }
        public bool InView { get; set; }
        public bool Explored { get; set; }
    }
}