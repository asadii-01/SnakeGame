namespace SnakeGame
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            pbCanvas = new PictureBox();
            lblScore = new Label();
            lblGameOver = new Label();
            gameTimer = new System.Windows.Forms.Timer(components);
            lblDifficulty = new Label();
            lblHighScore = new Label();
            lblEffect = new Label();
            ((System.ComponentModel.ISupportInitialize)pbCanvas).BeginInit();
            SuspendLayout();
            // 
            // pbCanvas
            // 
            pbCanvas.BackColor = Color.FloralWhite;
            pbCanvas.BackgroundImage = Properties.Resources.Board;
            pbCanvas.Location = new Point(21, 28);
            pbCanvas.Name = "pbCanvas";
            pbCanvas.Size = new Size(725, 538);
            pbCanvas.TabIndex = 0;
            pbCanvas.TabStop = false;
            pbCanvas.Paint += pbCanvas_Paint;
            // 
            // lblScore
            // 
            lblScore.AutoSize = true;
            lblScore.BackColor = Color.Transparent;
            lblScore.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblScore.ForeColor = Color.Honeydew;
            lblScore.Location = new Point(781, 68);
            lblScore.Name = "lblScore";
            lblScore.Size = new Size(105, 32);
            lblScore.TabIndex = 1;
            lblScore.Text = "Score: 0";
            // 
            // lblGameOver
            // 
            lblGameOver.BorderStyle = BorderStyle.FixedSingle;
            lblGameOver.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblGameOver.ForeColor = Color.Red;
            lblGameOver.Location = new Point(190, 181);
            lblGameOver.Name = "lblGameOver";
            lblGameOver.Size = new Size(362, 140);
            lblGameOver.TabIndex = 2;
            lblGameOver.Text = "Game Over !!!\r\nPress Enter to Retry";
            lblGameOver.TextAlign = ContentAlignment.MiddleCenter;
            lblGameOver.Visible = false;
            // 
            // gameTimer
            // 
            gameTimer.Tick += gameTimer_Tick;
            // 
            // lblDifficulty
            // 
            lblDifficulty.AutoSize = true;
            lblDifficulty.BackColor = Color.Transparent;
            lblDifficulty.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDifficulty.ForeColor = Color.Aquamarine;
            lblDifficulty.Location = new Point(781, 202);
            lblDifficulty.Name = "lblDifficulty";
            lblDifficulty.Size = new Size(192, 64);
            lblDifficulty.TabIndex = 3;
            lblDifficulty.Text = "Difficulty Level:\r\n1\r\n";
            lblDifficulty.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblHighScore
            // 
            lblHighScore.AutoSize = true;
            lblHighScore.BackColor = Color.Transparent;
            lblHighScore.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblHighScore.ForeColor = Color.Red;
            lblHighScore.Location = new Point(781, 133);
            lblHighScore.Name = "lblHighScore";
            lblHighScore.Size = new Size(173, 32);
            lblHighScore.TabIndex = 4;
            lblHighScore.Text = "High Score: 0 ";
            // 
            // lblEffect
            // 
            lblEffect.AllowDrop = true;
            lblEffect.AutoSize = true;
            lblEffect.BackColor = Color.Transparent;
            lblEffect.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblEffect.ForeColor = Color.Aquamarine;
            lblEffect.Location = new Point(781, 308);
            lblEffect.Name = "lblEffect";
            lblEffect.Size = new Size(155, 32);
            lblEffect.TabIndex = 5;
            lblEffect.Text = "Effect: None\r\n";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.Background;
            BackgroundImageLayout = ImageLayout.Zoom;
            ClientSize = new Size(1012, 587);
            Controls.Add(lblEffect);
            Controls.Add(lblHighScore);
            Controls.Add(lblDifficulty);
            Controls.Add(lblGameOver);
            Controls.Add(lblScore);
            Controls.Add(pbCanvas);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Snake Game";
            KeyDown += Form1_KeyDown;
            KeyUp += Form1_KeyUp;
            ((System.ComponentModel.ISupportInitialize)pbCanvas).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pbCanvas;
        private Label lblScore;
        private Label lblGameOver;
        private System.Windows.Forms.Timer gameTimer;
        private Label lblDifficulty;
        private Label lblHighScore;
        private Label lblEffect;
    }
}
