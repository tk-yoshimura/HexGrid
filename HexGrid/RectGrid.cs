using System;

namespace HexGrid {

    /// <summary>Rectangle Grid</summary>
    public class RectGrid : Grid{

        /// <summary>Grid Width</summary>
        public int Width { private set; get; }

        /// <summary>Grid Height</summary>
        public int Height { private set; get; }

        /// <summary>Make Instance</summary>
        public RectGrid(int width, int height) : base(checked(width * height)) {
            if (width < 1 || height < 1) {
                throw new ArgumentException($"{width}, {height}");
            }

            for (int y = 0, i = 0; y < height; y++) {
                for (int x = 0; x < width; x++, i++) {
                    Cell cell = this[i];

                    cell.X = x * 2 + y % 2;
                    cell.Y = y;

                    if (x > 0) {
                        cell.L = i - 1;
                    }
                    if (x < width - 1) {
                        cell.R = i + 1;
                    }

                    if (y % 2 == 0) {
                        if (y > 0) {
                            cell.RU = i - width;
                        }
                        if (y < height - 1) {
                            cell.RD = i + width;
                        }
                        if (x > 0) {
                            if (y > 0) {
                                cell.LU = i - width - 1;
                            }
                            if (y < height - 1) {
                                cell.LD = i + width - 1;
                            }
                        }
                    }
                    else {
                        if (x < width - 1) {
                            if (y > 0) {
                                cell.RU = i - width + 1;
                            }
                            if (y < height - 1) {
                                cell.RD = i + width + 1;
                            }
                        }
                        if (y > 0) {
                            cell.LU = i - width;
                        }
                        if (y < height - 1) {
                            cell.LD = i + width;
                        }
                    }
                }
            }

            this.Width = width;
            this.Height = height;
            this.MapWidth  = height > 1 ? width * 2 : width * 2 - 1;
            this.MapHeight = height;
        }
    }
}
