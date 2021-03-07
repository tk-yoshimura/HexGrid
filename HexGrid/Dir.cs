using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexGrid {
    public enum Dir : int {
        /// <summary>Upper</summary>
        U,
        /// <summary>LeftUpper</summary>
        LU,
        /// <summary>RightUpper</summary>
        RU,
        /// <summary>LeftDowner</summary>
        LD,
        /// <summary>RightDowner</summary>
        RD,
        /// <summary>Downer</summary>
        D
    }
}
