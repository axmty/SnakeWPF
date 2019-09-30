using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SnakeWPF
{
    public class Snake : IEnumerable<GridCell>
    {
        private static readonly int SnakeStartLength = 3;
        private static readonly int SnakeStartSpeed = 400;
        private static readonly int SnakeSpeedThreshold = 100;

        private readonly List<GridCell> _parts = new List<GridCell>();

        private Direction _direction = Direction.Right;

        public Snake((int x, int y) initialGridPosition)
        {
            this.AddPart(initialGridPosition);
        }

        public GridCell End => _parts.First();

        public GridCell Head => _parts.Last();

        public int Length { get; private set; } = SnakeStartLength;

        public int Speed { get; private set; } = SnakeStartSpeed;

        public Direction Direction
        {
            get
            {
                return _direction;
            }

            set
            {
                if ((value == Direction.Left && _direction != Direction.Right) ||
                    (value == Direction.Right && _direction != Direction.Left) ||
                    (value == Direction.Up && _direction != Direction.Down) ||
                    (value == Direction.Down && _direction != Direction.Up))
                {
                    _direction = value;
                }
            }
        }

        public bool IsTailExceeding()
        {
            return _parts.Count >= this.Length;
        }

        public void RemoveTailEnd()
        {
            _parts.RemoveAt(0);
        }

        public bool IsHead(GridCell part)
        {
            return this.Head == part;
        }

        public void AddNewHead()
        {
            var newHeadGridPosition = this.Head.GridPosition;

            switch (this.Direction)
            {
                case Direction.Left:
                    newHeadGridPosition.x--;
                    break;
                case Direction.Right:
                    newHeadGridPosition.x++;
                    break;
                case Direction.Up:
                    newHeadGridPosition.y--;
                    break;
                case Direction.Down:
                    newHeadGridPosition.y++;
                    break;
            }

            this.AddPart(newHeadGridPosition);
        }

        private void AddPart((int x, int y) gridPosition)
        {
            _parts.Add(new GridCell
            {
                GridPosition = gridPosition
            });
        }

        public IEnumerator<GridCell> GetEnumerator()
        {
            foreach (var part in _parts)
            {
                yield return part;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
