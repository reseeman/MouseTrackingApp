using System.Windows;
using System.Windows.Input;

namespace DbApp
{
    /// <summary>
    /// Логика взаимодействия для windowLogin.xaml
    /// </summary>
    public partial class WindowLogin : Window
    {
        string connectionString;

        string server, db, userID, password;

        private void btnLogin_MouseEnter(object sender, MouseEventArgs e)
        {
            if (tbxLogin.Text == "" || psbx.Password == "" || tbxServer.Text == "" || tbxDB.Text == "")
            {
                MessageBox.Show("Не все поля заполнены!");
            }
        }

        public WindowLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            server = tbxServer.Text;
            db = tbxDB.Text;
            userID = tbxLogin.Text;
            password = psbx.Password;

            connectionString = @"Data Source=.\" + server + ";Initial Catalog=" + db + ";User ID=" + userID + ";Password=" + password + ";";
            Odb.db = new System.Data.Entity.DbContext(connectionString);
            this.Close();
        }
    }
}
