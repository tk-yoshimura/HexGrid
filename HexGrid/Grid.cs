using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HexGrid {

    /// <summary>Generic Grid</summary>
    public class Grid {
        private readonly List<Cell> cell_list;

        /// <summary>Cell Counts</summary>
        public int Count => cell_list.Count;

        /// <summary>Coord Width</summary>
        public int CoordWidth { get; protected set; }

        /// <summary>Coord Height</summary>
        public int CoordHeight { get; protected set; }

        /// <summary>Indexer</summary>
        public Cell this[int index] {
            get { 
                return cell_list [index];
            }
        }

        public virtual (int coord_width, int coord_height) Size {
            get {
                return (CoordWidth, CoordHeight);
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
                else {
                    foreach (int dst_index in remap) {
                        cell_list[index].Remap(index, dst_index);
                    }
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
                }

                for (int index = 0; index < Count; index++) {
                    Cell cell = cell_list[index];
                    
                    if (cell.U  != Cell.None && cell.LU != Cell.None && cell_list[cell.U].LD  != cell.LU) {
                        return false;
                    }

                    if (cell.LU != Cell.None && cell.LD != Cell.None && cell_list[cell.LU].D  != cell.LD) {
                        return false;
                    }

                    if (cell.LD != Cell.None && cell.D  != Cell.None && cell_list[cell.LD].RD != cell.D) {
                        return false;
                    }

                    if (cell.D  != Cell.None && cell.RD != Cell.None && cell_list[cell.D].RU  != cell.RD) {
                        return false;
                    }

                    if (cell.RD != Cell.None && cell.RU != Cell.None && cell_list[cell.RD].U  != cell.RU) {
                        return false;
                    }

                    if (cell.RU != Cell.None && cell.U  != Cell.None && cell_list[cell.RU].LU != cell.U) {
                        return false;
                    }
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

                return true;
            }
        }

        /// <summary>Return Sparse Matrix</summary>
        public IEnumerable<IEnumerable<(Dir dir, int index)>> Link {
            get {
                foreach (Cell cell in cell_list) {
                    yield return cell.IndexList;
                }
            }
        }

        /// <summary>Move on Grid</summary>
        public int Move(int index, params Dir[] dirs) {
            if (index >= Count) {
                throw new IndexOutOfRangeException(nameof(index));
            }

            if (index < 0) {
                return Cell.None;
            }

            foreach (Dir dir in dirs) {
                index = cell_list[index][dir];

                if (index < 0) {
                    return Cell.None;
                }
            }

            return index;
        }

        /// <summary>Reflash Coord</summary>
        protected void ReflashCoord() {
            if (Count >= 1) {
                Cell cell_root = cell_list.First();

                List<int> searched_indexes = new(new int[] { Cell.None, cell_root.Index });
                Stack<int> searching_indexes = new();

                searching_indexes.Push(cell_root.Index);

                cell_root.X = 0;
                cell_root.Y = 0;

                while (searching_indexes.Count > 0) {
                    int searching_index = searching_indexes.Pop();
                    Cell cell_searching = cell_list[searching_index];

                    foreach ((Dir dir, int linked_index) in cell_list[searching_index].IndexList) {
                        if (!searched_indexes.Contains(linked_index)) {
                            searched_indexes.Add(linked_index);
                            searching_indexes.Push(linked_index);

                            Cell cell_linked = cell_list[linked_index];

                            (int dx, int dy) = Cell.DirToCoord[dir];

                            cell_linked.X = cell_searching.X + dx;
                            cell_linked.Y = cell_searching.Y + dy;
                        }
                    }
                }

                int min_coord_x = cell_list.Select((cell) => cell.X).Min();
                int min_coord_y = cell_list.Select((cell) => cell.Y).Min();

                if (min_coord_x != 0 || min_coord_y != 0) {
                    foreach (Cell cell in cell_list) {
                        cell.X -= min_coord_x;
                        cell.Y -= min_coord_y;
                    }
                }

                CoordWidth  = cell_list.Select((cell) => cell.X).Max() + 1;
                CoordHeight = cell_list.Select((cell) => cell.Y).Max() + 1;
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

        public string ToMap() { 
            int max_index = 0;
            int?[,] cells = new int?[CoordWidth, CoordHeight];

            foreach (Cell cell in cell_list) { 
                cells[cell.X, cell.Y] = cell.Index;

                if (max_index < cell.Index) {
                    max_index = cell.Index;
                }
            }

            int digits = $"{max_index}".Length;

            string cell_null = new(' ', digits);

            StringBuilder strbuilder = new();

            for (int y = 0; y < CoordHeight; y++) { 
                for (int x = 0; x < CoordWidth; x++) {
                    if (cells[x, y] is null) {
                        strbuilder.Append(cell_null);
                    }
                    else { 
                        strbuilder.Append($"{cells[x, y].Value}".PadLeft(digits));
                    }
                }

                strbuilder.Append("\n");
            }

            string str = strbuilder.ToString();

            return str;
        }
    }
}
