using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SnakeWPF
{
    public class Snake : IEnumerable<SnakePart>
    {
        private static readonly int SnakeStartLength = 3;
        private static readonly int SnakeStartSpeed = 400;
        private static readonly int SnakeSpeedThreshold = 100;

        private readonly List<SnakePart> _parts = new List<SnakePart>();

        public Snake((int x, int y) initialGridPosition)
        {
            this.AddPart(initialGridPosition);
        }

        public SnakePart End => _parts.First();

        public SnakePart Head => _parts.Last();

        public int Length { get; private set; } = SnakeStartLength;

        public int Speed { get; private set; } = SnakeStartSpeed;

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

        public bool IsTailExceeding()
        {
            return _parts.Count >= this.Length;
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
            _parts.Add(new SnakePart
            {
                GridPosition = gridPosition
            });
        }
    }
}
