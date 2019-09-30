using System;

namespace SnakeWPF
{
    public static class FoodManager
    {
        private static readonly Random _random = new Random();

        public static GridCell CreateRandomFood(int gridWidth, int gridHeight, Snake snake)
        {
            int foodX = _random.Next(0, gridWidth);
            int foodY = _random.Next(0, gridHeight);

            foreach (var snakePart in snake)
            {
                if (snakePart.GridPosition.x == foodX && snakePart.GridPosition.y == foodY)
                {
                    return CreateRandomFood(gridWidth, gridHeight, snake);
                }
            }

            return new GridCell
            {
                GridPosition = (foodX, foodY)
            };
        }
    }
}
