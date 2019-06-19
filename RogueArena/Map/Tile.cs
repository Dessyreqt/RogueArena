namespace RogueArena.Map
{
    public class Tile
    {
        public Tile(bool blocked, bool? blockSight = null)
        {
            Blocked = blocked;

            if (blockSight == null)
            {
                blockSight = blocked;
            }

            BlockSight = blockSight.Value;
        }

        public bool Blocked { get; set; }
        public bool BlockSight { get; set; }
    }
}