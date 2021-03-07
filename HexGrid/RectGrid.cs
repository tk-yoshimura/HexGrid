using System;

namespace HexGrid {

    /// <summary>Rectangle Grid</summary>
    public class RectGrid : Grid{

        /// <summary>Grid Width</summary>
        public int Width { private set; get; }

        /// <summary>Grid Height</summary>
        public int Height { private set; get; }

        /// <summary>Indexer</summary>
        public (Cell cell, int px, int py) this[int x, int y]
            => (x < Width && y < Height)
             ? (this[checked(x + y * Width)], x, y * 2 + x % 2)
             : throw new ArgumentOutOfRangeException($"{x}, {y}");

        /// <summary>Make Instance</summary>
        public RectGrid(int width, int height) : base(checked(width * height)) {
            if (width < 1 || height < 1) {
                throw new ArgumentException($"{width}, {height}");
            }

            for (int y = 0, i = 0; y < height; y++) {
                for (int x = 0; x < width; x++, i++) {
                    Cell cell = this[i];

                    if (y > 0) {
                        cell.U = i - width;
                    }
                    if (y < height - 1) {
                        cell.D = i + width;
                    }

                    if (x % 2 == 0) {
                        if (y > 0) {
                            if (x > 0) {
                                cell.LU = cell.U - 1;
                            }
                            if (x < width - 1) {
                                cell.RU = cell.U + 1;
                            }
                        }
                        if (x > 0) {
                            cell.LD = i - 1;
                        }
                        if (x < width - 1) {
                            cell.RD = i + 1;
                        }
                    }
                    else {
                        if (x > 0) {
                            cell.LU = i - 1;
                        }
                        if (x < width - 1) {
                            cell.RU = i + 1;
                        }
                        if (y < height - 1) { 
                            if (x > 0) {
                                cell.LD = cell.D - 1;
                            }
                            if (x < width - 1) {
                                cell.RD = cell.D + 1;
                            }
                        }
                    }
                }
            }

            this.Width = width;
            this.Height = height;
        }
    }
}
