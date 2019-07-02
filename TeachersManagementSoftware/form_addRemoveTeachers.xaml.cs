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
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.IO;

namespace TeachersManagementSoftware

{
    public partial class form_addRemoveTeachers : Window
    {
        //Edit this with your own Data Source in both forms
        SqlConnection con = new SqlConnection(@"Data Source=.\SQLSERVER;Initial Catalog=TeachersManagementDB;Integrated Security=True");
        public form_addRemoveTeachers()
        {
            InitializeComponent();
            goToStartState();
            try
            {
                MainWindow a = new MainWindow();
                a.checkConnection();
                loadData(); //Loading data into DataGrid
            }
            catch (Exception ee)
            { MessageBox.Show(ee.ToString()); }
        }
        private void goToStartState()
        {
            makeEmptyAll(); isEnableFalse();
            tb_id.IsEnabled = false;
            tb_id.Text = "";
            btn_update.Visibility = Visibility.Hidden;
            btn_cancel.Visibility = Visibility.Hidden;
            btn_fire.Visibility = Visibility.Hidden;
            btn_addFinal.Visibility = Visibility.Hidden;
            label_or.Visibility = Visibility.Hidden;
            btn_add.IsEnabled = true;
            btn_remove.IsEnabled = true;
            btn_edit.IsEnabled = true;
        }
        private void loadData()
        {
            con.Close();
            try
            {
                con.Open();
                if (con.State == ConnectionState.Open)
                {
                    String query = "select * from Teachers_Data";
                    SqlDataAdapter sda = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    con.Close();
                    dataGrid.ItemsSource = dt.DefaultView;

                }
                else { MessageBox.Show("Connection is Closed!"); }
            }
            catch (Exception ee)
            { MessageBox.Show(ee.ToString()); }
        }
        private bool checkEmptyTable()
        {
            try
            {
                if (dataGrid.Items.Count < 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ee)
            { MessageBox.Show(ee.ToString()); return false; }
        }
        private void makeEmptyAll()
        {
            tb_name.Text = "";
            tb_pw.Password = "";
            tb_contactNo.Text = "";
            tb_salary.Text = "";
            chckbx_eng.IsChecked = false;
            chckbx_isl.IsChecked = false;
            chckbx_maths.IsChecked = false;
            chckbx_urdu.IsChecked = false;
            chckbx_class1.IsChecked = false;
            chckbx_class2.IsChecked = false;
            chckbx_class3.IsChecked = false;
            chckbx_class4.IsChecked = false;
            chckbx_class5.IsChecked = false;
            chckbx_class6.IsChecked = false;
            chckbx_class7.IsChecked = false;
            chckbx_class8.IsChecked = false;
            chckbx_class9.IsChecked = false;
            chckbx_class10.IsChecked = false;
            cb_gender.Text = "";
            cb_classTeacherofClass.Text = "";
        }
        private int isPasswordinLimit()
        {
            if ((tb_pw.Password.Count() >= 8) && (tb_pw.Password.Count() <= 25))
                return 1; //ok
            else
                return 0; //Not ok
        }
        private string intoDateFormat(string NotDateFormat)
        {
            string[] x;
            x = NotDateFormat.Split(' ');
            return x[0];
        }
        private int isAnyFill()
        {
            if ((tb_name.Text == "") && (tb_contactNo.Text == "") && (tb_salary.Text == "") && (!isSubjectChecked()) && (!isClassChecked()) && (cb_gender.Text == "") && (cb_classTeacherofClass.Text == ""))
                return 0; //All Empty
            if ((tb_name.Text != "") && (tb_contactNo.Text != "") && (tb_salary.Text != "") && (isSubjectChecked()) && (isClassChecked()) && (cb_gender.Text != "") && (cb_classTeacherofClass.Text != ""))
                return 2; //All Filled
            else return 1; //Some Empty
        }
        private void btn_close_onClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Quit", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                Application.Current.Shutdown();
        }
        private void btn_minimize_onClick(object sender, RoutedEventArgs e)
        { this.WindowState = WindowState.Minimized; }
        private void btn_logout_onClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Logout", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                MainWindow a = new MainWindow();
                a.Show();
                this.Close();
            }

        }
        private void Btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            goToStartState();
            loadData();
        }
        private string removeWhiteSpaces(string text)
        {
            return string.Join("", text.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
        }
        private String removeComas(String str)
        {
            String a = "";
            for (int i = 0; i < str.Count() - 1; i++)
                a += str[i];
            return a;
        }
        private void Chckbx_allSubjects_Checked(object sender, RoutedEventArgs e)
        {
            bool val = (chckbx_allSubjectss.IsChecked == true);
            chckbx_eng.IsChecked = val;
            chckbx_urdu.IsChecked = val;
            chckbx_maths.IsChecked = val;
            chckbx_isl.IsChecked = val;
        }
        private void Chckbx_allSubject_Checked(object sender, RoutedEventArgs e)
        {
            chckbx_allSubjectss.IsChecked = null;
            if ((chckbx_eng.IsChecked == true) && (chckbx_urdu.IsChecked == true) && (chckbx_maths.IsChecked == true) && (chckbx_isl.IsChecked == true))
                chckbx_allSubjectss.IsChecked = true;
            if ((chckbx_eng.IsChecked == false) && (chckbx_urdu.IsChecked == false) && (chckbx_maths.IsChecked == false) && (chckbx_isl.IsChecked == false))
                chckbx_allSubjectss.IsChecked = false;
        }
        private void allowOnlyNumbers(object sender, TextCompositionEventArgs e)
        { Regex regex = new Regex("[^0-9.-]+"); e.Handled = regex.IsMatch(e.Text); }
        private bool isClassChecked()
        {
            if ((chckbx_class1.IsChecked.Value) || (chckbx_class2.IsChecked.Value) || (chckbx_class3.IsChecked.Value) || (chckbx_class4.IsChecked.Value) || (chckbx_class5.IsChecked.Value) ||
                (chckbx_class6.IsChecked.Value) || (chckbx_class7.IsChecked.Value) || (chckbx_class8.IsChecked.Value) || (chckbx_class9.IsChecked.Value) || (chckbx_class10.IsChecked.Value))
                return true;
            else
                return false;
        }
        private bool isSubjectChecked()
        {
            if ((chckbx_eng.IsChecked.Value) || (chckbx_isl.IsChecked.Value) || (chckbx_maths.IsChecked.Value) || (chckbx_urdu.IsChecked.Value))
                return true;
            else
                return false;
        }
        private void isEnableTrue()
        {
            tb_name.IsEnabled = true;
            tb_pw.IsEnabled = true;
            tb_contactNo.IsEnabled = true;
            tb_salary.IsEnabled = true;
            chckbx_allSubjectss.IsEnabled = true;
            chckbx_eng.IsEnabled = true;
            chckbx_urdu.IsEnabled = true;
            chckbx_isl.IsEnabled = true;
            chckbx_maths.IsEnabled = true;
            chckbx_class1.IsEnabled = true;
            chckbx_class2.IsEnabled = true;
            chckbx_class3.IsEnabled = true;
            chckbx_class4.IsEnabled = true;
            chckbx_class5.IsEnabled = true;
            chckbx_class6.IsEnabled = true;
            chckbx_class7.IsEnabled = true;
            chckbx_class8.IsEnabled = true;
            chckbx_class9.IsEnabled = true;
            chckbx_class10.IsEnabled = true;
            cb_classTeacherofClass.IsEnabled = true;
            cb_gender.IsEnabled = true;
        }
        private void isEnableFalse()
        {
            tb_name.IsEnabled = false;
            tb_pw.IsEnabled = false;
            tb_contactNo.IsEnabled = false;
            tb_salary.IsEnabled = false;
            chckbx_allSubjectss.IsEnabled = false;
            chckbx_eng.IsEnabled = false;
            chckbx_urdu.IsEnabled = false;
            chckbx_isl.IsEnabled = false;
            chckbx_maths.IsEnabled = false;
            chckbx_class1.IsEnabled = false;
            chckbx_class2.IsEnabled = false;
            chckbx_class3.IsEnabled = false;
            chckbx_class4.IsEnabled = false;
            chckbx_class5.IsEnabled = false;
            chckbx_class6.IsEnabled = false;
            chckbx_class7.IsEnabled = false;
            chckbx_class8.IsEnabled = false;
            chckbx_class9.IsEnabled = false;
            chckbx_class10.IsEnabled = false;
            cb_classTeacherofClass.IsEnabled = false;
            cb_gender.IsEnabled = false;
        }
        public string Encrypt(string textToEncrypt)
        {
            try
            {
                string ToReturn = "";
                string _key = "ay$a5%&jwrtmnh;lasjdf98787";
                string _iv = "def@45678bgvjakawb)$%hg";
                byte[] _ivByte = { };
                _ivByte = System.Text.Encoding.UTF8.GetBytes(_iv.Substring(0, 8));
                byte[] _keybyte = { };
                _keybyte = System.Text.Encoding.UTF8.GetBytes(_key.Substring(0, 8));
                MemoryStream ms = null; CryptoStream cs = null;
                byte[] inputbyteArray = System.Text.Encoding.UTF8.GetBytes(textToEncrypt);
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, des.CreateEncryptor(_keybyte, _ivByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    ToReturn = Convert.ToBase64String(ms.ToArray());
                }
                return ToReturn;
            }
            catch (Exception ae)
            {
                throw new Exception(ae.Message, ae.InnerException);
            }
        }
        public string Decrypt(string textToDecrypt)
        {
            try
            {
                string ToReturn = "";
                //string _key = "ay$a5%&jwrtmnh;lasjdf98787";
                //string _iv = "abc@98797hjkas$&asd(*$%";
                string _key = "ay$a5%&jwrtmnh;lasjdf98787";
                string _iv = "def@45678bgvjakawb)$%hg";
                byte[] _ivByte = { };
                _ivByte = System.Text.Encoding.UTF8.GetBytes(_iv.Substring(0, 8));
                byte[] _keybyte = { };
                _keybyte = System.Text.Encoding.UTF8.GetBytes(_key.Substring(0, 8));
                MemoryStream ms = null; CryptoStream cs = null;
                byte[] inputbyteArray = new byte[textToDecrypt.Replace(" ", "+").Length];
                inputbyteArray = Convert.FromBase64String(textToDecrypt.Replace(" ", "+"));
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, des.CreateDecryptor(_keybyte, _ivByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    Encoding encoding = Encoding.UTF8;
                    ToReturn = encoding.GetString(ms.ToArray());
                }
                return ToReturn;
            }
            catch (Exception ae)
            {
                throw new Exception(ae.Message, ae.InnerException);
            }
        }
        private void searchfromDB_KeyUp(object sender, KeyEventArgs e)
        {
            if (tb_id.Text == "")
            { loadData(); makeEmptyAll(); }
            else
            {
                try
                {
                    con.Open();
                    if (con.State == ConnectionState.Open)
                    {
                        String query = "select * from Teachers_Data where id like'" + removeWhiteSpaces(tb_id.Text) + "%" + "'";
                        SqlDataAdapter sda = new SqlDataAdapter(query, con);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        dataGrid.ItemsSource = dt.DefaultView;
                        SqlCommand cmd = new SqlCommand(query, con);
                        SqlDataReader dataReader = cmd.ExecuteReader();
                        //MessageBox.Show(dataReader.ToString() + dataReader.Read());
                        if (dataReader.Read())
                        {
                            //Retrieving data vales from select query in variables
                            tb_name.Text = dataReader["Name"].ToString();
                            string pw = dataReader["Password"].ToString();
                            tb_pw.Password = Decrypt(pw);
                            tb_contactNo.Text = dataReader["Contact_No"].ToString();
                            tb_salary.Text = dataReader["Salary"].ToString();
                            cb_classTeacherofClass.Text = dataReader["ClassTeacher_of_Class"].ToString();
                            cb_gender.Text = dataReader["Gender"].ToString();
                            //Get checkboxes data from DB and set it into checkboxes
                            String[] temp, temp3;
                            String temp2 = dataReader["Subjects"].ToString(), temp4 = dataReader["Classes"].ToString();
                            temp = temp2.Split(',');
                            foreach (string val in temp)
                            {
                                if (val == "English")
                                    chckbx_eng.IsChecked = true;
                                if (val == "Urdu")
                                    chckbx_urdu.IsChecked = true;
                                if (val == "Islamiat")
                                    chckbx_isl.IsChecked = true;
                                if (val == "Mathematics")
                                    chckbx_maths.IsChecked = true;
                            }
                            temp3 = temp4.Split(',');
                            foreach (string val in temp3)
                            {
                                if (val == "I")
                                    chckbx_class1.IsChecked = true;
                                if (val == "II")
                                    chckbx_class2.IsChecked = true;
                                if (val == "III")
                                    chckbx_class3.IsChecked = true;
                                if (val == "IV")
                                    chckbx_class4.IsChecked = true;
                                if (val == "V")
                                    chckbx_class5.IsChecked = true;
                                if (val == "VI")
                                    chckbx_class6.IsChecked = true;
                                if (val == "VII")
                                    chckbx_class7.IsChecked = true;
                                if (val == "VIII")
                                    chckbx_class8.IsChecked = true;
                                if (val == "IX")
                                    chckbx_class9.IsChecked = true;
                                if (val == "X")
                                    chckbx_class10.IsChecked = true;
                            }
                        }
                        else makeEmptyAll();
                        con.Close();
                    }
                    else MessageBox.Show("Connection Error!"); //Error; Connection is not open
                }
                catch (Exception ee)
                { MessageBox.Show(ee.ToString()); con.Close(); goToStartState(); loadData(); }
            }
        }
        private void Btn_add_Click(object sender, RoutedEventArgs e)
        {
            if (isAnyFill() == 0)
            {
                btn_cancel.Visibility = Visibility.Visible;
                btn_addFinal.Visibility = Visibility.Visible;
                btn_add.IsEnabled = false;
                btn_remove.IsEnabled = false;
                btn_edit.IsEnabled = false;
                tb_id.IsEnabled = false;
                isEnableTrue();
                MessageBox.Show("Fill in Fields");
                tb_name.Focus();
            }
        }
        private void Btn_addFinal_Click(object sender, RoutedEventArgs e)
        {
            if ((isAnyFill() == 0) && (isPasswordinLimit() == 0)) //all empty
            { MessageBox.Show("Fill in Fields"); tb_name.Focus(); }
            else if (isAnyFill() == 1) //some filled
                MessageBox.Show("Fill in all the fields!");
            else if (isAnyFill() == 2) //all filled
            {
                if (isPasswordinLimit() == 0)
                { MessageBox.Show("Passwords must be at least 8 characters long and in limit of 25 characters"); tb_pw.Focus(); }
                else if (isPasswordinLimit() == 1)
                { addDataintoDatabase(); goToStartState(); btn_add.Focus(); }
            }
        }
        private void addDataintoDatabase()
        {
            try
            {
                con.Open();
                //Getting data from checkbox into list
                ArrayList subjects_list = new ArrayList();
                ArrayList classes_list = new ArrayList();
                String subjects = "", classes = "";
                if (chckbx_eng.IsChecked.Value)
                    subjects_list.Add("English");
                if (chckbx_urdu.IsChecked.Value)
                    subjects_list.Add("Urdu");
                if (chckbx_isl.IsChecked.Value)
                    subjects_list.Add("Islamiat");
                if (chckbx_maths.IsChecked.Value)
                    subjects_list.Add("Mathematics");
                foreach (String str in subjects_list)
                    subjects += str + ",";
                if (chckbx_class1.IsChecked.Value)
                    classes_list.Add("I");
                if (chckbx_class2.IsChecked.Value)
                    classes_list.Add("II");
                if (chckbx_class3.IsChecked.Value)
                    classes_list.Add("III");
                if (chckbx_class4.IsChecked.Value)
                    classes_list.Add("IV");
                if (chckbx_class5.IsChecked.Value)
                    classes_list.Add("V");
                if (chckbx_class6.IsChecked.Value)
                    classes_list.Add("VI");
                if (chckbx_class7.IsChecked.Value)
                    classes_list.Add("VII");
                if (chckbx_class8.IsChecked.Value)
                    classes_list.Add("VIII");
                if (chckbx_class9.IsChecked.Value)
                    classes_list.Add("IX");
                if (chckbx_class10.IsChecked.Value)
                    classes_list.Add("X");
                foreach (String str2 in classes_list)
                    classes += str2 + ",";
                if (con.State == ConnectionState.Open)
                {
                    string query = "insert into Teachers_Data (Name, Password, Gender, Subjects, Classes, ClassTeacher_of_Class, Contact_No, Join_Date, Active, Salary)" +
                                                    " values('"
                                                           + removeWhiteSpaces(tb_name.Text) +
                                                    "', '" + Encrypt(tb_pw.Password.ToString()) +
                                                    "', '" + cb_gender.Text +
                                                    "', '" + removeComas(subjects) +
                                                    "', '" + removeComas(classes) +
                                                    "', '" + cb_classTeacherofClass.Text +
                                                    "', '" + Regex.Replace(tb_contactNo.Text, @"\s+", "").ToString() +
                                                    "', '" + intoDateFormat(DateTime.Now.Date.ToString()) +
                                                    "', '" + 1 +
                                                    "', '" + int.Parse(Regex.Replace(tb_salary.Text, @"\s+", "")) + "')";

                    SqlCommand scmd = new SqlCommand(query, con);
                    scmd.ExecuteNonQuery();
                    loadData();
                    MessageBox.Show("Done!");
                    con.Close();
                }
                else
                { MessageBox.Show("Connection Error!"); con.Close(); }
            }
            catch (Exception ee)
            { MessageBox.Show(ee.ToString()); btn_add.Focus(); con.Close(); }
        }
        private void Btn_remove_Click(object sender, RoutedEventArgs e)
        {
            if (checkEmptyTable() && (btn_cancel.Visibility == Visibility.Hidden)) MessageBox.Show("Table is Empty!", "Error");
            else if (checkEmptyTable())
            { MessageBox.Show("ID Not Found!"); tb_id.Focus(); }
            else if (isAnyFill() == 0)
            {
                btn_add.IsEnabled = false;
                btn_edit.IsEnabled = false;
                MessageBox.Show("Search by ID"); tb_id.IsEnabled = true; tb_id.Focus();
                btn_cancel.Visibility = Visibility.Visible; btn_fire.Visibility = Visibility.Visible; label_or.Visibility = Visibility.Visible;
            }
            else if (tb_id.Text == "") { MessageBox.Show("Search by ID"); tb_id.Focus(); }
            else
            { removeDatafromDatabase(); tb_id.IsEnabled = false; goToStartState(); btn_remove.Focus(); }
        }
        private void removeDatafromDatabase()
        {
            try
            {
                con.Open();
                if (con.State == ConnectionState.Open)
                {
                    string query = "delete from Teachers_Data where id = '" + int.Parse(removeWhiteSpaces(tb_id.Text)) + "'";
                    SqlCommand scmd = new SqlCommand(query, con);
                    scmd.ExecuteNonQuery();
                    loadData();
                    MessageBox.Show("Deleted!");
                    con.Close();
                }
                else
                { MessageBox.Show("Connection Error!"); }
            }
            catch (Exception ee)
            { MessageBox.Show(ee.ToString()); con.Close(); goToStartState(); loadData(); }
        }
        private void Btn_edit_Click(object sender, RoutedEventArgs e)
        {
            if (checkEmptyTable() && (btn_cancel.Visibility == Visibility.Hidden)) MessageBox.Show("Table is Empty!", "Error");
            else if (isAnyFill() == 0)
            {
                btn_cancel.Visibility = Visibility.Visible;
                btn_update.Visibility = Visibility.Visible;
                btn_add.IsEnabled = false;
                btn_remove.IsEnabled = false;
                btn_edit.IsEnabled = false;
                MessageBox.Show("Search by ID"); tb_id.IsEnabled = true; isEnableTrue(); tb_id.Focus();
            }
        }
        private void Btn_update_Click(object sender, RoutedEventArgs e)
        {
            if(tb_id.Text == "") { MessageBox.Show("Search by ID"); makeEmptyAll(); tb_id.Focus(); }
            else if ((checkEmptyTable() && (tb_id.Text != "")))
            { MessageBox.Show("ID Not Found!"); tb_id.Focus(); }
            else if (isAnyFill() == 2)
            { updateDatafromDatabase(); goToStartState(); }
            else if (isAnyFill() == 1)
                MessageBox.Show("Fill in all the Fields!");
            else
            { MessageBox.Show("Search by ID"); tb_id.Focus(); }
        }
        private void updateDatafromDatabase()
        {
            try
            {
                con.Open();
                //Getting data from checkbox into list
                ArrayList subjects_list = new ArrayList();
                ArrayList classes_list = new ArrayList();
                String subjects = "", classes = "";
                if (chckbx_eng.IsChecked.Value)
                    subjects_list.Add("English");
                if (chckbx_urdu.IsChecked.Value)
                    subjects_list.Add("Urdu");
                if (chckbx_isl.IsChecked.Value)
                    subjects_list.Add("Islamiat");
                if (chckbx_maths.IsChecked.Value)
                    subjects_list.Add("Mathematics");
                foreach (String str in subjects_list)
                    subjects += str + ",";
                if (chckbx_class1.IsChecked.Value)
                    classes_list.Add("I");
                if (chckbx_class2.IsChecked.Value)
                    classes_list.Add("II");
                if (chckbx_class3.IsChecked.Value)
                    classes_list.Add("III");
                if (chckbx_class4.IsChecked.Value)
                    classes_list.Add("IV");
                if (chckbx_class5.IsChecked.Value)
                    classes_list.Add("V");
                if (chckbx_class6.IsChecked.Value)
                    classes_list.Add("VI");
                if (chckbx_class7.IsChecked.Value)
                    classes_list.Add("VII");
                if (chckbx_class8.IsChecked.Value)
                    classes_list.Add("VIII");
                if (chckbx_class9.IsChecked.Value)
                    classes_list.Add("IX");
                if (chckbx_class10.IsChecked.Value)
                    classes_list.Add("X");
                foreach (String str2 in classes_list)
                    classes += str2 + ",";
                if (con.State == ConnectionState.Open)
                {
                    string query = "update Teachers_Data set Name ='" + tb_name.Text +
                                                    "' , Password = '" + Encrypt(tb_pw.Password) +
                                                    "', Gender = '" + cb_gender.Text +
                                                    "', Subjects = '" + removeComas(subjects) +
                                                    "', Classes = '" + removeComas(classes) +
                                                    "', ClassTeacher_of_Class =  '" + cb_classTeacherofClass.Text +
                                                    "', Contact_No = '" + tb_contactNo.Text +
                                                    "', Salary = '" + int.Parse(tb_salary.Text) + "'" +
                                                    "where ID = '" + int.Parse(removeWhiteSpaces(tb_id.Text)) + "'";
                    SqlCommand scmd = new SqlCommand(query, con);
                    scmd.ExecuteNonQuery();
                    loadData();
                    MessageBox.Show("Done!");
                    con.Close();
                }
                else
                { MessageBox.Show("Connection Error!"); }
            }
            catch (Exception ee)
            { MessageBox.Show(ee.ToString()); con.Close(); goToStartState(); loadData(); }
        }
        private void Btn_fire_Click(object sender, RoutedEventArgs e)
        {
            if (tb_id.Text == "") { MessageBox.Show("Search by ID"); tb_id.Focus(); }
            else if (checkEmptyTable())
            { MessageBox.Show("ID Not Found!"); tb_id.Focus(); }
            else if (isAnyFill() == 0)
            { MessageBox.Show("Search by ID"); tb_id.Focus(); }
            else if (isAnyFill() == 2)
            { fireTeacher(); goToStartState(); }            
        }
        private void fireTeacher()
        {
            try
            {
                con.Open();
                if (con.State == ConnectionState.Open)
                {
                    String query = "select Leave_Date from Teachers_Data where id ='" + removeWhiteSpaces(tb_id.Text) + "'";

                    SqlCommand scmd1 = new SqlCommand(query, con);
                    SqlDataReader dataReader = scmd1.ExecuteReader();
                    string LD = "";
                    if (dataReader.Read())
                        LD = dataReader["Leave_Date"].ToString();
                    con.Close();
                    con.Open();
                    if (LD == "")
                    {
                        string query2 = "update Teachers_Data set Leave_Date ='" + intoDateFormat(DateTime.Now.Date.ToString()) + "', Active = '" + 0 + "' where ID = '" + removeWhiteSpaces(tb_id.Text) + "'";
                        SqlCommand scmd2 = new SqlCommand(query2, con);
                        scmd2.ExecuteNonQuery();
                        loadData();
                        MessageBox.Show("Done!");
                    }
                    else
                        MessageBox.Show("Already Fired!");
                    dataReader.Close();
                    con.Close();
                    loadData();                    
                }
                else MessageBox.Show("Connection Error!"); //Error; Connection is not open
            }
            catch (Exception ee)
            { MessageBox.Show(ee.ToString()); con.Close(); goToStartState(); loadData(); }
        }
    }
}