using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Media;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class Form1 : Form
    {
        // Settings
        private List<Circle> Snake = new List<Circle>();
        private List<Circle> Walls = new List<Circle>();
        private Circle food = new Circle();
        private Circle? bonusFood = null;

        // Game Variables
        int maxWidth;
        int maxHeight;
        int score;
        int difficultyLevel;
        int highScore = 0;
        bool isPaused = false;
        int nextLevelThreshold = 50;

        // Assets
        Image? headImgUp, headImgDown, headImgRight, headImgLeft, bodyImgHorizontal, bodyImgVertical, tailImgUp, tailImgDown, tailImgLeft, tailImgRight;
        Image? foodImg, foodFastImg, foodSlowImg, foodBonusImg;
        Image? wallImg;
        SoundPlayer? eatSound, dieSound, introSound;

        public Form1()
        {
            InitializeComponent();

            new Settings();

            LoadHighScore();
            LoadAssets();
        }

        private void LoadAssets()
        {
            try { if (File.Exists("head-up.png")) headImgUp = Image.FromFile("head-up.png"); } catch { }
            try { if (File.Exists("head-down.png")) headImgDown = Image.FromFile("head-down.png"); } catch { }
            try { if (File.Exists("head-left.png")) headImgLeft = Image.FromFile("head-left.png"); } catch { }
            try { if (File.Exists("head-right.png")) headImgRight = Image.FromFile("head-right.png"); } catch { }

            try { if (File.Exists("tail-up.png")) tailImgUp = Image.FromFile("tail-up.png"); } catch { }
            try { if (File.Exists("tail-down.png")) tailImgDown = Image.FromFile("tail-down.png"); } catch { }
            try { if (File.Exists("tail-left.png")) tailImgLeft = Image.FromFile("tail-left.png"); } catch { }
            try { if (File.Exists("tail-right.png")) tailImgRight = Image.FromFile("tail-right.png"); } catch { }

            try { if (File.Exists("body-horizontal.png")) bodyImgHorizontal = Image.FromFile("body-horizontal.png"); } catch { }
            try { if (File.Exists("body-vertical.png")) bodyImgVertical = Image.FromFile("body-vertical.png"); } catch { }

            try { if (File.Exists("rock.png")) wallImg = Image.FromFile("rock.png"); } catch { }
            try { if (File.Exists("food.png")) foodImg = Image.FromFile("food.png"); } catch { }

            try { if (File.Exists("food_fast.png")) foodFastImg = Image.FromFile("food_fast.png"); } catch { }
            try { if (File.Exists("food_slow.png")) foodSlowImg = Image.FromFile("food_slow.png"); } catch { }
            try { if (File.Exists("food_bonus.png")) foodBonusImg = Image.FromFile("food_bonus.png"); } catch { }

            try {
                if (File.Exists("music.wav"))
                { introSound = new SoundPlayer("music.wav"); introSound.Load(); }
                else introSound = null;
            } catch { introSound = null; }
            try {
                if (File.Exists("food.wav"))
                { eatSound = new SoundPlayer("food.wav"); eatSound.Load(); }
                else eatSound = null;
            } catch { eatSound = null; }
            try {
                if (File.Exists("gameover.wav")) { dieSound = new SoundPlayer("gameover.wav"); dieSound.Load(); }
                else dieSound = null;
            } catch { dieSound = null; }
        }

        private void LoadHighScore()
        {
            if (File.Exists("highscore.txt"))
            {
                int.TryParse(File.ReadAllText("highscore.txt"), out highScore);
                lblHighScore.Text = "High Score: " + highScore;
            }
        }

        private void SaveHighScore()
        {
            if (Settings.Score > highScore)
            {
                highScore = Settings.Score;
                lblHighScore.Text = "High Score: " + highScore;
                File.WriteAllText("highscore.txt", highScore.ToString());
            }
        }

        private void InitTitleScreen()
        {
            Settings.State = GameState.Start;
            lblGameOver.Visible = false;
            lblScore.Text = "Score: 0";
            lblEffect.Text = "Effect: None";
            lblEffect.ForeColor = Color.Aquamarine;
            lblDifficulty.Text = "Difficulty Level:\n1";

            if (introSound != null) introSound.PlayLooping();

            gameTimer.Interval = 100;
            gameTimer.Start();
        }

        private void StartGame()
        {
            // Reset variables
            score = 0;
            difficultyLevel = 1;
            lblScore.Text = "Score: " + score;
            lblEffect.Text = "Effect: None";
            lblEffect.ForeColor = Color.Aquamarine;
            lblDifficulty.Text = "Difficulty Level:\n1";

            if (introSound != null) introSound.Stop();

            // Reset label visibility
            lblGameOver.Visible = false;

            // Reset Settings
            Settings.Width = 24;
            Settings.Height = 24;
            Settings.Score = 0;
            Settings.Points = 10;
            Settings.State = GameState.Playing;
            Settings.Direction = Directions.Down;

            nextLevelThreshold = 50;

            // Define map limits
            maxWidth = pbCanvas.Width / Settings.Width - 1;
            maxHeight = pbCanvas.Height / Settings.Height - 1;

            // Clear Game Objects
            Snake.Clear();
            Walls.Clear();

            // Add a head part to the snake logic
            Circle head = new Circle { X = 10, Y = 5 };
            Snake.Add(head);
            Circle body = new Circle
            {
                X = Snake[Snake.Count - 1].X,
                Y = Snake[Snake.Count - 1].Y
            };
            Snake.Add(body);

            GenerateFood();

            gameTimer.Interval = 100;
            gameTimer.Start();
        }

        private void GenerateFood()
        {
            // Randomly place food within bounds
            Random rand = new Random();
            food = new Circle { X = rand.Next(0, maxWidth), Y = rand.Next(0, maxHeight), Type = FoodType.Normal };

            if (rand.Next(0, 50) < 30)
            {
                bonusFood = new Circle { X = rand.Next(0, maxWidth), Y = rand.Next(0, maxHeight) };

                // Ensure it doesn't overlap with normal food
                if (bonusFood.X == food.X && bonusFood.Y == food.Y)
                {
                    bonusFood = null;
                }
                else
                {
                    // Randomize Type: 33% Fast, 33% Slow, 33% Bonus
                    int typeChance = rand.Next(0, 3);
                    if (typeChance == 0) bonusFood.Type = FoodType.Fast;
                    else if (typeChance == 1) bonusFood.Type = FoodType.Slow;
                    else bonusFood.Type = FoodType.Bonus;
                }
            }
            else
            {
                bonusFood = null;
            }
        }

        private void GenerateWalls()
        {
            Random rand = new Random();
            int wallsToAdd = 3;

            for (int i = 0; i < wallsToAdd; i++)
            {
                Circle wall = new Circle { X = rand.Next(0, maxWidth), Y = rand.Next(0, maxHeight) };

                if (wall.X != Snake[0].X && wall.Y != Snake[0].Y)
                {
                    Walls.Add(wall);
                }
            }
        }

        private void UpdateScreen(object sender, EventArgs e)
        {
            if (Settings.State == GameState.Start)
            {
                if (Input.KeyPressed(Keys.Enter))
                {
                    StartGame();
                }
            }
            else if (Settings.State == GameState.GameOver)
            {
                // If game is over, check for Enter key to restart
                if (Input.KeyPressed(Keys.Enter))
                {
                    StartGame();
                }
            }
            else if (Settings.State == GameState.Playing)
            {
                if (!isPaused)
                {
                    // Direction Control
                    if (Input.KeyPressed(Keys.Right) && Settings.Direction != Directions.Left)
                        Settings.Direction = Directions.Right;
                    else if (Input.KeyPressed(Keys.Left) && Settings.Direction != Directions.Right)
                        Settings.Direction = Directions.Left;
                    else if (Input.KeyPressed(Keys.Up) && Settings.Direction != Directions.Down)
                        Settings.Direction = Directions.Up;
                    else if (Input.KeyPressed(Keys.Down) && Settings.Direction != Directions.Up)
                        Settings.Direction = Directions.Down;

                    MovePlayer();
                }
            }

            pbCanvas.Invalidate();
        }

        private void MovePlayer()
        {
            for (int i = Snake.Count - 1; i >= 0; i--)
            {
                // Move Head
                if (i == 0)
                {
                    switch (Settings.Direction)
                    {
                        case Directions.Right:
                            Snake[i].X++;
                            break;
                        case Directions.Left:
                            Snake[i].X--;
                            break;
                        case Directions.Up:
                            Snake[i].Y--;
                            break;
                        case Directions.Down:
                            Snake[i].Y++;
                            break;
                    }

                    // 1. Border Collision Detection
                    if (Snake[i].X < 0 || Snake[i].Y < 0 || Snake[i].X > maxWidth || Snake[i].Y > maxHeight)
                    {
                        Die();
                    }

                    // 2. Body Collision Detection
                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[i].X == Snake[j].X && Snake[i].Y == Snake[j].Y)
                        {
                            Die();
                        }
                    }

                    // 3. Wall Collision (Obstacles)
                    foreach (Circle wall in Walls)
                    {
                        if (Snake[i].X == wall.X && Snake[i].Y == wall.Y)
                        {
                            Die();
                        }
                    }

                    // 4. Food Collision
                    if (Snake[0].X == food.X && Snake[0].Y == food.Y)
                    {
                        Eat();
                    }

                    if (bonusFood != null && Snake[0].X == bonusFood.X && Snake[0].Y == bonusFood.Y)
                    {
                        EatBonusFood();
                    }
                }
                else
                {
                    // Move Body: Follow the part before it
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }
        }

        private void CheckLevelUp()
        {
            if (Settings.Score >= nextLevelThreshold)
            {
                difficultyLevel++;
                lblDifficulty.Text = "Difficulty Level:\n" + difficultyLevel.ToString();
                gameTimer.Interval = Math.Max(25, gameTimer.Interval - 5);
                GenerateWalls();

                nextLevelThreshold += 50;
            }
        }

        private void Eat()
        {
            if (eatSound != null) eatSound.Play();

            Settings.Score += Settings.Points;
            lblScore.Text = "Score: " + Settings.Score.ToString();

            GrowSnake();
            CheckLevelUp();
            GenerateFood();
        }

        private void EatBonusFood()
        {
            if (eatSound != null) eatSound.Play();

            if(bonusFood == null) return;
            switch (bonusFood.Type)
            {
                case FoodType.Fast:
                    Settings.Score += 10;
                    gameTimer.Interval = Math.Max(20, gameTimer.Interval - 15);
                    lblEffect.Text = "Effect: Speed Up!";
                    lblEffect.ForeColor = Color.OrangeRed;
                    break;
                case FoodType.Slow:
                    Settings.Score += 10;
                    gameTimer.Interval = Math.Min(300, gameTimer.Interval + 15);
                    lblEffect.Text = "Effect: Slow Down!";
                    lblEffect.ForeColor = Color.Blue;
                    break;
                case FoodType.Bonus:
                    Settings.Score += 50;
                    lblEffect.Text = "Effect: +50 Points!";
                    lblEffect.ForeColor = Color.DarkGoldenrod;
                    break;
            }

            lblScore.Text = "Score: " + Settings.Score;

            GrowSnake();
            CheckLevelUp();

            bonusFood = null;
        }

        private void GrowSnake()
        {
            Circle body = new Circle
            {
                X = Snake[Snake.Count - 1].X,
                Y = Snake[Snake.Count - 1].Y
            };
            Snake.Add(body);
        }

        private void Die()
        {
            if (dieSound != null) dieSound.Play();

            SaveHighScore();

            Settings.State = GameState.GameOver;
            lblGameOver.Text = "Game Over \n Final Score: " + Settings.Score + "\n Press Enter to Retry";
            lblGameOver.Visible = true;
        }


        private void TogglePause()
        {
            if (Settings.State != GameState.Playing) return;

            isPaused = !isPaused;
            if (isPaused)
            {
                lblGameOver.Text = "PAUSED";
                lblGameOver.Visible = true;
            }
            else
            {
                lblGameOver.Visible = false;
            }
        }

        // --- EVENTS ---

        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            // DRAWING FOR START SCREEN
            if (Settings.State == GameState.Start)
            {
                string title = "SNAKE GAME";
                string subTitle = "Press Enter to Start";

                Font titleFont = new Font("Segoe UI", 32, FontStyle.Bold);
                Font subFont = new Font("Segoe UI", 16, FontStyle.Bold);
                Font bodyFont = new Font("Segoe UI", 12);
                Font smallFont = new Font("Segoe UI", 10);

                // Calculations for positioning
                float centerX = pbCanvas.Width / 2;
                float startY = 20;

                // 1. Draw Title
                SizeF titleSize = canvas.MeasureString(title, titleFont);
                canvas.DrawString(title, titleFont, Brushes.Gray, (centerX - titleSize.Width / 2) + 3, startY + 3);
                canvas.DrawString(title, titleFont, Brushes.DarkGreen, centerX - titleSize.Width / 2, startY);

                // 2. Draw Instruction Box
                int boxWidth = 320;
                int boxHeight = 280;
                int boxX = (int)(centerX - boxWidth / 2);
                int boxY = (int)startY + 80;

                Rectangle box = new Rectangle(boxX, boxY, boxWidth, boxHeight);
                canvas.FillRectangle(Brushes.WhiteSmoke, box);
                canvas.DrawRectangle(Pens.DarkGreen, box);

                // 3. Draw Instructions
                float lineX = boxX + 20;
                float lineY = boxY + 20;
                float spacing = 35;
                int iconSize = 20;

                canvas.DrawString("Move: Arrow Keys", bodyFont, Brushes.Black, lineX, lineY);
                lineY += spacing;

                canvas.DrawString("Avoid: Walls & Tail", bodyFont, Brushes.Black, lineX, lineY);
                lineY += spacing;

                canvas.DrawString("Food Types:", bodyFont, Brushes.Black, lineX, lineY);
                lineY += 25;

                if (foodImg != null) canvas.DrawImage(foodImg, lineX, lineY, iconSize, iconSize);
                else canvas.FillEllipse(Brushes.Red, lineX, lineY, iconSize, iconSize);
                canvas.DrawString("Normal (+10 Points)", smallFont, Brushes.DimGray, lineX + 25, lineY - 2);
                lineY += 25;

                if (foodFastImg != null) canvas.DrawImage(foodFastImg, lineX, lineY, iconSize, iconSize);
                else canvas.FillEllipse(Brushes.Orange, lineX, lineY, iconSize, iconSize);
                canvas.DrawString("Fast (+Speed)", smallFont, Brushes.DimGray, lineX + 25, lineY - 2);
                lineY += 25;

                if (foodSlowImg != null) canvas.DrawImage(foodSlowImg, lineX, lineY, iconSize, iconSize);
                else canvas.FillEllipse(Brushes.Blue, lineX, lineY, iconSize, iconSize);
                canvas.DrawString("Slow (-Speed)", smallFont, Brushes.DimGray, lineX + 25, lineY - 2);
                lineY += 25;

                if (foodBonusImg != null) canvas.DrawImage(foodBonusImg, lineX, lineY, iconSize, iconSize);
                else canvas.FillEllipse(Brushes.Gold, lineX, lineY, iconSize, iconSize);
                canvas.DrawString("Bonus (+50 Points)", smallFont, Brushes.DimGray, lineX + 25, lineY - 2);
                lineY += spacing;

                canvas.DrawString("Pause: 'P' or Space", bodyFont, Brushes.Black, lineX, lineY);

                // 4. Draw Prompt
                SizeF subSize = canvas.MeasureString(subTitle, subFont);
                canvas.DrawString(subTitle, subFont, Brushes.DarkRed, centerX - subSize.Width / 2, boxY + boxHeight + 20);
            }
            else if (Settings.State != GameState.Start)
            {

                // 1. Draw Visible Grid
                Pen gridPen = new Pen(Color.FromArgb(50, 0, 0, 0), 1);
                for (int x = 0; x * Settings.Width < pbCanvas.Width; x++)
                {
                    canvas.DrawLine(gridPen, x * Settings.Width, 0, x * Settings.Width, pbCanvas.Height);
                }
                for (int y = 0; y * Settings.Height < pbCanvas.Height; y++)
                {
                    canvas.DrawLine(gridPen, 0, y * Settings.Height, pbCanvas.Width, y * Settings.Height);
                }


                for (int i = 0; i < Snake.Count; i++)
                {
                    Rectangle rect = new Rectangle(Snake[i].X * Settings.Width,
                                                   Snake[i].Y * Settings.Height,
                                                   Settings.Width, Settings.Height);

                    if (i == 0) // HEAD
                    {
                        Image? currentHead = headImgUp;
                        switch (Settings.Direction)
                        {
                            case Directions.Up: currentHead = headImgUp; break;
                            case Directions.Down: currentHead = headImgDown; break;
                            case Directions.Left: currentHead = headImgLeft; break;
                            case Directions.Right: currentHead = headImgRight; break;
                        }

                        if (currentHead != null) canvas.DrawImage(currentHead, rect);
                        else canvas.FillEllipse(Brushes.Black, rect);
                    }
                    else if (i == Snake.Count - 1) // TAIL
                    {
                        Circle prevPart = Snake[i - 1];
                        Image? currentTail = tailImgDown;

                        if (prevPart.Y < Snake[i].Y) currentTail = tailImgUp;       
                        else if (prevPart.Y > Snake[i].Y) currentTail = tailImgDown; 
                        else if (prevPart.X < Snake[i].X) currentTail = tailImgLeft; 
                        else if (prevPart.X > Snake[i].X) currentTail = tailImgRight;

                        if (currentTail != null) canvas.DrawImage(currentTail, rect);
                        else canvas.FillEllipse(Brushes.DarkGreen, rect);
                    }
                    else // BODY
                    {
                        Circle prevPart = Snake[i - 1];
                        Circle nextPart = Snake[i + 1];

                        bool isVertical = (prevPart.X == Snake[i].X && nextPart.X == Snake[i].X);
                        bool isHorizontal = (prevPart.Y == Snake[i].Y && nextPart.Y == Snake[i].Y);

                        if (isVertical)
                        {
                            if (bodyImgVertical != null) canvas.DrawImage(bodyImgVertical, rect);
                            else canvas.FillEllipse(Brushes.DarkGreen, rect);
                        }
                        else if (isHorizontal)
                        {
                            if (bodyImgHorizontal != null) canvas.DrawImage(bodyImgHorizontal, rect);
                            else canvas.FillEllipse(Brushes.DarkGreen, rect);
                        }
                        else
                        {
                            if (bodyImgHorizontal != null) canvas.DrawImage(bodyImgHorizontal, rect);
                            else canvas.FillEllipse(Brushes.DarkGreen, rect);
                        }
                    }

                }

                // Draw Food
                Rectangle foodRect = new Rectangle(food.X * Settings.Width,
                                                   food.Y * Settings.Height,
                                                   Settings.Width, Settings.Height);
                if (foodImg != null) canvas.DrawImage(foodImg, foodRect);
                else canvas.FillEllipse(Brushes.Red, foodRect);

                // Draw BONUS Food (If exists)
                if (bonusFood != null)
                {
                    Rectangle bonusRect = new Rectangle(bonusFood.X * Settings.Width,
                                                        bonusFood.Y * Settings.Height,
                                                        Settings.Width, Settings.Height);

                    Image? bonusImage = null;
                    Brush bonusBrush = Brushes.Gold;

                    switch (bonusFood.Type)
                    {
                        case FoodType.Fast:
                            bonusImage = foodFastImg; bonusBrush = Brushes.Orange; break;
                        case FoodType.Slow:
                            bonusImage = foodSlowImg; bonusBrush = Brushes.Blue; break;
                        case FoodType.Bonus:
                            bonusImage = foodBonusImg; bonusBrush = Brushes.Gold; break;
                    }

                    if (bonusImage != null) canvas.DrawImage(bonusImage, bonusRect);
                    else canvas.FillEllipse(bonusBrush, bonusRect);
                }

                // Draw Walls
                foreach (Circle wall in Walls)
                {
                    Rectangle wallRect = new Rectangle(wall.X * Settings.Width,
                                                       wall.Y * Settings.Height,
                                                       Settings.Width, Settings.Height);
                    if (wallImg != null) canvas.DrawImage(wallImg, wallRect);
                    else canvas.FillRectangle(Brushes.Gray, wallRect);
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, true);

            // Pause Shortcut (P or Space)
            if (e.KeyCode == Keys.P || e.KeyCode == Keys.Space)
            {
                TogglePause();
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, false);
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            UpdateScreen(sender, e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitTitleScreen();
        }
    }

    // --- HELPER CLASSES ---

    class Circle
    {
        public int X { get; set; }
        public int Y { get; set; }

        public FoodType Type { get; set; } = FoodType.Normal;
    }

    // Global Game Settings
    public enum Directions { Left, Right, Up, Down };
    public enum GameState { Start, Playing, GameOver };
    public enum FoodType { Normal, Fast, Slow, Bonus }

    class Settings
    {
        public static int Width { get; set; }
        public static int Height { get; set; }
        public static int Score { get; set; }
        public static int Points { get; set; }
        public static GameState State { get; set; }
        public static Directions Direction { get; set; }

        public Settings()
        {
            Width = 24;
            Height = 24;
            Score = 0;
            Points = 10;
            State = GameState.Start;
            Direction = Directions.Down;
        }
    }

    // Input Handler to smooth out controls
    class Input
    {
        private static System.Collections.Hashtable keyTable = new System.Collections.Hashtable();

        public static bool KeyPressed(Keys key)
        {
            if (keyTable[key] == null)
            {
                return false;
            }
            return keyTable[key] is bool pressed && pressed;
        }

        public static void ChangeState(Keys key, bool state)
        {
            keyTable[key] = state;
        }
    }
}
