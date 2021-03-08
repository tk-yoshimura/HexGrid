using System.Collections.Generic;

namespace HexGrid {

    /// <summary>Hexagon Grid</summary>
    public class HexaGrid : RectGrid {

        /// <summary>Grid Size</summary>
        public int Size { private set; get; }

        /// <summary>Make Instance</summary>
        public HexaGrid(int size) : base(size * 2 - 1, size * 2 - 1) {
            List<int> remove_indexes = new();

            if (size % 2 == 1) {
                for (int i = 0, remove_lines = size - 2; i < remove_lines; i++) {
                    int remove_cells = size / 2 - (i + 1) / 2;

                    for (int j = 0; j < remove_cells; j++) {
                        remove_indexes.Add(j + i * Width);
                        remove_indexes.Add(j + (Height - i - 1) * Width);
                    }
                }

                for (int i = 0, remove_lines = size - 1; i < remove_lines; i++) {
                    int remove_cells = size / 2 - i / 2;

                    for (int j = 0; j < remove_cells; j++) {
                        remove_indexes.Add(Width - j - 1 + i * Width);
                        remove_indexes.Add(Width - j - 1 + (Height - i - 1) * Width);
                    }
                }

                Remove(remove_indexes.ToArray());
            }
            else {
                for (int i = 0, remove_lines = size - 1; i < remove_lines; i++) {
                    int remove_cells = size / 2 - (i + 1) / 2;

                    for (int j = 0; j < remove_cells; j++) {
                        remove_indexes.Add(j + i * Width);
                        remove_indexes.Add(j + (Height - i - 1) * Width);
                    }
                }

                for (int i = 0, remove_lines = size - 2; i < remove_lines; i++) {
                    int remove_cells = size / 2 - (i + 2) / 2;

                    for (int j = 0; j < remove_cells; j++) {
                        remove_indexes.Add(Width - j - 1 + i * Width);
                        remove_indexes.Add(Width - j - 1 + (Height - i - 1) * Width);
                    }
                }

                Remove(remove_indexes.ToArray());

                foreach (Cell cell in Cells) {
                    cell.X -= 1;
                }
            }

            MapWidth = size * 4 - 3;
            Size = size;
        }
    }
}
