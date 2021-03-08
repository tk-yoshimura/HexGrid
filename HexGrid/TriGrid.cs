using System.Collections.Generic;

namespace HexGrid {

    /// <summary>Triangle Grid</summary>
    public class TriGrid : RectGrid{

        /// <summary>Grid Size</summary>
        public int Size { private set; get; }

        /// <summary>Make Instance</summary>
        public TriGrid(int size) : base(size, size) {
            List<int> remove_indexes = new();

            if (size % 2 == 1) {
                for (int i = 0, remove_lines = size - 2; i < remove_lines; i++) {
                    int remove_cells = size / 2 - (i + 1) / 2;

                    for (int j = 0; j < remove_cells; j++) {
                        remove_indexes.Add(j + i * Width);
                    }
                }

                for (int i = 0, remove_lines = size - 1; i < remove_lines; i++) {
                    int remove_cells = size / 2 - i / 2;

                    for (int j = 0; j < remove_cells; j++) {
                        remove_indexes.Add(Width - j - 1 + i * Width);
                    }
                }

                Remove(remove_indexes.ToArray());
            }
            else {
                for (int i = 0, remove_lines = size - 1; i < remove_lines; i++) {
                    int remove_cells = size / 2 - (i + 1) / 2;

                    for (int j = 0; j < remove_cells; j++) {
                        remove_indexes.Add(j + i * Width);
                    }
                }

                for (int i = 0, remove_lines = size - 2; i < remove_lines; i++) {
                    int remove_cells = size / 2 - (i + 2) / 2;

                    for (int j = 0; j < remove_cells; j++) {
                        remove_indexes.Add(Width - j - 1 + i * Width);
                    }
                }
    
                Remove(remove_indexes.ToArray());

                foreach (Cell cell in Cells) {
                    cell.X -= 1;
                }
            }

            MapWidth = size * 2 - 1;
            Size = size;
        }
    }
}
