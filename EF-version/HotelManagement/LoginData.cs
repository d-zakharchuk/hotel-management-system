using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;

namespace HotelManagement
{
    public partial class LoginData : Form
    {
        private HotelDataEF.HotelManagementDatabaseEntities ctx;
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

        private object SelectAccount(string Login)
        {
            ctx = new HotelDataEF.HotelManagementDatabaseEntities();
            ctx.Login.Load();
            var result = (from l in ctx.Login where l.Login1 == Login select l).ToList();
            return result;
        }

        private object SelectAccount(int id)
        {
            ctx = new HotelDataEF.HotelManagementDatabaseEntities();
            ctx.Login.Load();
            var result = (from l in ctx.Login where l.Id == id select l).ToList();
            return result;
        }

        private void UpdateLogin(int id, string Login)
        {
            List<HotelDataEF.Login> AccountList = (List<HotelDataEF.Login>)(SelectAccount(id));
            HotelDataEF.Login DesiredAccount = AccountList[0];
            DesiredAccount.Login1 = Login;
            ctx.SaveChanges();
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
                List<HotelDataEF.Login> IsUserExist = (List<HotelDataEF.Login>)(SelectAccount(OldLogin));
                if (IsUserExist.Count > 0)
                {
                    if (IsUserExist[0].Password.ToString() == Password)
                    {
                        List<HotelDataEF.Login> IsNewLoginEligible = (List<HotelDataEF.Login>)(SelectAccount(NewLogin));
                        if (IsNewLoginEligible.Count == 0)
                        {
                            int AccountID = Convert.ToInt16(IsUserExist[0].Id.ToString());
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
            List<HotelDataEF.Login> AccountList = (List<HotelDataEF.Login>)(SelectAccount(id));
            HotelDataEF.Login DesiredAccount = AccountList[0];
            DesiredAccount.Password = Password;
            ctx.SaveChanges();

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
                List<HotelDataEF.Login> IsUserExist = (List<HotelDataEF.Login>)SelectAccount(UserLogin);
                if (IsUserExist.Count > 0)
                {
                    if (IsUserExist[0].Password.ToString() == OldPassword)
                    {
                        if (NewPassword == ConfirmPassword)
                        {
                            int AccountID = Convert.ToInt16(IsUserExist[0].Id.ToString());
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
