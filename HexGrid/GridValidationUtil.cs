using System.Collections.Generic;
using System.Linq;

namespace HexGrid {

    /// <summary>Validate Grid</summary>
    public static class GridValidationUtil {

        /// <summary>Validation</summary>
        public static bool IsValid(Grid grid) {
            if (!IsValidCellIndex(grid)) {
                return false;
            }

            if (!IsValidReciprocalConnects(grid)) {
                return false;
            }

            if (!IsValidReferenceCounts(grid)) {
                return false;
            }

            if (!IsAllConnected(grid)) {
                return false;
            }

            if (!IsValidMapSize(grid)) {
                return false;
            }

            if (!IsValidCoord(grid)) {
                return false;
            }

            return true;
        }

        /// <summary>Is Valid Cell Index</summary>
        public static bool IsValidCellIndex(Grid grid) {
            int i = 0;
            foreach (Cell cell in grid.Cells) {
                if (!cell.IsValid(grid.Count)) {
                    return false;
                }
                if (i != cell.Index) {
                    return false;
                }

                i++;
            }

            int index_max = grid.Cells.Select((cell) => cell.Index).Max();

            if (index_max + 1 != grid.Count) {
                return false;
            }

            return true;
        }

        /// <summary>Is Valid Connects</summary>
        public static bool IsValidReciprocalConnects(Grid grid) {
            IReadOnlyList<Cell> cell_list = grid.Cells;

            for (int index = 0; index < grid.Count; index++) {
                Cell cell = cell_list[index];

                if (cell.LU != Cell.None && cell_list[cell.LU].RD != index) {
                    return false;
                }
                if (cell.LD != Cell.None && cell_list[cell.LD].RU != index) {
                    return false;
                }
                if (cell.L != Cell.None && cell_list[cell.L].R != index) {
                    return false;
                }
                if (cell.R != Cell.None && cell_list[cell.R].L != index) {
                    return false;
                }
                if (cell.RU != Cell.None && cell_list[cell.RU].LD != index) {
                    return false;
                }
                if (cell.RD != Cell.None && cell_list[cell.RD].LU != index) {
                    return false;
                }

                if (cell.LU != Cell.None && cell.RU != Cell.None && cell_list[cell.LU].R != cell.RU) {
                    return false;
                }

                if (cell.RU != Cell.None && cell.R != Cell.None && cell_list[cell.RU].RD != cell.R) {
                    return false;
                }

                if (cell.R != Cell.None && cell.RD != Cell.None && cell_list[cell.R].LD != cell.RD) {
                    return false;
                }

                if (cell.RD != Cell.None && cell.LD != Cell.None && cell_list[cell.RD].L != cell.LD) {
                    return false;
                }

                if (cell.LD != Cell.None && cell.L != Cell.None && cell_list[cell.LD].LU != cell.L) {
                    return false;
                }

                if (cell.L != Cell.None && cell.LU != Cell.None && cell_list[cell.L].RU != cell.LU) {
                    return false;
                }
            }

            return true;
        }

        /// <summary>Is Valid ReferenceCounts</summary>
        public static bool IsValidReferenceCounts(Grid grid) {
            int[] ref_counts = new int[grid.Count];

            foreach (Cell cell in grid.Cells) {
                foreach ((_, int index) in cell.IndexList) {
                    ref_counts[index]++;
                }
            }

            if (ref_counts.Any(count => count > 6)) {
                return false;
            }

            return true;
        }

        /// <summary>Is All Connected</summary>
        public static bool IsAllConnected(Grid grid) {
            IReadOnlyList<Cell> cell_list = grid.Cells;

            if (grid.Count < 1) {
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

            bool is_connected = searched_indexes.Count == grid.Count + 1;

            return is_connected;
        }

        /// <summary>Is Valid MapSize</summary>
        public static bool IsValidMapSize(Grid grid) {
            if (grid.Cells.Select((cell) => cell.X).Min() != 0) {
                return false;
            }

            if (grid.Cells.Select((cell) => cell.Y).Min() != 0) {
                return false;
            }

            if (grid.Cells.Select((cell) => cell.Y).Max() + 1 != grid.MapHeight) {
                return false;
            }

            if (grid.Cells.Select((cell) => cell.X).Max() + 1 != grid.MapWidth) {
                return false;
            }

            return true;
        }

        /// <summary>Is Valid Coord</summary>
        public static bool IsValidCoord(Grid grid) {
            int[,] map = grid.Map;

            int counts = map.OfType<int>().Where((index) => index != Cell.None).Count();

            if (counts != grid.Count) {
                return false;
            }

            if (grid.Count < 1) {
                return true;
            }

            int offset_x = grid.Cells.First().X % 2;
            int offset_y = grid.Cells.First().Y % 2;

            for (int y = 0; y < map.GetLength(1); y++) {
                for (int x = 0; x < map.GetLength(0); x++) {
                    if (map[x, y] != Cell.None) {
                        if (y % 2 == offset_y && x % 2 != offset_x) {
                            return false;
                        }
                        else if (y % 2 != offset_y && x % 2 == offset_x) {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}
