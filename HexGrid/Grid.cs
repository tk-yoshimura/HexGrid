using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace HexGrid {

    /// <summary>Generic Grid</summary>
    [DebuggerDisplay("Grid size:{MapWidth}x{MapHeight} cells:{Count}")]
    public class Grid {
        private readonly List<Cell> cell_list;

        /// <summary>Cell Counts</summary>
        public int Count => cell_list.Count;

        /// <summary>Map Width</summary>
        public int MapWidth { get; protected set; }

        /// <summary>Map Height</summary>
        public int MapHeight { get; protected set; }

        /// <summary>Indexer</summary>
        public Cell this[int index] {
            get { 
                return cell_list [index];
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

        /// <summary>Make Instance</summary>
        internal Grid(int cells) {
            if (cells < 0) {
                throw new ArgumentException(nameof(cells));
            }

            this.cell_list = new List<Cell>((new Cell[cells]).Select((_, i) => new Cell(i)));
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

        /// <summary>Validation</summary>
        public bool IsValid {
            get {
                if (!IsConnected) {
                    return false;
                }

                for (int index = 0; index < Count; index++) {
                    Cell cell = cell_list[index];

                    if (!cell.IsValid(Count)) {
                        return false;
                    }
                }

                if (!IsValidConnects) {
                    return false;
                }

                int[] ref_counts = new int[Count];

                foreach (Cell cell in cell_list) {
                    foreach ((_, int index) in cell.IndexList) {
                        ref_counts[index]++;
                    }
                }

                if (ref_counts.Any(count => count > 6)) {
                    return false;
                }

                if (MapWidth != cell_list.Select((cell) => cell.X).Max() + 1) {
                    return false;
                }

                if (MapHeight != cell_list.Select((cell) => cell.Y).Max() + 1) {
                    return false;
                }

                return true;
            }
        }

        /// <summary>Is Valid Connects</summary>
        public bool IsValidConnects {
            get {
                for (int index = 0; index < Count; index++) {
                    Cell cell = cell_list[index];

                    if (cell.U != Cell.None && cell_list[cell.U].D != index) {
                        return false;
                    }
                    if (cell.LU != Cell.None && cell_list[cell.LU].RD != index) {
                        return false;
                    }
                    if (cell.LD != Cell.None && cell_list[cell.LD].RU != index) {
                        return false;
                    }
                    if (cell.RU != Cell.None && cell_list[cell.RU].LD != index) {
                        return false;
                    }
                    if (cell.RD != Cell.None && cell_list[cell.RD].LU != index) {
                        return false;
                    }
                    if (cell.D != Cell.None && cell_list[cell.D].U != index) {
                        return false;
                    }

                    if (cell.U != Cell.None && cell.LU != Cell.None && cell_list[cell.U].LD != cell.LU) {
                        return false;
                    }

                    if (cell.LU != Cell.None && cell.LD != Cell.None && cell_list[cell.LU].D != cell.LD) {
                        return false;
                    }

                    if (cell.LD != Cell.None && cell.D != Cell.None && cell_list[cell.LD].RD != cell.D) {
                        return false;
                    }

                    if (cell.D != Cell.None && cell.RD != Cell.None && cell_list[cell.D].RU != cell.RD) {
                        return false;
                    }

                    if (cell.RD != Cell.None && cell.RU != Cell.None && cell_list[cell.RD].U != cell.RU) {
                        return false;
                    }

                    if (cell.RU != Cell.None && cell.U != Cell.None && cell_list[cell.RU].LU != cell.U) {
                        return false;
                    }
                }

                return true;
            }
        }

        /// <summary>Is Connected</summary>
        public bool IsConnected {
            get {
                if (Count < 1) {
                    return true;
                }

                List<int> searched_indexes = new(new int[] { Cell.None, cell_list.First().Index });
                Stack<int> searching_indexes = new();

                searching_indexes.Push(cell_list.First().Index);

                while (searching_indexes.Count > 0) {
                    int searching_index = searching_indexes.Pop();

                    foreach ((_, int linked_index) in cell_list[searching_index].IndexList) {
                        if (!searched_indexes.Contains(linked_index)) {
                            searched_indexes.Add(linked_index);
                            searching_indexes.Push(linked_index);
                        }
                    }
                }

                bool is_connected = searched_indexes.Count == Count + 1;

                return is_connected;
            }
        }

        /// <summary>To String Map</summary>
        public override string ToString() {
            int[,] map = Map;

            int digits = $"{map.Cast<int>().Max()}".Length + 2;

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
