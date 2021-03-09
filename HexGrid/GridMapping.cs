using System;

namespace HexGrid {
    /// <summary>Grid Mapping</summary>
    public class GridMapping {
        private readonly int[,] map;
        private readonly bool is_even;

        /// <summary>Width</summary>
        public double Width { get; private set; }
        
        /// <summary>Height</summary>
        public double Height { get; private set; }

        /// <summary>MakeInstance</summary>
        public GridMapping(Grid grid) {
            if (grid.Count < 1) {
                throw new ArgumentException();
            }

            this.map = grid.Map;
            this.is_even = (grid[0].X + grid[0].Y) % 2 == 0;
            this.Width = grid.MapWidth * 0.5;
            this.Height = grid.MapHeight;
        }

        /// <summary>Mapped Cell Index</summary>
        public int this[double u, double v]{
            get{
                if (!InRange(u, v)) {
                    return Cell.None;
                }

                int x = (int)Math.Floor(u * 2);
                int y = (int)Math.Floor(v);

                x = ((y % 2 == 0) ^ is_even) ? ((x - 1) | 1) : (x & ~1);

                if (x < 0 || x >= map.GetLength(0) || y >= map.GetLength(1)) {
                    return Cell.None;
                }

                return map[x, y];
            }
        }

        /// <summary>Judgement in range</summary>
        public bool InRange(double u, double v) {
            return u >= 0 && u <= Width && v >= 0 && v <= Height;
        }
    }
}
