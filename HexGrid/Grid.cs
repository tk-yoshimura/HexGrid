using System;
using System.Collections.Generic;
using System.Linq;

namespace HexGrid {

    /// <summary>Generic Grid</summary>
    public class Grid {
        private readonly List<Cell> cell_list;

        /// <summary>Cell Counts</summary>
        public int Count => cell_list.Count;

        /// <summary>Indexer</summary>
        public Cell this[int index] => cell_list[index];

        /// <summary>Make Instance</summary>
        internal Grid(int cells) {
            if (cells < 0) {
                throw new ArgumentException(nameof(cells));
            }

            this.cell_list = new List<Cell>((new Cell[cells]).Select(_ => new Cell()));
        }

        /// <summary>Remove Cells</summary>
        public void Remove(int[] cell_indexes) {
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
                for (int index = 0; index < Count; index++) {
                    Cell cell = cell_list[index];
                    
                    if (!cell.IsValid(index, Count)) {
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
               
        /// <summary>NeighborCells</summary>
        public int[] NeighborCells(int index, int range) {
            if (index < 0 || index >= Count) {
                throw new IndexOutOfRangeException(nameof(index));
            }
            if (range < 0) { 
                throw new IndexOutOfRangeException(nameof(range));
            }

            if (range == 0) {
                return new int[] { index };
            }

            int[] cells = new int[checked(range * 6)];

            cells[0]         = Move(index, Enumerable.Repeat(Dir.U,  range).ToArray());
            cells[range]     = Move(index, Enumerable.Repeat(Dir.RU, range).ToArray());
            cells[range * 2] = Move(index, Enumerable.Repeat(Dir.RD, range).ToArray());
            cells[range * 3] = Move(index, Enumerable.Repeat(Dir.D,  range).ToArray());
            cells[range * 4] = Move(index, Enumerable.Repeat(Dir.LD, range).ToArray());
            cells[range * 5] = Move(index, Enumerable.Repeat(Dir.LU, range).ToArray());

            for (int i = 1; i < range; i++) {
                cells[i]             = Move(cells[i - 1],             Dir.RD);
                cells[i + range]     = Move(cells[i + range - 1],     Dir.D);
                cells[i + range * 2] = Move(cells[i + range * 2 - 1], Dir.LD);
                cells[i + range * 3] = Move(cells[i + range * 3 - 1], Dir.LU);
                cells[i + range * 4] = Move(cells[i + range * 4 - 1], Dir.U);
                cells[i + range * 5] = Move(cells[i + range * 5 - 1], Dir.RU);
            }

            return cells;
        }

        /// <summary>Plot NeighborCells</summary>
        public string PlotNeighborCells(int index) {
            int[] n1 = NeighborCells(index, 1);
            int[] n2 = NeighborCells(index, 2);

            string str = string.Empty;

            str += $"             {n2[ 0],8}\n";
            str += $"      {n2[11],8},  {n2[ 1],8}\n";
            str += $"{n2[10],8},  {n1[ 0],8},  {n2[ 2],8}\n";
            str += $"      {n1[ 5],8},  {n1[ 1],8}\n";
            str += $"{n2[ 9],8},  {index ,8},  {n2[ 3],8}\n";
            str += $"      {n1[ 4],8},  {n1[ 2],8}\n";
            str += $"{n2[ 8],8},  {n1[ 3],8},  {n2[ 4],8}\n";
            str += $"      {n2[ 7],8},  {n2[ 5],8}\n";
            str += $"             {n2[ 6],8}\n";

            return str;
        }
    }
}
