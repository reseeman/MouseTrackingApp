using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Configuration;
using System;
using System.Windows.Input;

namespace DbApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isTracking;
        Point startMousePosition, finishMousePosition;
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        SqlConnection conn;

        public MainWindow()
        {
            InitializeComponent();

            isTracking = false;
            conn = new SqlConnection(connectionString);
            conn.Open();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowLogin windowLogin = new WindowLogin();
            windowLogin.ShowDialog();
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            isTracking = !isTracking;
            if (isTracking)
            {
                startMousePosition = Mouse.GetPosition(Application.Current.MainWindow);
                startButton.Content = "Финиш";
                
                mouseGrid.ItemsSource = Odb.db.Database.SqlQuery<mouse>("SELECT * FROM mouseTable", new SqlParameter("@param1", "1")).ToList();
            }
            else if (!isTracking)
            {
                finishMousePosition = Mouse.GetPosition(Application.Current.MainWindow);
                startButton.Content = "Старт";

                mouseGrid.ItemsSource = Odb.db.Database.SqlQuery<mouse>("SELECT * FROM mouseTable", new SqlParameter("@param1", "1")).ToList();
            }
        }

        Point _lastPoint = new Point();
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (isTracking)
                {
                    Point position = Mouse.GetPosition(Application.Current.MainWindow);
                    Vector distance = Point.Subtract(_lastPoint, position);

                    if (distance.Length >= 5)
                    {
                        _lastPoint = position;

                        using (SqlCommand cmd = new SqlCommand("INSERT INTO mouseTable (startPosition, currentPosition, finishPosition) VALUES (@startPos, @curPos, @finishPos)"))
                        {
                            cmd.Parameters.AddWithValue("@startPos", startMousePosition.ToString());
                            cmd.Parameters.AddWithValue("@curPos", position.ToString());
                            cmd.Parameters.AddWithValue("@finishPos", finishMousePosition.ToString());
                            cmd.Connection = conn;
                            cmd.ExecuteNonQuery();
                        }
                        mouseGrid.ItemsSource = Odb.db.Database.SqlQuery<mouse>("SELECT * FROM mouseTable", new SqlParameter("@param1", "1")).ToList();
                    }
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " Неверный логин или пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);

                WindowLogin windowLogin = new WindowLogin();
                windowLogin.ShowDialog();
            }
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            conn.Close();
        }

        public class mouse
        {
            public int idMouse { get; set; }
            public string startPosition { get; set; }
            public string currentPosition { get; set; }
            public string finishPosition { get; set; }
        }
    }
}
