using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SnakeWPF
{
    public partial class MainWindow : Window
    {
        private static readonly int GridCellSize = 20;
        private static readonly int GridWidth = 20;
        private static readonly int GridHeight = 20;
        private static readonly Brush SnakeBodyColor = Brushes.Green;
        private static readonly Brush SnakeHeadColor = Brushes.DarkGreen;

        private readonly Snake _snake = new Snake();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MoveSnake()
        {
            while (_snake.Length >= _snake.Length)
            {
                GameGrid.Children.Remove(_snake.End.Shape);
                _snake.RemoveTailEnd();
            }

            _snake.AddNewHead();
            this.DrawSnake();
        }

        private void DrawShapeOnGrid(Shape shape, (int x, int y) gridPosition)
        {
            GameGrid.Children.Add(shape);
            Canvas.SetLeft(shape, gridPosition.x * GridCellSize);
            Canvas.SetTop(shape, gridPosition.y * GridCellSize);
        }

        private void DrawSnake()
        {
            foreach (var snakePart in _snake)
            {
                var isHeadPart = _snake.IsHead(snakePart);

                if (snakePart.Shape == null)
                {
                    snakePart.Shape = new Rectangle
                    {
                        Width = GridCellSize,
                        Height = GridCellSize
                    };
                    this.DrawShapeOnGrid(snakePart.Shape, snakePart.GridPosition);
                }

                snakePart.Shape.Fill = isHeadPart ? SnakeHeadColor : SnakeBodyColor;
            }
        }

        private void DrawInitialGrid()
        {
            GameGrid.Width = GridWidth * GridCellSize;
            GameGrid.Height = GridHeight * GridCellSize;

            var doneDrawing = false;
            var nextIsOdd = false;
            var nextGridPosition = (x: 0, y: 0);

            while (!doneDrawing)
            {
                var gridSquareColor = nextIsOdd ? Brushes.DarkGray : Brushes.LightGray;

                this.DrawGridCell(nextGridPosition, gridSquareColor);
                nextIsOdd = !nextIsOdd;
                nextGridPosition.x = (nextGridPosition.x + 1) % GridWidth;
                if (nextGridPosition.y == 0)
                {
                    nextGridPosition.y++;
                    nextIsOdd = nextGridPosition.y % 2 == 1;
                }

                doneDrawing = nextGridPosition.y >= GridHeight;
            }
        }

        private void DrawGridCell((int x, int y) gridPosition, Brush color)
        {
            var square = new Rectangle
            {
                Width = GridCellSize,
                Height = GridCellSize,
                Fill = color
            };
            this.DrawShapeOnGrid(square, gridPosition);
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            this.DrawInitialGrid();
        }
    }
}
