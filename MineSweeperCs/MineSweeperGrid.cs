using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MineSweeperCs
{
    internal class MineSweeperGrid
    {
        public Button[,] GameGrid { get; private set; }
        public event Action<GameFinishType> GameFinish;
        public event Action MineCountUpdate;
        public int FlaggedCount { get; private set; } = 0;

        private Control parent;

        public MineSweeperGrid(Control parent)
        {
            this.parent = parent;
        }

        #region Manage Game Grid

        /// <summary>
        /// Creates a grid of buttons for the Minesweeper game.
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="rows"></param>
        /// <param name="mineCount"></param>
        public void CreateGrid(int columns, int rows, int mineCount)
        {
            ClearGrid();

            int clientWidth = parent.ClientSize.Width;
            int clientHeight = parent.ClientSize.Height;

            // The grid must not go above 1/4 of the window height
            int maxGridHeight = clientHeight * 3 / 4;

            // Calculate the largest possible button size with 1 button margin on each side and bottom
            int buttonSize = Math.Min(
                clientWidth / (columns + 2),
                maxGridHeight / (rows + 1)
            );

            int totalGridWidth = buttonSize * columns;
            int totalGridHeight = buttonSize * rows;

            // Margins: 1 button on each side and bottom
            int offsetX = (clientWidth - totalGridWidth) / 2;
            int offsetY = clientHeight - buttonSize * (rows + 1);

            GameGrid = new Button[columns, rows];

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    Button button = new Button
                    {
                        Size = new Size(buttonSize, buttonSize),
                        Location = new Point(offsetX + col * buttonSize, offsetY + row * buttonSize),
                        BackColor = Color.AntiqueWhite,
                        Name = $"button_{row}_{col}",
                        Text = "",
                        Tag = new CellInfo()
                    };
                    button.MouseDown += GridButtonMouseDown;
                    parent.Controls.Add(button);
                    GameGrid[col, row] = button;
                }
            }

            var points = GenerateRandomPoints(mineCount, columns, rows);
            UpdateGrid(points);
        }

        /// <summary>
        /// Updates the game grid with new mine positions.
        /// </summary>
        /// <param name="points"></param>
        public void UpdateGrid(List<Point> points)
        {
            foreach (Point p in points)
            {
                CellInfo c = GameGrid[p.X, p.Y].Tag as CellInfo;
                c.MineNum = -1;
                GameGrid[p.X, p.Y].Tag = c;

                for (int i = p.X - 1; i <= p.X + 1; i++)
                {
                    if (i < 0 || i >= GameGrid.GetLength(0)) continue;
                    for (int j = p.Y - 1; j <= p.Y + 1; j++)
                    {
                        if (j < 0 || j >= GameGrid.GetLength(1)) continue;
                        c = GameGrid[i, j].Tag as CellInfo;
                        if (c.MineNum >= 0) c.MineNum = c.MineNum + 1;
                        GameGrid[i, j].Tag = c;
                    }
                }
            }
        }

        /// <summary>
        /// Clears the game grid by removing all buttons from the parent control.
        /// </summary>
        public void ClearGrid()
        {
            if (GameGrid == null) return;
            foreach (var b in GameGrid)
            {
                parent.Controls.Remove(b);
            }
            GameGrid = null;
        }

        /// <summary>
        /// Handles the mouse down event for the buttons in the game grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridButtonMouseDown(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            CellInfo cellInfo = button.Tag as CellInfo;
            if (cellInfo == null) return;


            if (cellInfo.Opened == false)
            {
                if (e.Button == MouseButtons.Left)
                {
                    button.Image = null;
                    if (cellInfo.MineNum == -1)
                    {
                        cellInfo.Opened = true;
                        button.Tag = cellInfo;
                        button.BackColor = Color.Red;
                        GameFinish?.Invoke(GameFinishType.Lose);
                    }
                    else
                    {
                        button.Text = cellInfo.MineNum.ToString();
                        cellInfo.Opened = true;
                        button.Tag = cellInfo;
                        button.BackColor = cellInfo.MineNum > 0 ? Color.Yellow : Color.LightGreen;
                        if (CheckWin())
                        {
                            GameFinish?.Invoke(GameFinishType.Win);
                        }
                    }
                }
                else if (e.Button == MouseButtons.Right)
                {
                    if (button.Image == null)
                    {
                        button.Image = new Bitmap(Properties.Resources.Csharp_Logo, button.Size);
                        button.ImageAlign = ContentAlignment.MiddleCenter;
                        button.Text = "";
                        FlaggedCount++;
                    }
                    else
                    {
                        button.Image = null;
                        FlaggedCount--;
                    }
                    MineCountUpdate?.Invoke();
                }

            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Generates a list of random points within the specified width and height.
        /// Intended to be used for placing mines in the game grid.
        /// </summary>
        /// <param name="count"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public List<Point> GenerateRandomPoints(int count, int width, int height)
        {
            Random rand = new Random();
            HashSet<Point> points = new HashSet<Point>();
            while (points.Count < count)
            {
                int w = rand.Next(0, width);
                int h = rand.Next(0, height);
                points.Add(new Point(w, h));
            }
            return new List<Point>(points);
        }

        /// <summary>
        /// Checks if the game is won by verifying that all non-mine cells are opened.
        /// </summary>
        /// <returns></returns>
        private bool CheckWin()
        {
            if (GameGrid == null) return false;
            foreach (var b in GameGrid)
            {
                var info = b.Tag as CellInfo;
                if (!info.Opened && info.MineNum != -1)
                {
                    return false;
                }
            }
            return true;
        }

        #endregion
    }

    /// <summary>
    /// Enumeration for the type of game finish.
    /// </summary>
    public enum GameFinishType
    {
        Win,
        Lose,
        EarlyQuit
    }
}
