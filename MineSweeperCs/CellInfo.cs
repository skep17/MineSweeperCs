using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeperCs
{
    /// <summary>
    /// Cell info class.
    /// </summary>
    public class CellInfo
    {
        /// <summary>
        /// Number of mines around this cell.
        /// -1 indicates a mine in this cell.
        /// </summary>
        public int MineNum { get; set; } = 0;

        /// <summary>
        /// Indicates if this cell is opened (clicked) or not.
        /// </summary>
        public bool Opened { get; set; } = false;

        /// <summary>
        /// X coordinate of the cell in the grid.
        /// </summary>
        public int X { get; set; } = 0;

        /// <summary>
        /// Y coordinate of the cell in the grid.
        /// </summary>
        public int Y { get; set; } = 0;

    }
}
