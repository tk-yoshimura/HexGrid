using System;

namespace HexGrid {

    /// <summary>Ring Grid (Horizontal Connected)</summary>
    public class RingGrid : RectGrid {

        /// <summary>Make Instance</summary>
        public RingGrid(int width, int height)
            : base(width > 1 ? width : throw new ArgumentOutOfRangeException(nameof(width)), height) {

            for (int y = 0; y < height; y++) {
                Cell cell_left = Cells[y * width], cell_right = Cells[width - 1 + y * width];

                cell_left.L = cell_right.Index;
                cell_right.R = cell_left.Index;

                if (y % 2 == 0) {
                    if (y > 0) {
                        cell_left.LU = cell_right.Index - width;
                    }
                    if (y < height - 1) {
                        cell_left.LD = cell_right.Index + width;
                    }
                }
                else {
                    if (y > 0) {
                        cell_right.RU = cell_left.Index - width;
                    }
                    if (y < height - 1) {
                        cell_right.RD = cell_left.Index + width;
                    }
                }
            }
        }
    }
}
