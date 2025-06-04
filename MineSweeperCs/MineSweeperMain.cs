using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MineSweeperCs
{
    public partial class MineSweeperMain : Form
    {
        public MineSweeperMain()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.Load += MineSweeperMain_Load;

            gameGrid = new MineSweeperGrid(this);
            gameGrid.GameFinish += (result) =>
            {
                if (result == GameFinishType.Win)
                {
                    FinishGame("Congratulations! You won the game!");
                }
                else if (result == GameFinishType.Lose)
                {
                    FinishGame("Game Over! You hit a mine.");
                }
            };
            gameGrid.MineCountUpdate += () =>
            {
                minesLabel.Text = $"Mines: {totalMines - gameGrid.FlaggedCount}";
            };
        }
    }
}
