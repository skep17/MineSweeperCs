using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeperCs
{
    public class CellInfo
    {
        public int MineNum { get; set; } = 0;
        public bool Opened { get; set; } = false;
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;

    }
}
