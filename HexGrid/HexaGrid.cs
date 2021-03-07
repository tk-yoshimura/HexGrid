using System;
using System.Collections.Generic;

namespace HexGrid {

    /// <summary>Hexagon Grid</summary>
    public class HexaGrid : RectGrid{

        /// <summary>Grid Size</summary>
        public int Size { private set; get; }

        /// <summary>Make Instance</summary>
        public HexaGrid(int size) : base(size * 2 - 1, size * 2 - 1) {
            List<int> remove_indexes = new();

            for (int i = 0, remove_lines = size / 2; i < remove_lines; i++) {
                int remove_cells = (size | 1) - i * 2 - 2;

                for (int j = 0; j < remove_cells; j++) {
                    remove_indexes.Add(i * Width + j);
                    remove_indexes.Add((i + 1) * Width - (j + 1));
                }
            }

            for (int i = 0, remove_lines = (size - 1) / 2; i < remove_lines; i++) {
                int remove_cells = ((size - 1) & ~1) - i * 2;

                for (int j = 0; j < remove_cells; j++) {
                    remove_indexes.Add((Height - i - 1) * Width + j);
                    remove_indexes.Add((Height - i) * Width - (j + 1));
                }
            }

            Remove(remove_indexes.ToArray());

            if (size % 2 == 0) {
                foreach (Cell cell in Cells) {
                    cell.Y -= 1;
                }
            }

            MapHeight = size * 4 - 3;
            Size = size;
        }
    }
}
