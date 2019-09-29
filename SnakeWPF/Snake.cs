using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SnakeWPF
{
    public class Snake : IEnumerable<SnakePart>
    {
        private readonly List<SnakePart> _parts = new List<SnakePart>();

        public SnakePart End => _parts.First();

        public SnakePart Head => _parts.Last();

        public int Length => _parts.Count;

        public Direction Direction { get; set; } = Direction.Right;

        public IEnumerator<SnakePart> GetEnumerator()
        {
            foreach (var part in _parts)
            {
                yield return part;
            }
        }

        public bool IsHead(SnakePart part)
        {
            return this.Head == part;
        }

        public void RemoveTailEnd()
        {
            _parts.RemoveAt(0);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void AddNewHead()
        {
            var newHeadGridPosition = (x: 0, y: 0);

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

            _parts.Add(new SnakePart
            {
                GridPosition = newHeadGridPosition
            });
        }
    }
}
