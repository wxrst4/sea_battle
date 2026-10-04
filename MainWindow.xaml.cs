using sea_battle.SeaBattle;
using sea_battle.service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace sea_battle
{
    public partial class MainWindow : Window
    {
        private GameEngine game;


        public MainWindow()
        {
            InitializeComponent();

            game = new GameEngine();
            game.BoardChanged += DrawBoards;
            game.StatusChanged += ChangeStatus;

            game.NewGame();
        }


        private void NewGame_Click(
            object sender,
            RoutedEventArgs e)
        {
            game.NewGame();
        }


        private void ChangeStatus(string text)
        {
            StatusText.Text = text;
        }

        private void DrawBoards()
        {
            PlayerGrid.Children.Clear();
            ComputerGrid.Children.Clear();


            for (int row = 0; row < Board.Size; row++)
            {
                for (int column = 0;
                     column < Board.Size;
                     column++)
                {
                    DrawPlayerCell(row, column);
                    DrawComputerCell(row, column);
                }
            }
        }

        private void DrawPlayerCell(
            int row,
            int column
            )
        {
            Cell cell = game.PlayerBoard.Cells[row, column];


            Button button = CreateButton();

            if (cell.State == CellState.Ship)
            {
                button.Background = Brushes.LightSkyBlue;
                button.Content = "■";
            }
            else if (cell.State == CellState.Hit)
            {
                button.Background = Brushes.IndianRed;
                button.Content = "X";
            }
            else if (cell.State == CellState.Miss)
            {
                button.Background = Brushes.LightGray;
                button.Content = "•";
            }
            else
            {
                button.Background = Brushes.White;
            }

            button.IsHitTestVisible = false;
            PlayerGrid.Children.Add(button);
        }

        private void DrawComputerCell(
            int row,
            int column
         )
        {
            Cell cell = game.ComputerBoard.Cells[row, column];

            Button button = CreateButton();
            button.Tag = new int[] { row, column };
            button.Click += EnemyCell_Click;


            if (cell.State == CellState.Hit)
            {
                button.Background = Brushes.IndianRed;
                button.Content = "X";
            }
            else if (cell.State == CellState.Miss)
            {
                button.Background = Brushes.LightGray;
                button.Content = "•";
            }
            else
            {
                button.Background = Brushes.White;
            }

            if (game.IsGameOver &&
                cell.State == CellState.Ship)
            {
                button.Background = Brushes.LightPink;
                button.Content = "■";
            }

            ComputerGrid.Children.Add(button);
        }


        private void EnemyCell_Click(
            object sender,
            RoutedEventArgs e
            )
        {
            if (game.IsGameOver)
            {
                return;
            }

            Button button = sender as Button;


            if (button == null)
            {
                return;
            }


            int[] position = button.Tag as int[];


            if (position == null)
            {
                return;
            }

            int row = position[0];
            int column = position[1];
            game.PlayerShoot(
                row,
                column
                );
        }

        private Button CreateButton()
        {
            Button button = new Button();

            button.Margin = new Thickness(1);
            button.BorderBrush = Brushes.Gray;
            button.BorderThickness = new Thickness(1);
            button.FontSize = 16;
            button.FontWeight = FontWeights.Bold;
            button.Padding = new Thickness(0);

            return button;
        }
    }
}
