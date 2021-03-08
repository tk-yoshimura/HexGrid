using System;

namespace HexGrid {

    /// <summary>Torus Grid</summary>
    public class TorusGrid : RingGrid {

        /// <summary>Make Instance</summary>
        public TorusGrid(int width, int height)
            : base(width, height % 2 == 0 ? height : throw new ArgumentException($"{nameof(height)} must be even.")) {

            for (int x = 0, lx = width - 1, rx = 1; x < width; x++, lx = (lx + 1) % width, rx = (rx + 1) % width) {
                Cell cell_u = Cells[x], cell_d = Cells[x + width * (height - 1)];

                cell_u.RU = cell_d.Index;
                cell_d.LD = cell_u.Index;

                cell_u.LU = lx + width * (height - 1);
                cell_d.RD = rx;
            }
        }
    }
}
