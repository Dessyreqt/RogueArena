namespace RogueArena.Map
{
    public class Tile
    {
        public bool Blocked { get; set; }
        public bool BlockSight { get; set; }

        public Tile(bool blocked, bool? blockSight = null)
        {
            Blocked = blocked;

            if (blockSight == null)
            {
                blockSight = blocked;
            }

            BlockSight = blockSight.Value;
        }
    }
}
