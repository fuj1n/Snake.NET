using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeSpeedrun
{
    public partial class Snake : Form
    {
        Graphics g;
        Graphics osg;
        Bitmap buffer;

        Timer t1;
        Random rand = new Random();

        int score;
        Label scoreLb;
        ScoreDisplay scoreFrm;

        Font goverFont = new Font(SystemFonts.DefaultFont.FontFamily, 18F, FontStyle.Bold);

        Point apple = new Point(5, 4);
        int psx = 12, psy = 12;

        int snakeStartBody = 5;

        int xdir, ydir;

        int gridSize = 20;
        int gridCount;

        bool incrementSnake = false;

        List<Point> snakes = new List<Point>();

        public Snake()
        {
            InitializeComponent();

            t1 = new Timer();
            t1.Interval = 1000 / 15;
            t1.Tick += DoGame;

            gridCount = Width / gridSize;

            scoreFrm = new ScoreDisplay();

            scoreLb = scoreFrm.scoreLb;
            scoreFrm.Show(this);

            scoreFrm.Top = Top;
            scoreFrm.Left = Right + 10;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            buffer = new Bitmap(Width, Height);
            g = Graphics.FromImage(buffer);
            osg = CreateGraphics();

            Init();
        }

        public void Init()
        {
            xdir = ydir = 0;
            score = 0;
            snakes.Clear();

            for (int i = 0; i < snakeStartBody; i++)
            {
                snakes.Add(new Point(psx + i, psy));
            }

            t1.Start();
        }

        public void DoGame(object sender, EventArgs e)
        {
            if (!scoreFrm.Visible)
            {
                Close();
                return;
            }

            if (scoreFrm.Focused)
                Focus();

            scoreLb.Text = "Score: " + score;

            Point lastPos = Point.Empty;
            for (int i = 0; i < snakes.Count; i++)
            {
                if (xdir == 0 && ydir == 0)
                    break;

                if (i == 0)
                {
                    int nextX = snakes[i].X + xdir;
                    int nextY = snakes[i].Y + ydir;

                    if (nextX < 0)
                        nextX = gridCount - 1;
                    if (nextY < 0)
                        nextY = gridCount - 1;
                    if (nextX >= gridCount)
                        nextX = 0;
                    if (nextY >= gridCount)
                        nextY = 0;

                    lastPos = new Point(nextX, nextY);
                }

                Point pos = snakes[i];
                snakes[i] = lastPos;
                lastPos = pos;
            }

            if (incrementSnake)
            {
                incrementSnake = false;
                snakes.Add(lastPos);
            }

            List<Point> snakeTest = new List<Point>();

            for (int i = 0; i < snakes.Count; i++)
            {
                if (snakeTest.Contains(snakes[i]))
                {
                    Lose();
                    return;
                }

                snakeTest.Add(snakes[i]);
            }

            if (snakes[0] == apple)
            {
                Score();
                apple = new Point(rand.Next(0, gridCount), rand.Next(0, gridCount));
                incrementSnake = true;
            }

            Render();
        }

        public void Render()
        {
            g.Clear(BackColor);

            g.FillRectangle(Brushes.Red, apple.X * gridSize, apple.Y * gridSize, gridSize, gridSize);

            for (int i = 0; i < snakes.Count; i++)
            {
                g.FillRectangle(Brushes.Lime, snakes[i].X * gridSize, snakes[i].Y * gridSize, gridSize, gridSize);
            }

            g.DrawRectangle(Pens.Cyan, 0, 0, Width - 1, Height - 1);

            osg.DrawImage(buffer, 0, 0);
        }

        public void Score()
        {
            score++;
        }

        public void Lose()
        {
            t1.Stop();
            Render();

            osg.Clear(BackColor);
            osg.DrawRectangle(Pens.Cyan, 0, 0, Width - 1, Height - 1);
            osg.DrawString("Game Over! Press space to try again!", goverFont, Brushes.White, Width / 2 - g.MeasureString("Game Over! Press space to try again!", goverFont).Width / 2, Height / 2);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    xdir = 0;
                    if (ydir != 1)
                        ydir = -1;
                    break;
                case Keys.Down:
                    xdir = 0;
                    if (ydir != -1)
                        ydir = 1;
                    break;
                case Keys.Left:
                    if (xdir != 1)
                        xdir = -1;
                    ydir = 0;
                    break;
                case Keys.Right:
                    if (xdir != -1)
                        xdir = 1;
                    ydir = 0;
                    break;
                case Keys.Space:
                    Init();
                    break;
            }
        }
    }
}
