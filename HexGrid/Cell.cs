using System;
using System.Collections.Generic;

namespace HexGrid {
    /// <summary>Cell</summary>
    public class Cell {

        /// <summary>None</summary>
        public const int None = -1;

        private readonly static Dictionary<Dir, (int dx, int dy)> dir_coord_table = new() {
            { Dir.LU, (-1, -1) },
            { Dir.RU, (+1, -1) },
            { Dir.L , (-2,  0) },
            { Dir.R , (+2,  0) },
            { Dir.LD, (-1, +1) },
            { Dir.RD, (+1, +1) },
        };

        /// <summary>Dir to Coord</summary>
        public static IReadOnlyDictionary<Dir, (int dx, int dy)> DirToCoord => dir_coord_table;

        /// <summary>Index</summary>
        public int Index { get; private set; }

        /// <summary>LeftUpper</summary>
        public int LU { get; internal set; } = None;
        
        /// <summary>RightUpper</summary>
        public int RU { get; internal set; } = None;

        /// <summary>Left</summary>
        public int L  { get; internal set; } = None;

        /// <summary>Right</summary>
        public int R  { get; internal set; } = None;
       
        /// <summary>LeftDowner</summary>
        public int LD { get; internal set; } = None;
        
        /// <summary>RightDowner</summary>
        public int RD { get; internal set; } = None;


        /// <summary>Coord X</summary>
        public int X { get; internal set; }

        /// <summary>Coord Y</summary>
        public int Y { get; internal set; }

        /// <summary>Indexer</summary>
        public int this[Dir dir] {
            get {
                return dir switch {
                    Dir.LU => LU,
                    Dir.RU => RU,
                    Dir.L  => L,
                    Dir.R  => R,
                    Dir.LD => LD,
                    Dir.RD => RD,
                    _ => throw new ArgumentException(nameof(dir)),
                };
            }

            internal set { 
                switch (dir) {
                    case Dir.LU:
                        LU = value;
                        break;
                    case Dir.RU:
                        RU = value;
                        break;
                    case Dir.L:
                        L = value;
                        break;
                    case Dir.R:
                        R = value;
                        break;
                    case Dir.LD:
                        LD = value;
                        break;
                    case Dir.RD:
                        RD = value;
                        break;
                    default:
                        throw new ArgumentException(nameof(dir));
                }
            }
        }

        /// <summary>Enum CellID</summary>
        public IEnumerable<(Dir dir, int index)> IndexList {
            get {
                if (LU != None) {
                    yield return (Dir.LU, LU);
                }
                if (RU != None) {
                    yield return (Dir.RU, RU);
                }
                if (L != None) {
                    yield return (Dir.L, L);
                }
                if (R != None) {
                    yield return (Dir.R, R);
                }
                if (LD != None) {
                    yield return (Dir.LD, LD);
                }
                if (RD != None) {
                    yield return (Dir.RD, RD);
                }
            }
        }

        /// <summary>Make Instance</summary>
        public Cell(int index) {
            Index = index;
        }

        /// <summary>Remap CellIndex</summary>
        internal void Remap(int src_index, int dst_index) {
            if (Index == src_index) {
                Index = dst_index;
            }
            if (LU == src_index) {
                LU = dst_index;
            }
            if (RU == src_index) {
                RU = dst_index;
            }
            if (L == src_index) {
                L = dst_index;
            }
            if (R == src_index) {
                R = dst_index;
            }
            if (LD == src_index) {
                LD = dst_index;
            }
            if (RD == src_index) {
                RD = dst_index;
            }
        }

        /// <summary>Validation</summary>
        internal bool IsValid(int cells) {
            foreach ((_, int id) in IndexList) {
                if (id < None || id >= cells || id == Index) {
                    return false;
                }
            }

            return true;
        }
    }
}
