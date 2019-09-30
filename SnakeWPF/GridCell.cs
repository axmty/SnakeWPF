using System.Windows.Shapes;

namespace SnakeWPF
{
    public class GridCell
    {
        public Shape Shape { get; set; }

        public (int x, int y) Position { get; set; }
    }
}
