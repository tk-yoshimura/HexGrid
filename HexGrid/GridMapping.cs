using System;

namespace HexGrid {
    /// <summary>Grid Mapping</summary>
    public class GridMapping {
        private readonly int[,] map;
        private readonly bool is_even;

        /// <summary>Width</summary>
        public int Width { get; private set; }
        
        /// <summary>Height</summary>
        public int Height { get; private set; }

        /// <summary>MakeInstance</summary>
        public GridMapping(Grid grid) {
            if (grid.Count < 1) {
                throw new ArgumentException();
            }

            this.map = grid.Map;
            this.is_even = (grid[0].X + grid[0].Y) % 2 == 0;
            this.Width = grid.MapWidth;
            this.Height = grid.MapHeight;
        }

        /// <summary>Mapped Cell Index</summary>
        public int this[int x, int y]{
            get{
                x = ((y % 2 == 0) ^ is_even) ? ((x - 1) | 1) : (x & ~1);

                if (!InRange(x, y)) {
                    return Cell.None;
                }

                return map[x, y];
            }
        }

        /// <summary>Judgement in range</summary>
        private bool InRange(int x, int y) {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }
    }
}
