using sea_battle.SeaBattle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sea_battle.patterns
{
    public static class FleetFactory
    {
        public static List<Ship> CreateFleet()
        {
            return new List<Ship>
            {
                new Ship(4),

                new Ship(3),
                new Ship(3),

                new Ship(2),
                new Ship(2),
                new Ship(2),

                new Ship(1),
                new Ship(1),
                new Ship(1),
                new Ship(1)
            };
        }
    }

    public interface IShotStrategy
    {
        Cell GetNextShot(Board board);
    }

    public class RandomShotStrategy : IShotStrategy
    {
        private readonly Random random;

        public RandomShotStrategy()
        {
            random = new Random();
        }

        public Cell GetNextShot(Board board)
        {
            List<Cell> availableCells = new List<Cell>();

            for (int row = 0; row < Board.Size; row++)
            {
                for (int column = 0; column < Board.Size; column++)
                {
                    Cell cell = board.Cells[row, column];

                    if (cell.State != CellState.Miss &&
                        cell.State != CellState.Hit)
                    {
                        availableCells.Add(cell);
                    }
                }
            }

            int index = random.Next(availableCells.Count);

            return availableCells[index];
        }
    }
}
