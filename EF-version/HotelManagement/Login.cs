using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;

namespace HotelManagement
{
    public partial class Login : Form
    {
        private HotelDataEF.HotelManagementDatabaseEntities ctx;
        public bool LoginUpdate;
        public Login(bool IsUpdate)
        {
            InitializeComponent();
            LoginUpdate = IsUpdate;
        }

        private int TryAuthentication(List<HotelDataEF.Login> LoginData, string Login, string Password)
        {
            foreach(var account in LoginData)
            {
                if (Login == account.Login1.ToString() && Password == account.Password.ToString())
                    return Convert.ToInt16(account.Id);
            }
            return -1;
        }

        private void Authentication()
        {
            ctx = new HotelDataEF.HotelManagementDatabaseEntities();
            ctx.Login.Load();
            List<HotelDataEF.Login> LoginInfo = (from l in ctx.Login select l).ToList();
            string Login = LoginTextBox.Text;
            string Password = PasswordTextBox.Text;
            int Account = TryAuthentication(LoginInfo, Login, Password);
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
