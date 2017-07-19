using System;
using System.Data;
using System.Windows.Forms;

namespace HotelManagement
{
    public partial class Login : Form
    {
        public bool LoginUpdate;
        public Login(bool IsUpdate)
        {
            InitializeComponent();
            LoginUpdate = IsUpdate;
        }

        private int TryAuthentication(DataTable dt, string Login, string Password)
        {
            foreach(DataRow dr in dt.Rows)
            {
                if (Login == dr[1].ToString() && Password == dr[2].ToString())
                    return Convert.ToInt16(dr[0]);
            }
            return -1;
        }

        private void Authentication()
        {
            DatabaseMethods dbme = new DatabaseMethods();
            DataTable dt = dbme.ReadData("Read_Login_Data", null);
            string Login = LoginTextBox.Text;
            string Password = PasswordTextBox.Text;
            int Account = TryAuthentication(dt, Login, Password);
            if (Account == 1)
            {
                HotelManagement HotelManagement = new HotelManagement();
                HotelManagement.Show();
                this.Visible = false;
            }
            else if (Account == 0)
                if (!LoginUpdate)
                {
                    SuperUser SuperUser = new SuperUser();
                    SuperUser.Show();
                    this.Visible = false;
                }
            else
                {
                    LoginData LoginData = new LoginData();
                    LoginData.Show();
                    this.Visible = false;
                }
            else MessageBox.Show("Incorrect login or password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            Authentication();
        }

        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
