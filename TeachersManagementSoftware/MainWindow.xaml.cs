using System.Windows;
using System.Data.SqlClient;
using System;
using System.Windows.Media.Imaging;

namespace TeachersManagementSoftware
{
    public partial class MainWindow : Window
    {
        //Edit this with your own Data Source in both forms
        String conString = "Data Source =.\\SQLSERVER; Initial Catalog = TeachersManagementDB; Integrated Security = True";
        public MainWindow()
        {
            InitializeComponent();
            lbl_check.Content = "";
            id.Focus();
            checkConnection();
            }
        private void btn_login_Click(object sender, RoutedEventArgs e)
        { adminLogin(); }
        private void adminLogin()
        {
            try
            {
                if (IsServerConnected(conString))
                {
                    SqlConnection con = new SqlConnection(conString);
                    SqlCommand scmd = new SqlCommand("select count(*) from Admins_Data where Name =@usr AND Password=@pwd", con);
                    scmd.Parameters.Clear();
                    scmd.Parameters.AddWithValue("@usr", id.Text);
                    scmd.Parameters.AddWithValue("@pwd", pw.Password.ToString());
                    con.Open();
                    if (scmd.ExecuteScalar().ToString() == "1")
                    {
                        form_addRemoveTeachers a = new form_addRemoveTeachers();
                        a.Show();
                        this.Close();
                    }
                    else
                    { lbl_check.Content = "UserName or Password is Incorrect"; pw.Password = ""; id.Focus(); }
                }
                else
                    MessageBox.Show("Connection is not established!");
            }
            catch (Exception ee)
            { MessageBox.Show(ee.ToString()); }
        }
        private void Pw_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            lbl_check.Content = "";
            if (e.Key == System.Windows.Input.Key.Enter) adminLogin();
        }
        private static bool IsServerConnected(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                { connection.Open(); return true; }
                catch (SqlException)
                { return false; }
            }
        }
        public void checkConnection()
        {
            if (IsServerConnected(conString))
            { img_connection.Source = new BitmapImage(new Uri("assests/connected.png", UriKind.Relative)); lbl_connection.Content = "Connected"; }
            else
            { img_connection.Source = new BitmapImage(new Uri("assests/disconnected.png", UriKind.Relative)); lbl_connection.Content = "Disconnected"; }
        }
        private void btn_close_onClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Quit", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                Application.Current.Shutdown();
        }
        private void btn_minimize_onClick(object sender, RoutedEventArgs e)
        { this.WindowState = WindowState.Minimized; }
    }
}
