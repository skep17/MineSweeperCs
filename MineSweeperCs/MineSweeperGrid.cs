using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
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
        /// Creates a new game grid with the specified dimensions and number of mines.
        /// </summary>
        /// <remarks>This method initializes a grid of buttons representing the game cells, calculates
        /// their size and  position based on the available client area, and places the specified number of mines
        /// randomly  within the grid. Each button is associated with a <see cref="CellInfo"/> object containing 
        /// metadata about the cell.  The grid is cleared before being recreated, and the buttons are added to the
        /// parent control.</remarks>
        /// <param name="columns">The number of columns in the grid. Must be greater than zero.</param>
        /// <param name="rows">The number of rows in the grid. Must be greater than zero.</param>
        /// <param name="mineCount">The number of mines to place in the grid. Must be less than or equal to the total number of cells  (columns
        /// * rows) and greater than or equal to zero.</param>
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
                        Tag = new CellInfo
                        {
                            X = col,
                            Y = row,
                            MineNum = 0,
                            Opened = false
                        }
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
        /// Updates the grid by marking specified points as mines and incrementing the mine count  for adjacent cells.
        /// </summary>
        /// <remarks>This method modifies the state of the grid by setting the <c>MineNum</c> property of
        /// the  cells at the specified points to -1, indicating they are mines. It also increments the  <c>MineNum</c>
        /// property of all adjacent cells for each specified point.  The grid is assumed to be a two-dimensional array
        /// where each cell's <c>Tag</c> property  contains a <see cref="CellInfo"/> object. The caller must ensure that
        /// the <c>points</c>  parameter contains valid coordinates within the grid's dimensions.</remarks>
        /// <param name="points">A list of <see cref="Point"/> objects representing the locations to be marked as mines. Each point must have
        /// valid coordinates within the bounds of the grid.</param>
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
        /// Clears all elements from the game grid and removes their associated controls from the parent container.
        /// </summary>
        /// <remarks>This method removes all controls associated with the game grid from the parent
        /// container and sets the game grid to null. If the game grid is already null, the method does
        /// nothing.</remarks>
        public void ClearGrid()
        {
            if (GameGrid == null) return;
            foreach (var b in GameGrid)
            {
                parent.Controls.Remove(b);
            }
            GameGrid = null;
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the mouse down event for a grid button, enabling interaction with the game grid.
        /// </summary>
        /// <remarks>This method processes user interactions with grid buttons, such as opening a cell or
        /// performing actions based on neighboring flags. - If the button's associated cell is unopened, it attempts to
        /// open the cell. - If the cell is already opened and the left mouse button is double-clicked, it checks
        /// neighboring flags and attempts to open neighboring cells if conditions are met.</remarks>
        /// <param name="sender">The button that triggered the event. Must be a <see cref="Button"/> with a <see cref="CellInfo"/> object in
        /// its <see cref="Button.Tag"/> property.</param>
        /// <param name="e">The mouse event arguments containing details about the mouse action, such as the button pressed and the
        /// number of clicks.</param>
        private void GridButtonMouseDown(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            CellInfo cellInfo = button.Tag as CellInfo;
            if (cellInfo == null) return;


            if (cellInfo.Opened == false)
            {
                TryButtonOpen(sender, e.Button, true);
            }
            else
            {
                if (e.Button == MouseButtons.Left && e.Clicks == 2)
                {
                    if (CountNeighboringFlags(sender) == cellInfo.MineNum)
                    {
                        TryButtonOpen(sender, MouseButtons.Left, false);
                    }
                }
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Generates a list of unique random points within the specified dimensions.
        /// </summary>
        /// <remarks>The method ensures that all generated points are unique. If the specified <paramref
        /// name="count"/> exceeds the total number of possible unique points within the given dimensions (<paramref
        /// name="width"/> × <paramref name="height"/>), the method will enter an infinite loop.</remarks>
        /// <param name="count">The number of unique points to generate. Must be greater than 0.</param>
        /// <param name="width">The maximum width of the area in which points can be generated. Must be greater than 0.</param>
        /// <param name="height">The maximum height of the area in which points can be generated. Must be greater than 0.</param>
        /// <returns>A list of <see cref="Point"/> objects representing the generated points. The list will contain exactly
        /// <paramref name="count"/> points.</returns>
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
        /// Determines whether the current game state represents a win condition.
        /// </summary>
        /// <remarks>A win condition is achieved when all cells that are not mines have been opened. This
        /// method returns <see langword="false"/> if the game grid is null or if any  non-mine cell remains
        /// unopened.</remarks>
        /// <returns><see langword="true"/> if all non-mine cells in the game grid are opened;  otherwise, <see
        /// langword="false"/>.</returns>
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

        /// <summary>
        /// Attempts to handle a button click event based on the mouse button pressed.
        /// </summary>
        /// <remarks>This method performs different actions depending on the mouse button pressed: <list
        /// type="bullet"> <item><description>If the left mouse button is pressed, the button is opened, revealing its
        /// associated cell information. If the cell contains a mine, the game ends with a loss. If all non-mine cells
        /// are opened, the game ends with a win.</description></item> <item><description>If the right mouse button is
        /// pressed, the button is flagged or unflagged, updating the flagged count and triggering a mine count update
        /// event.</description></item> </list> The method invokes the <see cref="GameFinish"/> event when the game ends
        /// and the <see cref="MineCountUpdate"/> event when the flagged count changes.</remarks>
        /// <param name="sender">The button that triggered the event. Must be of type <see cref="Button"/>.</param>
        /// <param name="mouseButtons">The mouse button used to trigger the event. Expected values are <see cref="MouseButtons.Left"/> or <see
        /// cref="MouseButtons.Right"/>.</param>
        private bool TryButtonOpen(Object sender, MouseButtons mouseButtons, bool safeOpen = true)
        {
            bool ret = false;
            
            Button button = sender as Button;
            if (button == null) return ret;

            CellInfo cellInfo = button.Tag as CellInfo;
            if (cellInfo == null) return ret;

            if (mouseButtons == MouseButtons.Left)
            {
                button.Image = null;
                if (cellInfo.MineNum == -1)
                {
                    button.Image = new Bitmap(Properties.Resources.Mine_Icon, button.Size);
                    button.ImageAlign = ContentAlignment.MiddleCenter;
                    button.Text = "";
                    button.BackColor = Color.Red;
                    GameFinish?.Invoke(GameFinishType.Lose);
                }
                else
                {
                    OpenButton(sender, safeOpen);
                    if (CheckWin())
                    {
                        GameFinish?.Invoke(GameFinishType.Win);
                    }
                    ret = true;
                }
            }
            else if (mouseButtons == MouseButtons.Right)
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

            return ret;
        }

        /// <summary>
        /// Handles the opening of a button in the game grid.
        /// </summary>
        /// <remarks>This method processes the button click event, revealing the associated cell's
        /// information. If the cell contains a number of adjacent mines, the button displays the count and changes its
        /// background color. If the cell has no adjacent mines, the method recursively opens neighboring
        /// cells.</remarks>
        /// <param name="sender">The button object that triggered the event. Must be of type <see cref="Button"/> and contain valid tag data.</param>
        private void OpenButton(Object sender, bool safeOpen = true)
        {
            Button button = sender as Button;
            if (button == null || button.Image != null) return;
            CellInfo cellInfo = button.Tag as CellInfo;
            if (cellInfo == null || (cellInfo.Opened && safeOpen)) return;
            button.Text = cellInfo.MineNum.ToString();
            cellInfo.Opened = true;
            button.Tag = cellInfo;
            if (cellInfo.MineNum > 0 && safeOpen)
            {
                button.BackColor = Color.Yellow;
            }
            else
            {
                if (safeOpen) button.BackColor = Color.LightGreen;
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i == 0 && j == 0) continue;
                        int newX = cellInfo.X + i;
                        int newY = cellInfo.Y + j;
                        if (newX >= 0 && newX < GameGrid.GetLength(0) && newY >= 0 && newY < GameGrid.GetLength(1))
                        {
                            if (GameGrid[newX, newY].Image == null && !TryButtonOpen(GameGrid[newX, newY], MouseButtons.Left)) return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Counts the number of neighboring cells that have a flag image.
        /// </summary>
        /// <remarks>This method evaluates the eight neighboring cells surrounding the cell represented by
        /// the sender's <see cref="CellInfo"/> tag. A cell is considered to have a flag if its <see cref="Image"/>
        /// property is not null.</remarks>
        /// <param name="sender">The object that triggered the event, expected to be a <see cref="Button"/> with a <see cref="CellInfo"/>
        /// tag.</param>
        /// <returns>The number of neighboring cells that contain a flag image. Returns 0 if the sender is not a valid <see
        /// cref="Button"/> or its tag is not a valid <see cref="CellInfo"/>.</returns>
        private int CountNeighboringFlags(Object sender) {
            Button button = sender as Button;
            if (button == null) return 0;
            CellInfo cellInfo = button.Tag as CellInfo;
            if (cellInfo == null) return 0;

            int count = 0;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;
                    int newX = cellInfo.X + i;
                    int newY = cellInfo.Y + j;
                    if (newX >= 0 && newX < GameGrid.GetLength(0) && newY >= 0 && newY < GameGrid.GetLength(1))
                    {
                        var neighborCell = GameGrid[newX, newY];
                        if (neighborCell.Image != null)
                        {
                            count++;
                        }
                    }
                }
            }
            return count;
        }

        #endregion
    }

    /// <summary>
    /// Represents the possible outcomes of a game session.
    /// </summary>
    /// <remarks>This enumeration defines the different ways a game session can conclude,  including winning,
    /// losing, or quitting early.</remarks>
    public enum GameFinishType
    {
        Win,
        Lose,
        EarlyQuit
    }
}
