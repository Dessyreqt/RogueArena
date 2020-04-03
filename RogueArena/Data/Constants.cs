namespace RogueArena.Data
{
    using RogueArena.Map;

    public static class Constants
    {
        public const string WindowTitle = "RogueArena";
        public const int ScreenWidth = 80;
        public const int ScreenHeight = 50;

        public const int BarWidth = 20;
        public const int PanelHeight = 7;
        public const int PanelY = ScreenHeight - PanelHeight;

        public const int MessageX = BarWidth + 2;
        public const int MessageWidth = ScreenWidth - BarWidth - 2;
        public const int MessageHeight = PanelHeight - 1;

        public const int MapWidth = 80;
        public const int MapHeight = 43;

        public const int MinRoomSize = 6;
        public const int MaxRoomSize = 10;
        public const int MaxRooms = 30;

        public const int FovAlgorithm = DungeonMap.FovBasic;
        public const bool FovLightWalls = true;
        public const int FovRadius = 10;

        public const int MaxMonstersPerRoom = 3;
        public const int MaxItemsPerRoom = 2;
    }
}
