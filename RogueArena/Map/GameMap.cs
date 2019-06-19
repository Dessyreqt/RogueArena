namespace RogueArena.Map
{
    public class GameMap
    {
        public GameMap(int width, int height)
        {
            Width = width;
            Height = height;
            Tiles = InitializeTiles();
        }

        public int Width { get; private set; }
        public int Height { get; private set; }
        public Tile[,] Tiles { get; private set; }

        public Tile[,] InitializeTiles()
        {
            var tiles = new Tile[Width, Height];

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    tiles[x, y] = new Tile(false);
                }
            }

            tiles[30, 22].Blocked = true;
            tiles[30, 22].BlockSight = true;
            tiles[31, 22].Blocked = true;
            tiles[31, 22].BlockSight = true;
            tiles[32, 22].Blocked = true;
            tiles[32, 22].BlockSight = true;

            return tiles;
        }

        public bool IsBlocked(int x, int y)
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height)
            {
                return true;
            }

            if (Tiles[x, y].Blocked)
            {
                return true;
            }

            return false;
        }
    }
}