using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace HotelManagement
{
    public partial class Services : Form
    {
        public List<string> SelectedServices = new List<string>();
        public bool UpdateMode;
        public int CheckBoxWidth = 0, NumOfServices = 0;

        public Services(List<string> text, bool IsUpdate)
        {
            InitializeComponent();
            SelectedServices = text;
            UpdateMode = IsUpdate;
        }

        public void AddService(string Service, double Price)
        {
            DatabaseMethods dbme = new DatabaseMethods();
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@service", SqlDbType.VarChar, 30);
            param[0].Value = Service;
            param[1] = new SqlParameter("@price", SqlDbType.Float);
            param[1].Value = Price;
            dbme.ConOpen();
            dbme.Query("SuperUser_Add_Service", param);
            dbme.ConClose();
        }

        public void DeleteService(int id)
        {
            DatabaseMethods dbme = new DatabaseMethods();
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@id", SqlDbType.Int);
            param[0].Value = id;
            dbme.ConOpen();
            dbme.Query("SuperUser_Delete_Service", param);
            dbme.ConClose();
        }

        public DataTable SelectService(int id)
        {
            DatabaseMethods dbme = new DatabaseMethods();
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@id", SqlDbType.Int);
            param[0].Value = id;
            DataTable dt = new DataTable();
            dbme.ConOpen();
            dt = dbme.ReadData("SuperUser_Select_Service", param);
            dbme.ConClose();
            return dt;
        }

        public void UpdateService(int id, string Service, double Price)
        {
            DatabaseMethods dbme = new DatabaseMethods();
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@id", SqlDbType.Int);
            param[0].Value = id;
            param[1] = new SqlParameter("@service", SqlDbType.VarChar, 30);
            param[1].Value = Service;
            param[2] = new SqlParameter("@price", SqlDbType.Float);
            param[2].Value = Price;
            dbme.ConOpen();
            dbme.Query("SuperUser_Update_Service", param);
            dbme.ConClose();
        }

        public float ServiceCost(int ServiceId)
        {
            DatabaseMethods dbme = new DatabaseMethods();
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@Service_id", SqlDbType.Int);
            param[0].Value = ServiceId;
            DataTable dt = new DataTable();
            dbme.ConOpen();
            dt = dbme.ReadData("Service_Cost", param);
            dbme.ConClose();
            float result = float.Parse(dt.Rows[0][0].ToString());
            return result;
        }

        public void LoadItems()
        {
            DatabaseMethods dbme = new DatabaseMethods();
            DataTable dt = new DataTable();
            dbme.ConOpen();
            dt = dbme.ReadData("Read_Services", null);
            dbme.ConClose();
            NumOfServices = dt.Rows.Count;
            foreach (DataRow row in dt.Rows)
            {
                string item = row[1].ToString() + " - " + row[2].ToString();
                string index = row[0].ToString();
                ServicesIdListBox.Items.Add(index);
                ServicesCheckedListBox.Items.Add(item);
                ServiceLenLabel.Text = item;
                if (CheckBoxWidth < ServiceLenLabel.Size.Width)
                    CheckBoxWidth = ServiceLenLabel.Size.Width;
            }
            CheckBoxWidth += 20;
        }

        private void ChangeSize()
        {
            int CheckBoxHeight = 15 * ServicesCheckedListBox.Items.Count + 4;
            ServicesCheckedListBox.Size = new Size(CheckBoxWidth, CheckBoxHeight);
            int NewFormWidth = 40 + ServicesCheckedListBox.Size.Width;
            int NewFormHeight = 100 + 15 * ServicesCheckedListBox.Items.Count;
            this.Size = new Size(NewFormWidth, NewFormHeight);
            AcceptServicesButton.Location = new Point((this.ClientSize.Width - AcceptServicesButton.Size.Width) / 2,
                CheckBoxHeight + 30);
        }

        private void LoadCheckBoxes()
        {
            string[] list1 = new string[NumOfServices];
            for (int i = 0; i < ServicesIdListBox.Items.Count; i++)
            {
                for(int j=0; j<SelectedServices.Count; j++)
                {
                    if (ServicesIdListBox.Items[i].ToString() == SelectedServices[j].ToString())
                    {
                        list1[i] = SelectedServices[j].ToString();
                        break;
                    }
                    else list1[i] = "0";
                }
                //list1[i] = SelectedServices;
            }
                
            for (int MyIndex=0; MyIndex<ServicesCheckedListBox.Items.Count; MyIndex++ )
                if (Convert.ToInt16(list1[MyIndex]) > 0)
                    ServicesCheckedListBox.SetItemCheckState(ServicesCheckedListBox.Items.
                        IndexOf(ServicesCheckedListBox.Items[MyIndex]), CheckState.Checked);
                else ServicesCheckedListBox.SetItemCheckState(ServicesCheckedListBox.Items.
                    IndexOf(ServicesCheckedListBox.Items[MyIndex]), CheckState.Unchecked);
        }

        private void TransferListBoxItems(ListBox from, ListBox to)
        {
            to.Items.Clear();
            foreach (var item in from.Items)
            {
                to.Items.Add(item);
            }
        }

        private void SaveServices()
        {
            UpdateResultLabel();
            HotelManagement hm = this.Owner as HotelManagement;
            if (UpdateMode)
                TransferListBoxItems(ResultListBox, hm.SelectedServicesListBox2);
                //hm.SelectedServicesListBox2 = ResultListBox;
            else TransferListBoxItems(ResultListBox, hm.SelectedServicesListBox1);
            //hm.SelectedServicesListBox1 = ResultListBox;
            Dispose();
        }

        private void UpdateResultLabel()
        {
            string[] list1 = new string[NumOfServices];
            //for (int i = 0; i < ResultLabel.Text.Length; i++)
            //    list1[i] = ResultLabel.Text[i];
            int MyIndex = 0;
            foreach (object itemChecked in ServicesCheckedListBox.Items)
            {
                if (ServicesCheckedListBox.GetItemCheckState(ServicesCheckedListBox.Items.IndexOf(itemChecked))
                    == CheckState.Checked)
                {
                    list1[MyIndex] = ServicesIdListBox.Items[MyIndex].ToString();
                }
                    //list1[MyIndex] = '1';
                else list1[MyIndex] = "0";
                MyIndex++;
            }
            ResultListBox.Items.Clear();
            //ResultLabel.Text = "";
            for (int i = 0; i < list1.Length; i++)
            {
                if(Convert.ToInt16(list1[i])>0)
                {
                    ResultListBox.Items.Add(list1[i]);
                }
            }
                //ResultLabel.Text += list1[i];
        }

        private void AcceptServicesButton_Click(object sender, EventArgs e)
        {
            SaveServices();
        }

        private void Services_Load(object sender, EventArgs e)
        {
            LoadItems();
            LoadCheckBoxes();
            ChangeSize();
        }
    }
}
