using System;
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

        public MainWindow()
        {
            InitializeComponent();
            _gameTimer.Tick += this.GameTimer_Tick;
            _drawManager = new DrawManager(GameGrid);
        }

        private void SyncTimerToSnakeSpeed()
        {
            _gameTimer.Interval = TimeSpan.FromMilliseconds(_snake.Speed);
        }

        private void StartNewGame()
        {
            _snake = new Snake(SnakeInitialGridPosition);
            _snake.AddNewHead();
            _drawManager.DrawSnake(_snake);
            this.CreateNewFood();
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

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            this.MoveSnake();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            _drawManager.DrawEmptyInitialGrid(GridWidth, GridHeight);
            this.StartNewGame();
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
            }
        }
    }
}
