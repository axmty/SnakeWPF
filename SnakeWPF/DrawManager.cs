using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SnakeWPF
{
    public class DrawManager
    {
        private static readonly int GridCellSize = 20;

        private static readonly Brush SnakeBodyColor = Brushes.Green;
        private static readonly Brush SnakeHeadColor = Brushes.DarkGreen;
        private static readonly Brush EvenCellColor = Brushes.LightGray;
        private static readonly Brush OddCellColor = Brushes.DarkGray;
        private static readonly Brush FoodColor = Brushes.Red;

        private readonly Canvas _gameGrid;

        public DrawManager(Canvas gameGrid)
        {
            _gameGrid = gameGrid;
        }

        public void DrawSnake(Snake snake)
        {
            foreach (var snakePart in snake)
            {
                var isHeadPart = snake.IsHead(snakePart);

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

        public void CleanSnakeShapes(Snake snake)
        {
            foreach (var part in snake)
            {
                if (part.Shape != null)
                {
                    this.RemoveShape(part.Shape);
                }
            }
        }

        public void DrawEmptyInitialGrid(int gridWidth, int gridHeight)
        {
            _gameGrid.Width = gridWidth * GridCellSize;
            _gameGrid.Height = gridHeight * GridCellSize;

            var doneDrawing = false;
            var nextIsOdd = false;
            var nextGridPosition = (x: 0, y: 0);

            while (!doneDrawing)
            {
                var gridSquareColor = nextIsOdd ? OddCellColor : EvenCellColor;

                this.DrawEmptyGridCell(nextGridPosition, gridSquareColor);
                nextIsOdd = !nextIsOdd;
                nextGridPosition.x = (nextGridPosition.x + 1) % gridWidth;
                if (nextGridPosition.x == 0)
                {
                    nextGridPosition.y++;
                    nextIsOdd = nextGridPosition.y % 2 == 1;
                }

                doneDrawing = nextGridPosition.y >= gridHeight;
            }
        }

        public void DrawFood(GridCell food)
        {
            food.Shape = new Ellipse
            {
                Width = GridCellSize,
                Height = GridCellSize,
                Fill = FoodColor
            };
            this.DrawShapeOnGrid(food.Shape, food.GridPosition);
        }

        public void RemoveShape(Shape shape)
        {
            _gameGrid.Children.Remove(shape);
        }

        private void DrawEmptyGridCell((int x, int y) gridPosition, Brush color)
        {
            var square = new Rectangle
            {
                Width = GridCellSize,
                Height = GridCellSize,
                Fill = color
            };
            this.DrawShapeOnGrid(square, gridPosition);
        }

        private void DrawShapeOnGrid(Shape shape, (int x, int y) gridPosition)
        {
            _gameGrid.Children.Add(shape);
            Canvas.SetLeft(shape, gridPosition.x * GridCellSize);
            Canvas.SetTop(shape, gridPosition.y * GridCellSize);
        }
    }
}
