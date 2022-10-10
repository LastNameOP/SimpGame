using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int size = 10;

        static Random rand = new Random();
        Ellipse playerObj;
        int collectedFood = 0;
        int totalFood = 0;
        Rectangle[,] dieTiles;
        Ellipse[,] foodsTiles;
        bool isEnd = false;
        List<Ellipse> foodList = new List<Ellipse>();
        bool extraExist = true;
        int steps = 0;
        public MainWindow()
        {
            InitializeComponent();
            GenerateAll();
        }


        private void GenerateTitles()
        {
            for (int i = 1; i < size; i++)
            {
                for (int j = 1; j < size; j++)
                {
                    if (rand.Next(100) > 70)
                    {
                        Rectangle blackTitle = new Rectangle();
                        blackTitle.Width = 80;
                        blackTitle.Height = 80;
                        blackTitle.Fill = Brushes.PeachPuff;
                        dieTiles[i, j] = blackTitle;
                        Greeded.Children.Add(blackTitle);
                        Grid.SetRow(blackTitle, i);
                        Grid.SetColumn(blackTitle, j);
                    }
                }
            }
        }
        private void GenerateFood()
        {
            for (int i = 1; i < size; i++)
            {
                for (int j = 1; j < size; j++)
                {
                    if (dieTiles[i, j] == null && rand.Next(100) > 85)
                    {
                        Ellipse food = new Ellipse();
                        food.Width = 60;
                        food.Height = 60;
                        food.Fill = Brushes.DeepPink;
                        Greeded.Children.Add(food);
                        Grid.SetColumn(food, j);
                        Grid.SetRow(food, i);
                        totalFood++;
                        foodList.Add(food);
                        CurFood.Text = $"{collectedFood}/{totalFood}";
                        foodsTiles[i, j] = food;
                    }
                }
            }
        }

        private void GenerateAll()
        {
            if(playerObj == null)
            playerObj = Player;
            dieTiles = new Rectangle[size, size];
            foodsTiles = new Ellipse[size, size];
            GenerateTitles();
            GenerateFood();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (isEnd) Restart();
            else
                switch (e.Key)
                {
                    case Key.A:
                        if (Grid.GetColumn(Player) - 1 >= 0)
                            Grid.SetColumn(Player, Grid.GetColumn(Player) - 1);
                        break;
                    case Key.D:
                        if (Grid.GetColumn(Player) + 1 < size)
                            Grid.SetColumn(Player, Grid.GetColumn(Player) + 1);
                        break;
                    case Key.W:
                        if (Grid.GetRow(Player) - 1 >= 0)
                            Grid.SetRow(Player, Grid.GetRow(Player) - 1);
                        break;
                    case Key.S:
                        if (Grid.GetRow(Player) + 1 < size)
                            Grid.SetRow(Player, Grid.GetRow(Player) + 1);
                        break;
                    case Key.R:
                        Restart();
                        steps--;
                        break;

                }
            steps++;
            StepsText.Text = "Steps: " + steps.ToString();
            CollectFood();
            if (dieTiles[Grid.GetRow(Player), Grid.GetColumn(Player)] != null)
            {
                if (!extraExist)
                    Die();
                else
                {
                    Greeded.Children.Remove(dieTiles[Grid.GetRow(Player), Grid.GetColumn(Player)]);
                    dieTiles[Grid.GetRow(Player), Grid.GetColumn(Player)] = null;
                    extraExist = false;
                    ExtraText.Text = "Extra: 0/1";
                }
            }      
            if (collectedFood == totalFood)
                Win();

        }
        private void CollectFood()
        {
            if (foodsTiles[Grid.GetRow(Player), Grid.GetColumn(Player)] != null)
            {
                collectedFood++;
                Greeded.Children.Remove(foodsTiles[Grid.GetRow(Player), Grid.GetColumn(Player)]);
                foodsTiles[Grid.GetRow(Player), Grid.GetColumn(Player)] = null;
                CurFood.Text = $"{collectedFood}/{totalFood}";
            }
        }
        private void Die()
        {
            Meth();
            LoseText.Visibility = Visibility.Visible;
        }
        private void Win()
        {
            Meth();
            WinText.Visibility = Visibility.Visible;
        }
        private void Meth()
        {
            Greeded.Visibility = Visibility.Hidden;
            isEnd = true;
            CurFood.Visibility = Visibility.Hidden;
        }
        private void Restart()
        {
            steps = 0;
            StepsText.Text = "Steps: 0";
            ExtraText.Text = "Extra: 1/1";
            extraExist = true;
            CurFood.Visibility = Visibility.Visible;
            if (WinText.Visibility == Visibility.Visible) WinText.Visibility = Visibility.Hidden;
            else LoseText.Visibility = Visibility.Hidden;
            Greeded.Children.Clear();
            Greeded.Visibility = Visibility.Visible;
            Array.Clear(dieTiles,0,size);
            Array.Clear(foodsTiles,0,size);
            collectedFood = 0;
            totalFood = 0;
            isEnd = false;
            Greeded.Children.Add(playerObj);
            Grid.SetColumn(Player, 0);
            Grid.SetRow(Player, 0);
            GenerateAll();
            ChangeSizeOfDieTiles();
        }

        private void ChangeSizeOfDieTiles()
        {
            foreach (Rectangle tile in dieTiles)
            {
                if (tile != null)
                {
                    tile.Width = Greeded.Width / size;
                    tile.Height = Greeded.Height / size;
                }
            }
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ChangeSizeOfDieTiles();
        }

        private void ExtraText_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Вы можете уничитожить одну непроходимую ячейку", "Подсказка");
        }
    }
}
