using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MineSweeperCs
{
    partial class MineSweeperMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MineSweeperMain));
            this.SuspendLayout();

            this.InitializeCustomComponents();

            // 
            // MineSweeperMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(512, 512);
            this.Controls.Add(this.quitButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.easyButton);
            this.Controls.Add(this.mediumButton);
            this.Controls.Add(this.hardButton);
            this.Controls.Add(this.backButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MineSweeperMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MineSweeper";
            this.ResumeLayout(false);
        }

        #endregion

        #region Initialize Custom Components

        /// <summary>
        /// Initializes the custom components for the MineSweeper game.
        /// </summary>
        private void InitializeCustomComponents()
        {
            this.InitializeInitialMenuButtons();
            this.InitializeOptionsMenuButtons();
            this.InitializeGameInfo();
        }

        /// <summary>
        /// Initializes the buttons for the initial menu.
        /// </summary>
        private void InitializeInitialMenuButtons()
        {
            startButton = new Button
            {
                Name = "startButton",
                Text = "Start",
                BackColor = Color.SkyBlue,
                Visible = true,
                Enabled = true
            };
            startButton.Click += StartButtonClick;
            quitButton = new Button
            {
                Name = "quitButton",
                Text = "Quit",
                BackColor = Color.SkyBlue,
                Visible = true,
                Enabled = true
            };
            quitButton.Click += QuitButtonClick;
        }

        /// <summary>
        /// Initializes the buttons for the options menu.
        /// </summary>
        private void InitializeOptionsMenuButtons()
        {
            easyButton = new Button
            {
                Name = "easyButton",
                Text = "Easy",
                BackColor = Color.Green,
                Visible = false,
                Enabled = false
            };
            easyButton.Click += EasyButtonClick;
            mediumButton = new Button
            {
                Name = "mediumButton",
                Text = "Medium",
                BackColor = Color.Yellow,
                Visible = false,
                Enabled = false
            };
            mediumButton.Click += MediumButtonClick;
            hardButton = new Button
            {
                Name = "hardButton",
                Text = "Hard",
                BackColor = Color.Red,
                Visible = false,
                Enabled = false
            };
            hardButton.Click += HardButtonClick;
            backButton = new Button
            {
                Name = "backButton",
                Text = "Back",
                BackColor = Color.SkyBlue,
                Visible = false,
                Enabled = false
            };
            backButton.Click += BackButtonClick;
        }

        private void InitializeGameInfo()
        {
            if (minesLabel == null)
            {
                minesLabel = new Label
                {
                    AutoSize = true,
                    Font = new Font("Segoe UI", 18, FontStyle.Bold),
                    ForeColor = Color.White,
                    BackColor = Color.Transparent,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Visible = false
                };
                this.Controls.Add(minesLabel);
            }

            if (timerLabel == null)
            {
                timerLabel = new Label
                {
                    AutoSize = true,
                    Font = new Font("Segoe UI", 18, FontStyle.Bold),
                    ForeColor = Color.White,
                    BackColor = Color.Transparent,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Visible = false
                };
                this.Controls.Add(timerLabel);
            }

            if (finishGameButton == null)
            {
                finishGameButton = new Button
                {
                    Text = "Finish Game",
                    Font = new Font("Segoe UI", 14, FontStyle.Bold),
                    BackColor = Color.OrangeRed,
                    ForeColor = Color.White,
                    AutoSize = true,
                    Visible = false
                };
                finishGameButton.Click += FinishGameButtonClick;
                this.Controls.Add(finishGameButton);
            }

            if (gameTimer == null)
            {
                gameTimer = new Timer();
                gameTimer.Interval = 1000;
                gameTimer.Tick += (s, e) =>
                {
                    elapsedTime++;
                    UpdateTimerLabel();
                };
            }

            LayoutGameInfoControls();
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the click event for the start button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartButtonClick(object sender, EventArgs e)
        {
            this.ShowInitialMenu(false);

            this.ShowOptionsMenu(true);
        }

        /// <summary>
        /// Handles the click event for the quit button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuitButtonClick(object sender, EventArgs e)
        {
            // Close the application
            Application.Exit();
        }

        /// <summary>
        /// Handles the click event for the easy button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EasyButtonClick(object sender, EventArgs e)
        {
            this.ShowOptionsMenu(false);

            this.CreateEasyGrid();
        }

        /// <summary>
        /// Handles the click event for the medium button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MediumButtonClick(object sender, EventArgs e)
        {
            this.ShowOptionsMenu(false);

            this.CreateMediumGrid();
        }

        /// <summary>
        /// Handles the click event for the hard button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HardButtonClick(object sender, EventArgs e)
        {
            this.ShowOptionsMenu(false);

            this.CreateHardGrid();
        }

        /// <summary>
        /// Handles the click event for the back button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackButtonClick(object sender, EventArgs e)
        {
            this.ShowOptionsMenu(false);

            this.ShowInitialMenu(true);
        }

        /// <summary>
        /// Handles the click event for the finish game button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FinishGameButtonClick(object sender, EventArgs e)
        {
            this.FinishGame("Game Finished Early!");
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Shows the initial menu with start and quit buttons.
        /// </summary>
        /// <param name="arg"></param>
        private void ShowInitialMenu(bool arg = true)
        {
            startButton.Enabled = arg;
            startButton.Visible = arg;
            quitButton.Enabled = arg;
            quitButton.Visible = arg;
        }

        /// <summary>
        /// Shows the options menu with difficulty level buttons.
        /// </summary>
        /// <param name="arg"></param>
        private void ShowOptionsMenu(bool arg = true)
        {
            easyButton.Enabled = arg;
            easyButton.Visible = arg;
            mediumButton.Enabled = arg;
            mediumButton.Visible = arg;
            hardButton.Enabled = arg;
            hardButton.Visible = arg;
            backButton.Enabled = arg;
            backButton.Visible = arg;
        }

        /// <summary>
        /// Shows the game information controls such as mines left, timer, and finish game button.
        /// </summary>
        /// <param name="arg"></param>
        private void ShowGameInfoControls(bool arg = true)
        {
            minesLabel.Visible = arg;
            timerLabel.Visible = arg;
            finishGameButton.Enabled = arg;
            finishGameButton.Visible = arg;
        }

        /// <summary>
        /// Centers the initial menu buttons on the form.
        /// </summary>
        private void CenterInitialMenuButtons()
        {
            int totalHeight = menuButtonHeight * 2 + menuButtonGap;
            int startY = (this.ClientSize.Height - totalHeight) / 2;
            int centerX = (this.ClientSize.Width - menuButtonWidth) / 2;

            startButton.Location = new Point(centerX, startY);
            quitButton.Location = new Point(centerX, startY + menuButtonHeight + menuButtonGap);
        }

        /// <summary>
        /// Centers the options menu buttons on the form.
        /// </summary>
        private void CenterOptionsMenuButtons()
        {
            int totalHeight = menuButtonHeight * 4 + menuButtonGap * 3;
            int startY = (this.ClientSize.Height - totalHeight) / 2;
            int centerX = (this.ClientSize.Width - menuButtonWidth) / 2;

            easyButton.Location = new Point(centerX, startY);
            mediumButton.Location = new Point(centerX, startY + menuButtonHeight + menuButtonGap);
            hardButton.Location = new Point(centerX, startY + (menuButtonHeight + menuButtonGap) * 2);
            backButton.Location = new Point(centerX, startY + (menuButtonHeight + menuButtonGap) * 3);
        }

        /// <summary>
        /// Updates the sizes of the menu buttons based on the defined width and height.
        /// </summary>
        private void UpdateMenuButtonSizes()
        {
            startButton.Size = new Size(menuButtonWidth, menuButtonHeight);
            quitButton.Size = new Size(menuButtonWidth, menuButtonHeight);
            easyButton.Size = new Size(menuButtonWidth, menuButtonHeight);
            mediumButton.Size = new Size(menuButtonWidth, menuButtonHeight);
            hardButton.Size = new Size(menuButtonWidth, menuButtonHeight);
            backButton.Size = new Size(menuButtonWidth, menuButtonHeight);
        }

        /// <summary>
        /// Helper method to finish the game and display a message.
        /// </summary>
        /// <param name="message"></param>
        private void FinishGame(string message)
        {
            gameTimer.Stop();
            MessageBox.Show(message, "Game Finished", MessageBoxButtons.OK);
            gameGrid.ClearGrid();
            ShowGameInfoControls(false);
            this.ShowInitialMenu();
        }

        /// <summary>
        /// Layouts the game information controls in the center of the form.
        /// </summary>
        private void LayoutGameInfoControls()
        {
            int areaHeight = this.ClientSize.Height / 4;
            int centerY = areaHeight / 2;

            int spacing = 32;
            int buttonWidth = finishGameButton.Width;
            int minesWidth = minesLabel.Width;
            int timerWidth = timerLabel.Width;
            int totalWidth = minesWidth + timerWidth + buttonWidth + spacing * 2;

            int startX = (this.ClientSize.Width - totalWidth) / 2;

            timerLabel.Location = new Point(startX, centerY - minesLabel.Height / 2);
            minesLabel.Location = new Point(startX + minesWidth + spacing, centerY - timerLabel.Height / 2);
            finishGameButton.Location = new Point(startX + minesWidth + spacing + timerWidth + spacing, centerY - finishGameButton.Height / 2);
        }

        /// <summary>
        /// Updates the mines label to show the number of remaining mines.
        /// </summary>
        /// <param name="flaggedCount"></param>
        private void UpdateMinesLabel(int flaggedCount)
        {
            int remaining = totalMines - flaggedCount;
            minesLabel.Text = $"Mines: {remaining}";
            LayoutGameInfoControls();
        }

        /// <summary>
        /// Updates the timer label to show the elapsed time in minutes and seconds.
        /// </summary>
        private void UpdateTimerLabel()
        {
            int minutes = elapsedTime / 60;
            int seconds = elapsedTime % 60;
            timerLabel.Text = $"Time: {minutes}:{seconds}";
            LayoutGameInfoControls();
        }

        #endregion

        #region Grid Creation Methods

        /// <summary>
        /// Creates an easy grid for the Minesweeper game.
        /// </summary>
        private void CreateEasyGrid()
        {
            int gridWidth = 8, gridHeight = 8, mineCount = 10;
            CreateGameGrid(gridWidth, gridHeight, mineCount);
        }

        /// <summary>
        /// Creates a medium grid for the Minesweeper game.
        /// </summary>
        private void CreateMediumGrid()
        {
            int gridWidth = 16, gridHeight = 16, mineCount = 40;
            CreateGameGrid(gridWidth, gridHeight, mineCount);
        }

        /// <summary>
        /// Creates a hard grid for the Minesweeper game.
        /// </summary>
        private void CreateHardGrid()
        {
            int gridWidth = 30, gridHeight = 16, mineCount = 99;
            CreateGameGrid(gridWidth, gridHeight, mineCount);
        }

        /// <summary>
        /// Creates a game grid with the specified dimensions and mine count.
        /// </summary>
        /// <param name="gridWidth"></param>
        /// <param name="gridHeight"></param>
        /// <param name="mineCount"></param>
        private void CreateGameGrid(int gridWidth, int gridHeight, int mineCount)
        {
            totalMines = mineCount;
            elapsedTime = 0;
            UpdateTimerLabel();
            UpdateMinesLabel(0);
            ShowGameInfoControls(true);

            gameGrid.CreateGrid(gridWidth, gridHeight, mineCount);

            ShowGameInfoControls(true);

            gameTimer.Start();
        }

        #endregion

        private void MineSweeperMain_Load(object sender, EventArgs e)
        {
            menuButtonWidth = this.ClientSize.Width / 4;
            menuButtonHeight = this.ClientSize.Height / 16;
            menuButtonGap = menuButtonHeight / 2;

            UpdateMenuButtonSizes();

            CenterInitialMenuButtons();
            CenterOptionsMenuButtons();
        }

        private int menuButtonWidth;
        private int menuButtonHeight;
        private int menuButtonGap;

        private Button startButton;
        private Button quitButton;
        private Button easyButton;
        private Button mediumButton;
        private Button hardButton;
        private Button backButton;

        private Label minesLabel;
        private Label timerLabel;
        private Button finishGameButton;
        private Timer gameTimer;
        private int elapsedTime;
        private int totalMines;

        private MineSweeperGrid gameGrid;
    }
}

