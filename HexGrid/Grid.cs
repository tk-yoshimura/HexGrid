using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace HexGrid {

    /// <summary>Generic Grid</summary>
    [DebuggerDisplay("Grid size:{MapWidth}x{MapHeight} cells:{Count}")]
    public abstract class Grid {
        private readonly List<Cell> cell_list;

        /// <summary>Cell Counts</summary>
        public int Count => cell_list.Count;

        /// <summary>Map Width</summary>
        public int MapWidth { get; protected set; }

        /// <summary>Map Height</summary>
        public int MapHeight { get; protected set; }

        /// <summary>Make Instance</summary>
        internal Grid(int cells) {
            if (cells < 0) {
                throw new ArgumentException(nameof(cells));
            }

            this.cell_list = new List<Cell>((new Cell[cells]).Select((_, i) => new Cell(i)));
        }

        /// <summary>Indexer</summary>
        public Cell this[int index] {
            get {
                return cell_list[index];
            }
        }

        /// <summary>Readonly Cell List</summary>
        public IReadOnlyList<Cell> Cells => cell_list;

        /// <summary>Return Sparse Matrix</summary>
        public IEnumerable<(int index, IEnumerable<(Dir dir, int index)>)> Link {
            get {
                foreach (Cell cell in cell_list) {
                    yield return (cell.Index, cell.IndexList);
                }
            }
        }

        /// <summary>Return Cell Map</summary>
        public int[,] Map {
            get {
                int[,] cells = new int[MapWidth, MapHeight];

                for (int y = 0; y < MapHeight; y++) {
                    for (int x = 0; x < MapWidth; x++) {
                        cells[x, y] = Cell.None;
                    }
                }

                foreach (Cell cell in cell_list) {
                    cells[cell.X, cell.Y] = cell.Index;
                }

                return cells;
            }
        }

        /// <summary>Remove Cells</summary>
        protected void Remove(int[] cell_indexes) {
            if (cell_indexes is null) {
                throw new ArgumentNullException(nameof(cell_indexes));
            }

            if (cell_indexes.Any((index) => index < 0 || index >= Count)) {
                throw new IndexOutOfRangeException(nameof(cell_indexes));
            }

            if (cell_indexes.Distinct().Count() != cell_indexes.Length) {
                throw new ArgumentException($"{nameof(cell_indexes)} contains duplicated item.");
            }

            int[] remap = (new int[Count]).Select((_, i) => i).ToArray();

            for (int index = 0; index < Count; index++) {
                if (cell_indexes.Contains(index)) {
                    remap[index] = Cell.None;

                    for (int i = index + 1; i < Count; i++) {
                        remap[i]--;
                    }
                }
            }

            for (int index = Count - 1; index >= 0; index--) {
                if (cell_indexes.Contains(index)) {
                    cell_list.RemoveAt(index);
                }
            }

            for (int index = 0; index < Count; index++) {
                for (int src_index = 0; src_index < remap.Length; src_index++) {
                    int dst_index = remap[src_index];

                    cell_list[index].Remap(src_index, dst_index);
                }
            }
        }

        /// <summary>To String Map</summary>
        public override string ToString() {
            int[,] map = Map;

            int digits = $"{map.Cast<int>().Max()}".Length;

            string cell_null = new(' ', digits);

            StringBuilder strbuilder = new();

            for (int y = 0; y < MapHeight; y++) {
                for (int x = 0; x < MapWidth; x++) {
                    if (map[x, y] < 0) {
                        strbuilder.Append(cell_null);
                    }
                    else {
                        strbuilder.Append($"{map[x, y]}".PadLeft(digits));
                    }
                }

                strbuilder.Append('\n');
            }

            string str = strbuilder.ToString();

            return str;
        }
    }
}
