using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sea_battle
{
    namespace SeaBattle
    {
        public enum CellState
        {
            Empty,
            Ship,
            Miss,
            Hit
        }

        public enum ShotResult
        {
            AlreadyShot,
            Miss,
            Hit,
            Sunk
        }

        public class Cell
        {
            public int Row { get; set; }
            public int Column { get; set; }

            public CellState State { get; set; }

            public Ship Ship { get; set; }

            public Cell(int row, int column)
            {
                Row = row;
                Column = column;
                State = CellState.Empty;
            }
        }

        public class Ship
        {
            public int Size { get; private set; }

            public int Hits { get; private set; }

            public bool IsSunk
            {
                get
                {
                    return Hits >= Size;
                }
            }

            public Ship(int size)
            {
                Size = size;
                Hits = 0;
            }

            public void Hit()
            {
                Hits++;
            }
        }

        public class Board
        {
            public const int Size = 10;

            public Cell[,] Cells { get; private set; }

            private readonly List<Ship> ships;

            public Board()
            {
                Cells = new Cell[Size, Size];
                ships = new List<Ship>();

                for (int row = 0; row < Size; row++)
                {
                    for (int column = 0; column < Size; column++)
                    {
                        Cells[row, column] = new Cell(row, column);
                    }
                }
            }

            public bool PlaceShip(
                Ship ship,
                int row,
                int column,
                bool horizontal)
            {
                if (horizontal)
                {
                    if (column + ship.Size > Size)
                    {
                        return false;
                    }
                }
                else
                {
                    if (row + ship.Size > Size)
                    {
                        return false;
                    }
                }

                for (int i = 0; i < ship.Size; i++)
                {
                    int currentRow = horizontal ? row : row + i;
                    int currentColumn = horizontal ? column + i : column;

                    if (Cells[currentRow, currentColumn].State != CellState.Empty)
                    {
                        return false;
                    }
                }

                for (int i = 0; i < ship.Size; i++)
                {
                    int currentRow = horizontal ? row : row + i;
                    int currentColumn = horizontal ? column + i : column;

                    Cells[currentRow, currentColumn].State = CellState.Ship;
                    Cells[currentRow, currentColumn].Ship = ship;
                }

                ships.Add(ship);

                return true;
            }

            public ShotResult ReceiveShot(int row, int column)
            {
                Cell cell = Cells[row, column];

                if (cell.State == CellState.Miss ||
                    cell.State == CellState.Hit)
                {
                    return ShotResult.AlreadyShot;
                }

                if (cell.State == CellState.Ship)
                {
                    cell.State = CellState.Hit;

                    cell.Ship.Hit();

                    if (cell.Ship.IsSunk)
                    {
                        return ShotResult.Sunk;
                    }

                    return ShotResult.Hit;
                }

                cell.State = CellState.Miss;

                return ShotResult.Miss;
            }

            public bool AllShipsSunk()
            {
                foreach (Ship ship in ships)
                {
                    if (!ship.IsSunk)
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}
