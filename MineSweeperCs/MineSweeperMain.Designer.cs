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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MineSweeperMain));
            this.startButton = new System.Windows.Forms.Button();
            this.quitButton = new System.Windows.Forms.Button();
            this.easyButton = new System.Windows.Forms.Button();
            this.mediumButton = new System.Windows.Forms.Button();
            this.hardButton = new System.Windows.Forms.Button();
            this.backButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.BackColor = System.Drawing.Color.SkyBlue;
            this.startButton.Location = new System.Drawing.Point(206, 200);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(100, 50);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = false;
            this.startButton.Click += new System.EventHandler(this.startButtonClick);
            // 
            // quitButton
            // 
            this.quitButton.BackColor = System.Drawing.Color.SkyBlue;
            this.quitButton.Location = new System.Drawing.Point(206, 272);
            this.quitButton.Name = "quitButton";
            this.quitButton.Size = new System.Drawing.Size(100, 50);
            this.quitButton.TabIndex = 1;
            this.quitButton.Text = "Quit";
            this.quitButton.UseVisualStyleBackColor = false;
            this.quitButton.Click += new System.EventHandler(this.quitButtonClick);
            //
            // easyButton
            //
            this.easyButton.BackColor = System.Drawing.Color.Green;
            this.easyButton.Location = new System.Drawing.Point(206, 138);
            this.easyButton.Name = "easyButton";
            this.easyButton.Size = new System.Drawing.Size(100, 50);
            this.easyButton.TabIndex = 2;
            this.easyButton.Text = "Easy";
            this.easyButton.UseVisualStyleBackColor = false;
            this.easyButton.Visible = false;
            this.easyButton.Click += new System.EventHandler(this.easyButtonClick);
            //
            // mediumButton
            //
            this.mediumButton.BackColor = System.Drawing.Color.Yellow;
            this.mediumButton.Location = new System.Drawing.Point(206, 200);
            this.mediumButton.Name = "mediumButton";
            this.mediumButton.Size = new System.Drawing.Size(100, 50);
            this.mediumButton.TabIndex = 3;
            this.mediumButton.Text = "Medium";
            this.mediumButton.UseVisualStyleBackColor = false;
            this.mediumButton.Visible = false;
            this.mediumButton.Click += new System.EventHandler(this.mediumButtonClick);
            //
            // hardButton
            //
            this.hardButton.BackColor = System.Drawing.Color.Red;
            this.hardButton.Location = new System.Drawing.Point(206, 272);
            this.hardButton.Name = "hardButton";
            this.hardButton.Size = new System.Drawing.Size(100, 50);
            this.hardButton.TabIndex = 4;
            this.hardButton.Text = "Hard";
            this.hardButton.UseVisualStyleBackColor = false;
            this.hardButton.Visible = false;
            this.hardButton.Click += new System.EventHandler(this.hardButtonClick);
            //
            // backButton
            //
            this.backButton.BackColor = System.Drawing.Color.SkyBlue;
            this.backButton.Location = new System.Drawing.Point(206, 334);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(100, 50);
            this.backButton.TabIndex = 5;
            this.backButton.Text = "Back";
            this.backButton.UseVisualStyleBackColor = false;
            this.backButton.Visible = false;
            this.backButton.Click += new System.EventHandler(this.backButtonClick);
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

        private void showInitialMenu(bool arg = true)
        {
            startButton.Enabled = arg;
            startButton.Visible = arg;
            quitButton.Enabled = arg;
            quitButton.Visible = arg;
        }

        private void showOptionsMenu(bool arg = true)
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

        private void startButtonClick(object sender, EventArgs e)
        {
            this.showInitialMenu(false);

            this.showOptionsMenu(true);
        }

        private void quitButtonClick(object sender, EventArgs e)
        {
            // Close the application
            Application.Exit();
        }
        
        private void easyButtonClick(object sender, EventArgs e)
        {
            this.showOptionsMenu(false);

            this.createEasyGrid();
        }

        private void mediumButtonClick(object sender, EventArgs e)
        {
            this.showOptionsMenu(false);
        }

        private void hardButtonClick(object sender, EventArgs e)
        {
            this.showOptionsMenu(false);
        }

        private void backButtonClick(object sender, EventArgs e)
        {
            this.showOptionsMenu(false);

            this.showInitialMenu(true);
        }

        private bool checkWin()
        {
            bool ret = true;

            foreach (var b in gameGrid)
            {
                var info = b.Tag as CellInfo;
                if (!info.Opened && info.MineNum != -1)
                {
                    ret = false;
                    break;
                }
            }

            return ret;
        }

        private void blankButtonMouseDown(object sender, MouseEventArgs e)
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
                        MessageBox.Show("Game Over!");
                        this.clearGrid();
                        this.showInitialMenu();
                    }
                    else
                    {
                        button.Text = cellInfo.MineNum.ToString();
                        cellInfo.Opened = true;
                        button.Tag = cellInfo;
                        button.BackColor = Color.LightGreen;
                        if (checkWin())
                        {
                            MessageBox.Show("You Win!");
                            this.clearGrid();
                            this.showInitialMenu();
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
                    }
                    else
                    {
                        button.Image = null;
                    }
                }
                   
            }
        }

        private void createGrid(int width, int height, int offset, int buttonSize)
        {
            this.gameGrid = new Button[width,height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Button button = new Button();

                    button.Size = new System.Drawing.Size(buttonSize, buttonSize);
                    button.Location = new Point(offset + j * buttonSize, 100 + offset + i * buttonSize);
                    button.BackColor = Color.AntiqueWhite;
                    button.Name = $"button_{i}_{j}";
                    button.Text = "";
                    button.Tag = new CellInfo();
                    button.MouseDown += blankButtonMouseDown;

                    this.Controls.Add(button);
                    this.gameGrid[i, j] = button;
                }
            }
        }

        private void clearGrid()
        {
            foreach (var b in  this.gameGrid)
            {
                Controls.Remove(b);
            }
            gameGrid = null;
        }

        private List<Point> generateRandomPoints(int count, int width, int height)
        {
            Random rand = new Random();
            HashSet<Point> points = new HashSet<Point>();

            while (points.Count < count)
            {
                int w = rand.Next(0, width);
                int h = rand.Next(0, height);
                Point p = new Point(w, h);

                points.Add(p);
            }

            return new List<Point>(points);
        }

        private void updateGrid(List<Point> points)
        {
            foreach (Point p in points)
            {
                CellInfo c = gameGrid[p.X, p.Y].Tag as CellInfo;
                c.MineNum = -1;
                gameGrid[p.X, p.Y].Tag = c;

                for (int i = p.X - 1; i <= p.X + 1; i++)
                {
                    if (i < 0 || i >= gameGrid.GetLength(0)) continue;
                    for (int j = p.Y - 1; j <= p.Y + 1; j++)
                    {
                        if (j < 0 || j >= gameGrid.GetLength(1)) continue;
                        c = gameGrid[i,j].Tag as CellInfo;
                        if (c.MineNum >= 0) c.MineNum = c.MineNum + 1;
                        gameGrid[i, j].Tag = c;
                    }
                }
            }
        }

        private void createEasyGrid()
        {
            this.createGrid(8, 8, 6, 25);
            List<Point> points = this.generateRandomPoints(10, 8, 8);
            updateGrid(points);
        }


        #endregion

        private Button startButton;
        private Button quitButton;
        private Button easyButton;
        private Button mediumButton;
        private Button hardButton;
        private Button backButton;

        private Button[,] gameGrid;
    }
}

