using System.Windows.Shapes;

namespace SnakeWPF
{
    public class SnakePart
    {
        public Shape Shape { get; set; }

        public (int x, int y) GridPosition { get; set; }
    }
}
