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
using System.Data.SQLite;

namespace Karobar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string sha256(string password)
        {
            System.Security.Cryptography.SHA256Managed crypt = new System.Security.Cryptography.SHA256Managed();
            System.Text.StringBuilder hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(password), 0, Encoding.UTF8.GetByteCount(password));
            foreach (byte bit in crypto)
            {
                hash.Append(bit.ToString("x2"));
            }
            return hash.ToString();
        }

        public MainWindow()
        {
            InitializeComponent();
            if (Properties.Settings.Default["logged"].ToString() == "1")
            {
                this.Hide();
                Dashboard second = new Dashboard();
                second.Show();
                this.Close();
            }

        }

        private void _login_Click(object sender, RoutedEventArgs e)
        {
            string dbConnectionString = @"Data Source=database.db;Version=3";
            
            try
            {
                SQLiteConnection conn = new SQLiteConnection(dbConnectionString);
                conn.Open();
                string querry = "select * from user where name = '" + this._username.Text + "' and password = '" + sha256(this._password.Password) + "';";
                SQLiteCommand command = new SQLiteCommand(querry, conn);
                command.ExecuteNonQuery();
                SQLiteDataReader reader = command.ExecuteReader();
                int count = 0;
                while (reader.Read())
                {
                    count++;
                }
                if (count == 1)
                {
                    if (_remember_me.IsChecked == true)
                    {
                        Properties.Settings.Default["logged"] = "1";
                        Properties.Settings.Default.Save();
                    }
                    this.Hide();
                    Dashboard second = new Dashboard();
                    second.Show();
                    this.Close();
                }
                else
                    MessageBox.Show("Login Failed " + count);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //MessageBox.Show(Properties.Settings.Default["logged"].ToString());
            //Properties.Settings.Default["logged"] = "Some Value";
            //Properties.Settings.Default.Save();
        }
    }
}
