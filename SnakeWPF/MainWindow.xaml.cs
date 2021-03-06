﻿using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace SnakeWPF
{
    public partial class MainWindow : Window
    {
        private static readonly int GridWidth = 20;
        private static readonly int GridHeight = 20;
        private static readonly (int, int) SnakeInitialGridPosition = (5, 5);

        private readonly DrawManager _drawManager;
        private readonly DispatcherTimer _gameTimer = new DispatcherTimer();
        
        private Snake _snake;
        private GridCell _food;
        private int _score = 0;

        public MainWindow()
        {
            this.InitializeComponent();
            _gameTimer.Tick += this.GameTimer_Tick;
            _drawManager = new DrawManager(GameGrid);
            this.HighScoreList.Load();
        }

        public HighScoreList HighScoreList { get; } = new HighScoreList();

        private void SyncTimerToSnakeSpeed()
        {
            _gameTimer.Interval = TimeSpan.FromMilliseconds(_snake.Speed);
        }

        private void Clean()
        {
            if (_snake != null)
            {
                _drawManager.CleanSnakeShapes(_snake);
            }

            if (_food != null)
            {
                _drawManager.RemoveShape(_food.Shape);
            }

            _score = 0;
        }

        private void HideBorderControls()
        {
            BorderWelcomeMessage.Visibility = Visibility.Collapsed;
            BorderHighScoreList.Visibility = Visibility.Collapsed;
            BorderEndOfGame.Visibility = Visibility.Collapsed;
        }

        private void StartNewGame()
        {
            this.Clean();
            this.HideBorderControls();

            _snake = new Snake(SnakeInitialGridPosition);
            _snake.AddNewHead();
            _drawManager.DrawSnake(_snake);

            this.CreateNewFood();

            this.UpdateGameStatus();

            this.SyncTimerToSnakeSpeed();
            _gameTimer.Start();
        }

        private void MoveSnake()
        {
            while (_snake.IsTailExceeding())
            {
                _drawManager.RemoveShape(_snake.End.Shape);
                _snake.RemoveTailEnd();
            }

            _snake.AddNewHead();
            _drawManager.DrawSnake(_snake);
            this.CheckCollision();
        }

        private void CreateNewFood()
        {
            if (_food != null)
            {
                _drawManager.RemoveShape(_food.Shape);
            }

            _food = FoodManager.CreateRandomFood(GridWidth, GridHeight, _snake);
            _drawManager.DrawFood(_food);
        }

        private void CheckCollision()
        {
            var snakeHeadPosition = _snake.Head.Position;

            if (snakeHeadPosition == _food.Position)
            {
                this.EatFood();
            }
            else if (this.IsOutsideGrid(snakeHeadPosition) || _snake.HasBodyCollision())
            {
                this.EndGame();
            }
        }

        private void EndGame()
        {
            if (_score > 0 && this.HighScoreList.CanAdd(_score))
            {
                BorderNewHighScore.Visibility = Visibility.Visible;
                TextBoxPlayerName.Focus();
            }
            else
            {
                TextBlockFinalScore.Text = _score.ToString();
                BorderEndOfGame.Visibility = Visibility.Visible;
            }

            _gameTimer.Stop();
        }

        private bool IsOutsideGrid((int x, int y) position)
        {
            return position.x < 0
                || position.x >= GridWidth
                || position.y < 0
                || position.y >= GridHeight;
        }

        private void UpdateGameStatus()
        {
            TextBlockStatusScore.Text = _score.ToString();
            TextBlockStatusSpeed.Text = _snake.Speed.ToString();
        }

        private void EatFood()
        {
            _snake.Grow();
            _score++;
            this.SyncTimerToSnakeSpeed();
            this.CreateNewFood();
            this.UpdateGameStatus();
        } 

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            this.MoveSnake();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            _drawManager.DrawEmptyInitialGrid(GridWidth, GridHeight);
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    _snake.Direction = Direction.Up;
                    break;
                case Key.Down:
                    _snake.Direction = Direction.Down;
                    break;
                case Key.Left:
                    _snake.Direction = Direction.Left;
                    break;
                case Key.Right:
                    _snake.Direction = Direction.Right;
                    break;
                case Key.Space:
                    this.StartNewGame();
                    break;
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonAddToHighScoreList_Click(object sender, RoutedEventArgs e)
        {
            this.HighScoreList.Add(TextBoxPlayerName.Text, _score);
            this.HighScoreList.Save();

            BorderNewHighScore.Visibility = Visibility.Collapsed;
            BorderHighScoreList.Visibility = Visibility.Visible;
        }

        private void ButtonShowHighScoreList_Click(object sender, RoutedEventArgs e)
        {
            BorderWelcomeMessage.Visibility = Visibility.Collapsed;
            BorderHighScoreList.Visibility = Visibility.Visible;
        }
    }
}
