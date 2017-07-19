using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HotelManagement
{
    public partial class LoginData : Form
    {
        public LoginData()
        {
            InitializeComponent();
        }

        private void LogOut()
        {
            base.Dispose();
            Login lg = new Login(false);
            lg.Visible = true;
        }

        private DataTable SelectAccount(string Login)
        {
            DatabaseMethods dbme = new DatabaseMethods();
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@login", SqlDbType.VarChar);
            param[0].Value = Login;
            DataTable dt = new DataTable();
            dbme.ConOpen();
            dt = dbme.ReadData("SuperUser_Select_Account", param);
            dbme.ConClose();
            return dt;
        }

        private void UpdateLogin(int id, string Login)
        {
            DatabaseMethods dbme = new DatabaseMethods();
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@id", SqlDbType.Int);
            param[0].Value = id;
            param[1] = new SqlParameter("@login", SqlDbType.VarChar);
            param[1].Value = Login;
            dbme.ConOpen();
            dbme.Query("SuperUser_Update_Login", param);
            dbme.ConClose();
        }

        private void Update_Login()
        {
            string OldLogin = UpdateOldLoginTextBox.Text;
            string Password = UpdateLoginPasswordTextBox.Text;
            string NewLogin = UpdateNewLoginTextBox.Text;
            if (OldLogin.Trim().Length == 0 || Password.Trim().Length == 0 || NewLogin.Trim().Length == 0)
                MessageBox.Show("You have left some field empty.", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            else
            {
                DataTable IsUserExist = SelectAccount(OldLogin);
                if (IsUserExist.Rows.Count > 0)
                {
                    if (IsUserExist.Rows[0][2].ToString() == Password)
                    {
                        DataTable IsNewLoginEligible = SelectAccount(NewLogin);
                        if (IsNewLoginEligible.Rows.Count == 0)
                        {
                            int AccountID = Convert.ToInt16(IsUserExist.Rows[0][0].ToString());
                            UpdateLogin(AccountID, NewLogin);
                            string TextPattern = "Default User";
                            if (AccountID == 0)
                                TextPattern = "Super User";
                            MessageBox.Show(TextPattern + " Login has been successfully changed.", "Login Change",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearTextBoxes(UpdateOldLoginTextBox, UpdateLoginPasswordTextBox, UpdateNewLoginTextBox);
                        }
                        else MessageBox.Show("New Login you entered already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else MessageBox.Show("Incorrect Password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    MessageBox.Show("User with the following login does not exist.", "Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }
        }

        private void UpdatePassword(int id, string Password)
        {
            DatabaseMethods dbme = new DatabaseMethods();
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@id", SqlDbType.Int);
            param[0].Value = id;
            param[1] = new SqlParameter("@password", SqlDbType.VarChar);
            param[1].Value = Password;
            dbme.ConOpen();
            dbme.Query("SuperUser_Update_Password", param);
            dbme.ConClose();
        }

        private void Update_Password()
        {
            string UserLogin = UpdatePasswordLoginTextBox.Text;
            string OldPassword = UpdateOldPasswordTextBox.Text;
            string NewPassword = UpdateNewPasswordTextBox.Text;
            string ConfirmPassword = UpdateConfirmPasswordTextBox.Text;
            if (UserLogin.Trim().Length == 0 || OldPassword.Trim().Length == 0 || NewPassword.Trim().Length == 0 ||
                ConfirmPassword.Trim().Length == 0)
                MessageBox.Show("You have left some field empty.", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            else
            {
                DataTable IsUserExist = SelectAccount(UserLogin);
                if (IsUserExist.Rows.Count > 0)
                {
                    if (IsUserExist.Rows[0][2].ToString() == OldPassword)
                    {
                        if (NewPassword == ConfirmPassword)
                        {
                            int AccountID = Convert.ToInt16(IsUserExist.Rows[0][0].ToString());
                            UpdatePassword(AccountID, NewPassword);
                            string TextPattern = "Default User";
                            if (AccountID == 0)
                                TextPattern = "Super User";
                            MessageBox.Show(TextPattern + " Password has been successfully changed.",
                                "Password Change", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearTextBoxes(UpdatePasswordLoginTextBox, UpdateOldPasswordTextBox,
                                UpdateNewPasswordTextBox, UpdateConfirmPasswordTextBox);
                        }
                        else MessageBox.Show("Password filled in 'Confirm Password' field differs from New Password.",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else MessageBox.Show("Old Password is incorrect.", "Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                else MessageBox.Show("User with the following login does not exist.", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ShowPasswords()
        {
            if(!ShowPasswordCheckBox.Checked)
            {
                UpdateLoginPasswordTextBox.PasswordChar = '*';
                UpdateOldPasswordTextBox.PasswordChar = '*';
                UpdateNewPasswordTextBox.PasswordChar = '*';
                UpdateConfirmPasswordTextBox.PasswordChar = '*';
            }
            if(ShowPasswordCheckBox.Checked)
            {
                UpdateLoginPasswordTextBox.PasswordChar = '\0';
                UpdateOldPasswordTextBox.PasswordChar = '\0';
                UpdateNewPasswordTextBox.PasswordChar = '\0';
                UpdateConfirmPasswordTextBox.PasswordChar = '\0';
            }
        }

        private void ClearTextBoxes(params TextBox[] Fields)
        {
            foreach (TextBox tb in Fields)
                tb.Clear();
        }

        private void LoginData_FormClosed(object sender, FormClosedEventArgs e)
        {
            LogOut();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            LogOut();
        }

        private void UpdateLoginButton_Click(object sender, EventArgs e)
        {
            Update_Login();
        }

        private void UpdatePasswordButton_Click(object sender, EventArgs e)
        {
            Update_Password();
        }

        private void ShowPasswordCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ShowPasswords();
        }
    }
}
