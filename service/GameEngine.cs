using sea_battle.patterns;
using sea_battle.SeaBattle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sea_battle.service
{
    public class GameEngine
    {
        public Board PlayerBoard { get; private set; }
        public Board ComputerBoard { get; private set; }
        public bool IsGameOver { get; private set; }


        private readonly Random random;
        private readonly IShotStrategy computerStrategy;
        public event Action BoardChanged;
        public event Action<string> StatusChanged;


        public GameEngine()
        {
            random = new Random();

            computerStrategy = new RandomShotStrategy();
        }


        public void NewGame()
        {
            PlayerBoard = new Board();
            ComputerBoard = new Board();
            IsGameOver = false;

            PlaceFleet(PlayerBoard);
            PlaceFleet(ComputerBoard);


            SendStatus("Игра началась. Стреляйте по полю компьютера.");
            NotifyBoardChanged();
        }


        private void PlaceFleet(Board board)
        {
            List<Ship> ships = FleetFactory.CreateFleet();

            foreach (Ship ship in ships)
            {
                bool placed = false;

                while (!placed)
                {
                    int row = random.Next(Board.Size);
                    int column = random.Next(Board.Size);
                    bool horizontal =random.Next(2) == 0;


                    placed = board.PlaceShip(
                        ship,
                        row,
                        column,
                        horizontal);
                }
            }
        }


        public void PlayerShoot(int row, int column)
        {
            if (IsGameOver)
            {
                return;
            }


            ShotResult playerResult = ComputerBoard.ReceiveShot(row, column);

            if (playerResult == ShotResult.AlreadyShot)
            {
                SendStatus("Вы уже стреляли в эту клетку.");
                return;
            }


            string playerText = ResultToText(playerResult);


            if (ComputerBoard.AllShipsSunk())
            {
                IsGameOver = true;

                SendStatus("Вы победили! Все корабли компьютера уничтожены.");
                NotifyBoardChanged();

                return;
            }

            Cell target = computerStrategy.GetNextShot(PlayerBoard);


            ShotResult computerResult = PlayerBoard.ReceiveShot(
                    target.Row,
                    target.Column
             );


            string computerText = ResultToText(computerResult);

            if (PlayerBoard.AllShipsSunk())
            {
                IsGameOver = true;

                SendStatus("Компьютер победил.");
                NotifyBoardChanged();

                return;
            }


            SendStatus(
                "Вы: " + playerText +
                " | Компьютер: " + computerText
             );


            NotifyBoardChanged();
        }


        private string ResultToText(ShotResult result)
        {
            switch (result)
            {
                case ShotResult.Miss:
                    return "промах";

                case ShotResult.Hit:
                    return "попадание";

                case ShotResult.Sunk:
                    return "корабль уничтожен";

                default:
                    return "";
            }
        }

        private void NotifyBoardChanged()
        {
            if (BoardChanged != null)
            {
                BoardChanged();
            }
        }


        private void SendStatus(string text)
        {
            if (StatusChanged != null)
            {
                StatusChanged(text);
            }
        }
    }
}
