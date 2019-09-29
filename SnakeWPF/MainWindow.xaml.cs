using System;
using System.Collections.Generic;
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

        private readonly List<SnakePart> _snake = new List<SnakePart>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void DrawShapeOnArea(Shape shape, double x, double y)
        {
            Area.Children.Add(shape);
            Canvas.SetLeft(shape, x);
            Canvas.SetTop(shape, y);
        }

        private void DrawSnakePart(SnakePart snakePart, bool isHead)
        {
            snakePart.Shape = new Rectangle
            {
                Width = GridCellSize,
                Height = GridCellSize,
                Fill = isHead ? SnakeHeadColor : SnakeBodyColor
            };
            this.DrawShapeOnArea(snakePart.Shape, snakePart.Position.X, snakePart.Position.Y);
        }

        private void DrawSnake()
        {
            for (int i = 0; i < _snake.Count; i++)
            {
                var snakePart = _snake[i];
                var isHead = i == _snake.Count - 1;
                
                if (snakePart.Shape == null)
                {
                    this.DrawSnakePart(snakePart, isHead);
                }
            }
        }

        private void DrawInitialArea()
        {
            var doneDrawing = false;
            var nextIsOdd = false;
            var nextSquareX = 0;
            var nextSquareY = 0;

            Area.Width = GridWidth * GridCellSize;
            Area.Height = GridHeight * GridCellSize;

            while (!doneDrawing)
            {
                var gridSquareColor = nextIsOdd ? Brushes.DarkGray : Brushes.LightGray;

                this.DrawGridCell(nextSquareX, nextSquareY, gridSquareColor);
                nextIsOdd = !nextIsOdd;
                nextSquareX = (nextSquareX + 1) % GridWidth;
                if (nextSquareX == 0)
                {
                    nextSquareY++;
                    nextIsOdd = nextSquareY % 2 == 1;
                }

                doneDrawing = nextSquareY >= GridHeight;
            }
        }

        private void DrawGridCell(int squareX, int squareY, Brush color)
        {
            var square = new Rectangle
            {
                Width = GridCellSize,
                Height = GridCellSize,
                Fill = color
            };
            var squarePosX = squareX * GridCellSize;
            var squarePosY = squareY * GridCellSize;

            this.DrawShapeOnArea(square, squarePosX, squarePosY);
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            this.DrawInitialArea();
        }
    }
}
