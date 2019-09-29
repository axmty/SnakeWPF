using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SnakeWPF
{
    public partial class MainWindow : Window
    {
        private static readonly int GridSquareSize = 20;
        private static readonly int GridWidth = 20;
        private static readonly int GridHeight = 20;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void DrawInitialArea()
        {
            var doneDrawing = false;
            var nextIsOdd = false;
            var nextSquareX = 0;
            var nextSquareY = 0;

            Area.Width = GridWidth * GridSquareSize;
            Area.Height = GridHeight * GridSquareSize;

            while (!doneDrawing)
            {
                var gridSquareColor = nextIsOdd ? Brushes.DarkGray : Brushes.LightGray;

                this.DrawGridSquare(nextSquareX, nextSquareY, gridSquareColor);
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

        private void DrawGridSquare(int squareX, int squareY, Brush color)
        {
            var square = new Rectangle
            {
                Width = GridSquareSize,
                Height = GridSquareSize,
                Fill = color
            };
            var squarePosX = squareX * GridSquareSize;
            var squarePosY = squareY * GridSquareSize;

            Area.Children.Add(square);
            Canvas.SetTop(square, squarePosY);
            Canvas.SetLeft(square, squarePosX);
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            this.DrawInitialArea();
        }
    }
}
