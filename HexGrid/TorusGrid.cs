using System;

namespace HexGrid {

    /// <summary>Torus Grid</summary>
    public class TorusGrid : RingGrid {

        /// <summary>Make Instance</summary>
        public TorusGrid(int width, int height)
            : base(width, height > 1 ? height : throw new ArgumentOutOfRangeException(nameof(height))) {

            for (int x = 0; x < width; x++) {
                Cell cell_up = Cells[x], cell_down = Cells[x + width * (height - 1)];

                cell_up.U = cell_down.Index;
                cell_down.D = cell_up.Index;

                if (x % 2 == 0) {
                    if (x > 0) {
                        cell_up.LU = cell_down.Index - 1;
                    }
                    else {
                        cell_up.LU = width * height - 1;
                    }

                    if (x < width - 1) {
                        cell_up.RU = cell_down.Index + 1;
                    }
                    else {
                        cell_up.RU = width - 1;
                    }
                }
                else { 
                    if (x > 0) {
                        cell_down.LD = cell_up.Index - 1;
                    }
                    else {
                        cell_down.LD = width - 1;
                    }

                    if (x < width - 1) {
                        cell_down.RD = cell_up.Index + 1;
                    }
                    else {
                        cell_down.RD = 0;
                    }
                }
            }
        }
    }
}
