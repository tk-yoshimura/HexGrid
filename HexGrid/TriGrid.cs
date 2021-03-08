using System;
using System.Collections.Generic;

namespace HexGrid {

    /// <summary>Triangle Grid</summary>
    public class TriGrid : RectGrid{

        /// <summary>Grid Size</summary>
        public int Size { private set; get; }

        /// <summary>Make Instance</summary>
        public TriGrid(int size) : base(size * 2 + 1, (size * 3 - 1) / 2) {
            List<int> remove_indexes = new();

            for (int i = 0; i < Height; i++) {
                int remove_cells = ((Height - i) * 2 + 3) / 3;

                for (int j = 0; j < remove_cells; j++) {
                    remove_indexes.Add(i * Width + j);
                    remove_indexes.Add((i + 1) * Width - (j + 1));
                }
            }

            Remove(remove_indexes.ToArray());

            foreach (Cell cell in Cells) {
                cell.X -= 1;
            }

            if (size % 2 == 1) {
                foreach (Cell cell in Cells) {
                    cell.Y -= 1;
                }
            }

            MapWidth = size * 2 - 1;
            MapHeight = size * 3 - 2;
            Size = size;
        }
    }
}
