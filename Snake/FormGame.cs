using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public partial class FormGame : Form
    {
        #region Constructor
        #region Variable
        int score = 0;
        bool gameover = false;
        int direction = 0; // 0 -> down, 1->left, 2-> right, 3->up
        List<SnakePart> snake = new List<SnakePart>();
        SnakePart food;
        Timer gameLoop = new Timer();
        Timer snakeLoop = new Timer();
        float snakeRate = 5.5f;
        #endregion
        public FormGame()
        {
            InitializeComponent();
            gameLoop.Tick += new EventHandler(Update);
            snakeLoop.Tick += new EventHandler(UpdateSnake);
            gameLoop.Interval = 1000 / 120;
            snakeLoop.Interval = (int) (1000 / snakeRate);
            gameLoop.Start();
            snakeLoop.Start();
            StartGame();
        }
        #endregion
        #region Form Events
        private void FormGame_KeyDown(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, true);
        }

        private void FormGame_KeyUp(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, false);
        }
        private void Snake_Paint(object sender, PaintEventArgs e)
        {
            Draw(e.Graphics);
        }
        #endregion
        #region
        private void StartGame()
        {
            snakeRate = 5.5f;
            gameover = false;
            snakeLoop.Interval = (int)(1000 / snakeRate);
            snake.Clear();
            score = 0;
            SnakePart head = new SnakePart(15,10);
            snake.Add(head);
            GenerateFood();
        }

        private void Update(Object sender, EventArgs e)
        {
            if (gameover)
            {
                if (Input.Press(Keys.Enter))
                {
                    StartGame();
                }
            }
            else
            {
                if (Input.Press(Keys.Left))
                {
                    if (snake.Count < 2 || snake[0].x == snake[1].x)
                    {
                        direction = 1;
                    }
                }
                else if (Input.Press(Keys.Right))
                {
                    if (snake.Count < 2 || snake[0].x == snake[1].x)
                    {
                        direction = 2;
                    }
                }
                else if (Input.Press(Keys.Up))
                {
                    if (snake.Count < 2 || snake[0].y == snake[1].y)
                    {
                        direction = 3;
                    }
                }
                else if (Input.Press(Keys.Down))
                {
                    if (snake.Count < 2 || snake[0].y == snake[1].y)
                    {
                        direction = 0;
                    }
                }
            }
            SnakeCanvas.Invalidate();
        }

        private void UpdateSnake(Object sender, EventArgs e)
        {
            if (!gameover)
            {
                for (int i = snake.Count-1; i >= 0; i--)
                {
                    if (i == 0)
                    {
                        switch (direction)
                        {
                            case 0:
                                snake[0].y++;
                                break;
                            case 1:
                                snake[0].x--;
                                break;
                            case 2:
                                snake[0].x++;
                                break;
                            case 3:
                                snake[0].y--;
                                break;
                        }
                        SnakePart head = snake[0];
                        if (head.x >= 31 || head.x < 0 || head.y >=22 || head.y <0)
                        {
                            Gameover();
                        }
                        for (int j = 1; j < snake.Count; j++)
                        {
                            if (head.x == snake[j].x && head.y == snake[j].y)
                            {
                                Gameover();
                            }
                        }
                        if (head.x == food.x && head.y == food.y)
                        {
                            SnakePart part = new SnakePart(snake[snake.Count - 1].x, snake[snake.Count - 1].y);
                            snake.Add(part);
                            GenerateFood();
                            score++;
                            if (snakeRate < 15)
                            {
                                snakeRate += 0.7f;
                                snakeLoop.Interval = (int) (1000 / snakeRate);
                            }
                        }
                    }
                    else
                    {
                        snake[i].x = snake[i - 1].x;
                        snake[i].y = snake[i - 1].y;
                    }
                }
            }
        }

        private void Draw(Graphics canvas)
        {
            Font font = this.Font;
            if (gameover)
            {
                SizeF message = canvas.MeasureString("Game Over!", font);
                canvas.DrawString("Game Over!", font, Brushes.White, new PointF(248 - message.Width / 2, 120));
                message = canvas.MeasureString("Final Score: " + score.ToString(), font);
                canvas.DrawString("Final Score: " + score.ToString(), font, Brushes.White, new PointF(248 - message.Width / 2, 140));
                message = canvas.MeasureString("Press Enter to Start a New Game",font);
                canvas.DrawString("Press Enter to Start a New Game", font, Brushes.White, new PointF(248 - message.Width / 2, 160));
            }
            else
            {
                canvas.DrawString("Score: " + score.ToString(), font, Brushes.White, new PointF(4, 4));
                for(int i=0; i<snake.Count; i++)
                {
                    Color snake_color = i == 0 ? Color.Red : Color.LimeGreen;
                    SnakePart currentPart = snake[i];
                    canvas.FillEllipse(new SolidBrush(snake_color), new Rectangle(currentPart.x * 16, currentPart.y * 16, 16, 16));
                }
                canvas.FillEllipse(new SolidBrush(Color.Gold), new Rectangle(food.x * 16, food.y * 16, 16, 16));
            }
        }

        private void Gameover()
        {
            gameover = true;
        }

        private void GenerateFood()
        {
            Random random = new Random();
            food = new SnakePart(random.Next(0, 31), random.Next(0, 22));

        }
        #endregion
        public void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        public void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new About()).ShowDialog();
        }
    }
}
