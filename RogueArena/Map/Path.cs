namespace RogueArena.Map
{
    using System;
    using System.Collections.Generic;

    public class Cell
    {
        public int ParentX { get; set; }
        public int ParentY { get; set; }
        public double FValue { get; set; }
        public double GValue { get; set; }
        public double HValue { get; set; }
    }

    public class NodeScore
    {
        public NodeScore(double score, Point location)
        {
            Score = score;
            Location = location;
        }

        public double Score { get; set; }
        public Point Location { get; set; }
    }

    public class Path
    {
        public const int NorthWest = 0;
        public const int North = 1;
        public const int NorthEast = 2;
        public const int West = 3;
        public const int None = 4;
        public const int East = 5;
        public const int SouthWest = 6;
        public const int South = 7;
        public const int SouthEast = 8;

        private static readonly List<int> _directionX = new List<int> { -1, 0, 1, -1, 0, 1, -1, 0, 1 };
        private static readonly List<int> _directionY = new List<int> { -1, -1, -1, 0, 0, 0, 1, 1, 1 };

        public Path(GameMap map, double diagonalCost)
        {
            if (map == null)
            {
                throw new ArgumentException("Cannot create path on null map", nameof(map));
            }

            SetPathSize(map.Width, map.Height);
            Map = map;
            DiagonalCost = diagonalCost;
            OpenList = new List<NodeScore>();
        }

        public int OriginX { get; private set; }
        public int OriginY { get; private set; }
        public int DestinationX { get; private set; }
        public int DestinationY { get; private set; }
        public List<Point> Nodes { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public bool[,] ClosedList { get; private set; }
        public Cell[,] CellDetails { get; private set; }
        public List<NodeScore> OpenList { get; private set; }
        public double DiagonalCost { get; private set; }
        public GameMap Map { get; private set; }

        public bool Compute(int originX, int originY, int destX, int destY)
        {
            OriginX = originX;
            OriginY = originY;
            DestinationX = destX;
            DestinationY = destY;
            Nodes.Clear();
            OpenList.Clear();

            if (originX == destX && originY == destY)
            {
                return true;
            }

            var originOutsideMap = !IsValid(OriginX, OriginY);
            var destOutsideMap = !IsValid(DestinationX, DestinationY);
            if (originOutsideMap || destOutsideMap)
            {
                return false;
            }

            if (!Map.Tiles[OriginX, OriginY].Walkable || !Map.Tiles[DestinationX, DestinationY].Walkable)
            {
                return false;
            }

            InitializeGrid(ClosedList, false, Width, Height);
            InitializeCellDetails();

            OpenList.Add(new NodeScore(0, new Point(OriginX, OriginY)));

            bool foundDest = false;

            while (OpenList.Count != 0)
            {
                var nodeScore = OpenList[0];
                OpenList.RemoveAt(0);

                var x = nodeScore.Location.X;
                var y = nodeScore.Location.Y;
                ClosedList[x, y] = true;

                if (ProcessCell(x, y, NorthEast, DiagonalCost))
                {
                    break;
                }

                if (ProcessCell(x, y, North, 1))
                {
                    break;
                }

                if (ProcessCell(x, y, NorthWest, DiagonalCost))
                {
                    break;
                }

                if (ProcessCell(x, y, East, 1))
                {
                    break;
                }

                if (ProcessCell(x, y, West, 1))
                {
                    break;
                }

                if (ProcessCell(x, y, SouthEast, DiagonalCost))
                {
                    break;
                }

                if (ProcessCell(x, y, South, 1))
                {
                    break;
                }

                if (ProcessCell(x, y, SouthWest, DiagonalCost))
                {
                    break;
                }
            }

            return true;
        }

        private bool ProcessCell(int originX, int originY, int direction, double moveCost)
        {
            var delta = GetDelta(direction);

            int x = originX + delta.X;
            int y = originY + delta.Y;

            double fNew;
            double gNew;
            double hNew;

            if (!IsValid(x, y))
            {
                return false;
            }

            if (x == DestinationX && y == DestinationY)
            {
                CellDetails[x, y].ParentX = originX;
                CellDetails[x, y].ParentY = originY;
                SetNodes();

                return true;
            }

            if (ClosedList[x, y] == false && Map.Tiles[x, y].Walkable)
            {
                gNew = CellDetails[originX, originY].GValue + moveCost;
                hNew = CalculateHValue(x, y);
                fNew = gNew + hNew;

                if (CellDetails[x, y].FValue > fNew)
                {
                    OpenList.Add(new NodeScore(fNew, new Point(x, y)));

                    var curCell = CellDetails[x, y];
                    curCell.FValue = fNew;
                    curCell.GValue = gNew;
                    curCell.HValue = hNew;
                    curCell.ParentX = originX;
                    curCell.ParentY = originY;
                }
            }

            return false;
        }

        private double CalculateHValue(int x, int y)
        {
            return Math.Sqrt((x - DestinationX) * (x - DestinationX) + (y - DestinationY) * (y - DestinationY));
        }

        private void SetNodes()
        {
            var curX = DestinationX;
            var curY = DestinationY;

            while (!(CellDetails[curX, curY].ParentX == curX && CellDetails[curX, curY].ParentY == curY))
            {
                Nodes.Insert(0, new Point(curX, curY));

                var curCell = CellDetails[curX, curY];
                curX = curCell.ParentX;
                curY = curCell.ParentY;
            }

            Nodes.Insert(0, new Point(curX, curY));
        }

        private void InitializeCellDetails()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    CellDetails[x, y] = new Cell { FValue = double.MaxValue, GValue = double.MaxValue, HValue = double.MaxValue, ParentX = -1, ParentY = -1 };
                }
            }

            var initialCell = CellDetails[OriginX, OriginY];
            initialCell.FValue = 0;
            initialCell.GValue = 0;
            initialCell.HValue = 0;
            initialCell.ParentX = OriginX;
            initialCell.ParentY = OriginY;
        }

        private void SetPathSize(int width, int height)
        {
            Width = width;
            Height = height;
            ClosedList = new bool[width, height];
            CellDetails = new Cell[width, height];
            Nodes = new List<Point>();
        }

        private bool IsValid(int x, int y)
        {
            return 0 <= x && x < Width && 0 <= y && y < Height;
        }

        private void InitializeGrid<T>(T[,] grid, T value, int width, int height)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    grid[x, y] = value;
                }
            }
        }

        private Point GetDelta(int direction)
        {
            return new Point(_directionX[direction], _directionY[direction]);
        }

        private int InvertDirection(int direction)
        {
            return 8 - direction;
        }
    }
}