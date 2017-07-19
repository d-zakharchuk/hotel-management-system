using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace HotelManagement
{
    public partial class SuperUser : Form
    {
        public SuperUser()
        {
            InitializeComponent();
            ShowTypeTable();
        }
        DatabaseMethods dbme = new DatabaseMethods();
        Room ROOMS = new Room();

        private void PageChanges()
        {
            if (tabControl1.SelectedIndex == 0)
            {
                typeToolStripMenuItem.Checked = true;
                roomToolStripMenuItem.Checked = false;
                serviceToolStripMenuItem.Checked = false;
                incomeToolStripMenuItem.Checked = false;
                ShowTypeTable();
            }
            if (tabControl1.SelectedIndex == 1)
            {
                typeToolStripMenuItem.Checked = false;
                roomToolStripMenuItem.Checked = true;
                serviceToolStripMenuItem.Checked = false;
                incomeToolStripMenuItem.Checked = false;
                LoadRoomTypes();
                ShowRoomTable();
            }
            if (tabControl1.SelectedIndex == 2)
            {
                typeToolStripMenuItem.Checked = false;
                roomToolStripMenuItem.Checked = false;
                serviceToolStripMenuItem.Checked = true;
                incomeToolStripMenuItem.Checked = false;
                ShowServicesTable();
            }
            if (tabControl1.SelectedIndex == 3)
            {
                typeToolStripMenuItem.Checked = false;
                roomToolStripMenuItem.Checked = false;
                serviceToolStripMenuItem.Checked = false;
                incomeToolStripMenuItem.Checked = true;
                ShowIncomeTable();
            }
        }

        private void ChangeTabControlIndex(int index)
        {
            tabControl1.SelectedIndex = index;
            PageChanges();
        }

        private void OpenChangeLoginData()
        {
            base.Dispose();
            Login lg = new Login(true);
            lg.Visible = true;
        }

        private void LogOut()
        {
            base.Dispose();
            Login lg = new Login(false);
            lg.Visible = true;
        }

        private void ConfigureIncomeTableHeaders()
        {
            IncomeTable.Columns[0].HeaderText = "ID";
            IncomeTable.Columns[1].HeaderText = "Room";
            IncomeTable.Columns[2].HeaderText = "Residents";
            IncomeTable.Columns[3].HeaderText = "Date in";
            IncomeTable.Columns[4].HeaderText = "Date out";
            IncomeTable.Columns[5].HeaderText = "Room cost";
            IncomeTable.Columns[6].HeaderText = "Services cost";
            IncomeTable.Columns[7].HeaderText = "Total";
        }

        private void ShowIncomeTable()
        {
            IncomeTable.DataSource = dbme.ReadData("Income_Table", null);
            ConfigureIncomeTableHeaders();
        }

        private void ShowServicesTable()
        {
            ServiceTable.DataSource = dbme.ReadData("Read_Services", null);
        }

        private void ConfigureRoomTableHeaders()
        {
            RoomTable.Columns[0].HeaderText = "Room";
            RoomTable.Columns[1].HeaderText = "Type";
            RoomTable.Columns[2].HeaderText = "Floor";
            RoomTable.Columns[3].HeaderText = "People";
        }

        private void ShowRoomTable()
        {
            RoomTable.DataSource = dbme.ReadData("SuperUser_Read_Rooms", null);
            ConfigureRoomTableHeaders();
        }

        private void ConfigureTypeTableHeaders()
        {
            TypeTable.Columns[0].HeaderText = "Type";
            TypeTable.Columns[1].HeaderText = "Max people";
            TypeTable.Columns[2].HeaderText = "Price 1";
            TypeTable.Columns[3].HeaderText = "Price";
            TypeTable.Columns[4].HeaderText = "Area";
        }

        private void ShowTypeTable()
        {
            TypeTable.DataSource = dbme.ReadData("SuperUser_Read_Types", null);
            ConfigureTypeTableHeaders();
        }

        private void ClearTextBoxes(params TextBox[] Fields)
        {
            foreach (TextBox tb in Fields)
                tb.Clear();
        }

        private void SelectedServiceID()
        {
            if(!UpdateServiceButton.Enabled)
            {
                int CurrentRow = ServiceTable.CurrentCell.RowIndex;
                decimal value = Convert.ToDecimal(ServiceTable.Rows[CurrentRow].Cells[0].Value.ToString());
                UpdateServiceIDUpDown.Value = value;
                DeleteServiceIDUpDown.Value = value;
            }
        }

        private void SelectedIncomeID()
        {
            int CurrentRow = IncomeTable.CurrentCell.RowIndex;
            IncomeIDUpDown.Value = Convert.ToDecimal(IncomeTable.Rows[CurrentRow].Cells[0].Value.ToString());
        }

        private void SelectedRoomNum()
        {
            int CurrentRow = RoomTable.CurrentCell.RowIndex;
            if (!UpdateRoomButton.Enabled)
                RoomNumTextBox.Text = RoomTable.Rows[CurrentRow].Cells[0].Value.ToString();
        }

        private void SelectedType()
        {
            int CurrentRow = TypeTable.CurrentCell.RowIndex;
            if (!UpdateTypeButton.Enabled)
            {
                UpdateTypeTextBox.Text = TypeTable.Rows[CurrentRow].Cells[0].Value.ToString();
                DeleteTypeTextBox.Text = TypeTable.Rows[CurrentRow].Cells[0].Value.ToString();
            }
        }

        /*private int DetermineNewServiceID()
        {
            int NewID = 1;
            for (int i = 0; i < ServiceTable.RowCount; i++)
                if (Convert.ToInt16(ServiceTable.Rows[i].Cells[0].Value.ToString()) == i + 1)
                    NewID++;
                else break;
            return NewID;
        }*/

        private void Add_Service()
        {
            if (AddServiceNameTextBox.Text.Trim().Length == 0 || AddServicePriceTextBox.Text.Trim().Length == 0)
                MessageBox.Show("You have left some field(s) empty.", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            else
                try
                {
                    //int id = DetermineNewServiceID();
                    double Price = Convert.ToDouble(AddServicePriceTextBox.Text);
                    Services SERVICES = new Services(new List<string>(), false);
                    SERVICES.AddService(AddServiceNameTextBox.Text, Price);
                    MessageBox.Show("Service has been added successfully", "Service Add", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch
                {
                    MessageBox.Show("Service Price data is incorrect! Decimal num expected like '123,45'", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            ShowServicesTable();
        }

        private void Delete_Service()
        {
            DataTable dt = new DataTable();
            dt = dbme.ReadData("Select_All_Reservations", null);
            string ConfirmTextPattern = "Are you sure you want to delete this service?";
            if (dt.Rows.Count > 0)
                ConfirmTextPattern =
                    "It is not recommended to delete services if there are some reservations in hotel. Continue?";
            if (MessageBox.Show(ConfirmTextPattern, "Confirm Deletion", MessageBoxButtons.YesNo,
                MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                Services SERVICES = new Services(new List<string>(), false);
                int id = Convert.ToInt16(DeleteServiceIDUpDown.Value);
                SERVICES.DeleteService(id);
                //ServicesTableIntegrity(id);
                MessageBox.Show("Successfully deleted", "Delete Service", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Deletion cancelled", "Delete Service", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            ShowServicesTable();
        }

        /*private void ServicesTableIntegrity(int start)
        {
            Services SERVICES = new Services(new List<string>(), false);
            for (int i = start; i < ServiceTable.RowCount; i++)
                SERVICES.UpdateService(i + 1, i, ServiceTable.Rows[i].Cells[1].Value.ToString(),
                    Convert.ToDouble(ServiceTable.Rows[i].Cells[2].Value.ToString()));
        }*/

        private void BeginServiceUpdate()
        {
            BeginUpdateServiceButton.Text = "Cancel Update";
            UpdateServiceButton.Enabled = true;
            AddServiceButton.Enabled = false;
            DeleteServiceButton.Enabled = false;
            Services SERVICES = new Services(new List<string>(), false);
            DataTable dt = SERVICES.SelectService(Convert.ToInt16(UpdateServiceIDUpDown.Value));
            if (dt.Rows.Count > 0)
            {
                UpdateServiceNameTextBox.Text = dt.Rows[0][1].ToString();
                UpdateServicePriceTextBox.Text = dt.Rows[0][2].ToString();
            }
        }

        private void EndServiceUpdate()
        {
            BeginUpdateServiceButton.Text = "Begin Update";
            UpdateServiceButton.Enabled = false;
            AddServiceButton.Enabled = true;
            DeleteServiceButton.Enabled = true;
            ClearTextBoxes(UpdateServiceNameTextBox, UpdateServicePriceTextBox);
            SelectedServiceID();
        }

        private void Begin_Service_Update()
        {
            if(BeginUpdateServiceButton.Text=="Begin Update")
                BeginServiceUpdate();
            else if(BeginUpdateServiceButton.Text=="Cancel Update")
                EndServiceUpdate();
        }

        private void Update_Service()
        {
            if (UpdateServiceNameTextBox.Text.Trim().Length == 0 || UpdateServicePriceTextBox.Text.Trim().Length == 0)
                MessageBox.Show("You have left some field(s) empty.", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            else
                try
                {
                    int id = Convert.ToInt16(UpdateServiceIDUpDown.Value);
                    double Price = Convert.ToDouble(UpdateServicePriceTextBox.Text);
                    Services SERVICES = new Services(new List<string>(), false);
                    SERVICES.UpdateService(id, UpdateServiceNameTextBox.Text, Price);
                    MessageBox.Show("Service has been updated successfully", "Service Update", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    EndServiceUpdate();
                }
                catch
                {
                    MessageBox.Show("Service Price data is incorrect! Decimal num expected like '123,45'", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            ShowServicesTable();
        }

        private void Delete_Income_Record()
        {
            if (MessageBox.Show("Are you sure you want to delete this income record?", "Confirm Deletion",
                MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                HotelIncome HInc = new HotelIncome();
                int id = Convert.ToInt16(IncomeIDUpDown.Value);
                HInc.DeleteIncome(id);
                MessageBox.Show("Successfully deleted", "Delete Income Record", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Deletion cancelled", "Delete Income", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            ShowIncomeTable();
        }

        private void LoadRoomTypes()
        {
            DataTable source = ROOMS.Room_Type();
            RoomTypeComboBox.DataSource = source;
            RoomTypeComboBox.DisplayMember = "type1";
            RoomTypeComboBox.ValueMember = "type1";
        }

        private void BeginRoomUpdate()
        {
            BeginRoomUpdateButton.Text = "Cancel Update";
            UpdateRoomButton.Enabled = true;
            DataTable dt = ROOMS.SelectRoomByNum(Convert.ToInt16(RoomNumTextBox.Text));
            if(dt.Rows.Count>0)
            {
                RoomTypeComboBox.Text = dt.Rows[0][1].ToString();
                RoomFloorUpDown.Value = Convert.ToDecimal(dt.Rows[0][2].ToString());
            }
        }

        private void EndRoomUpdate()
        {
            BeginRoomUpdateButton.Text = "Begin Update";
            UpdateRoomButton.Enabled = false;
            SelectedRoomNum();
        }

        private void Begin_Room_Update()
        {
            if (BeginRoomUpdateButton.Text == "Begin Update")
                BeginRoomUpdate();
            else if (BeginRoomUpdateButton.Text == "Cancel Update")
                EndRoomUpdate();
        }

        private void Update_Room()
        {
            try
            {
                int floor = Convert.ToInt16(RoomFloorUpDown.Value);
                string old_type = ROOMS.GetTypeOfFrrom(Convert.ToInt16(RoomNumTextBox.Text));
                string new_type = RoomTypeComboBox.Text;
                int room_num = Convert.ToInt16(RoomNumTextBox.Text);
                /*[Max number of residents currently living in rooms of old type]>
                  [Max number of residents allowed by room of new type]*/
                if (ROOMS.MaxResidents(old_type, 1) > ROOMS.MaxResidents(new_type, 2))
                    MessageBox.Show("You cannot change room type from '" + old_type + "' to '" + new_type +
                        "' because of max num of residents currently living in rooms of old type is greather than" +
                        "max num of residents allowed by room of new type", "Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                else
                {
                    ROOMS.UpdateRoom(room_num, new_type, floor);
                    MessageBox.Show("Room has been updated successfully", "Room Update", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    EndRoomUpdate();
                }
            }
            catch
            {
                MessageBox.Show("Room floor data is incorrect! Integer num expected", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            ShowRoomTable();
        }

        private void BeginTypeUpdate()
        {
            BeginUpdateTypeButton.Text = "Cancel Update";
            UpdateTypeButton.Enabled = true;
            AddTypeButton.Enabled = false;
            DeleteTypeButton.Enabled = false;
            DataTable dt = ROOMS.SelectType(DeleteTypeTextBox.Text);
            if (dt.Rows.Count > 0)
            {
                UpdateTypeTextBox.Text = dt.Rows[0][0].ToString();
                UpdatePeopleUpDown.Value = Convert.ToDecimal(dt.Rows[0][1].ToString());
                UpdatePrice1TextBox.Text = dt.Rows[0][2].ToString();
                UpdatePriceTextBox.Text = dt.Rows[0][3].ToString();
                UpdateAreaTextBox.Text = dt.Rows[0][4].ToString();
            }
        }

        private void EndTypeUpdate()
        {
            BeginUpdateTypeButton.Text = "Begin Update";
            UpdateTypeButton.Enabled = false;
            AddTypeButton.Enabled = true;
            DeleteTypeButton.Enabled = true;
            ClearTextBoxes(UpdateTypeTextBox, UpdatePrice1TextBox, UpdatePriceTextBox, UpdateAreaTextBox);
            SelectedType();
        }

        private void Begin_Type_Update()
        {
            if (BeginUpdateTypeButton.Text == "Begin Update")
                BeginTypeUpdate();
            else if (BeginUpdateTypeButton.Text == "Cancel Update")
                EndTypeUpdate();
        }

        private void Update_Type()
        {
            try
            {
                string old_type = DeleteTypeTextBox.Text;
                string new_type = UpdateTypeTextBox.Text;
                int people = Convert.ToUInt16(UpdatePeopleUpDown.Value.ToString());
                double price1 = Convert.ToDouble(UpdatePrice1TextBox.Text);
                double price = Convert.ToDouble(UpdatePriceTextBox.Text);
                double area = Convert.ToDouble(UpdateAreaTextBox.Text);
                if (new_type.Trim().Length == 0)
                    MessageBox.Show("Enter type name, please.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    DataTable dt = ROOMS.SelectType(new_type);
                    if (old_type != new_type && dt.Rows.Count > 0)
                        MessageBox.Show("Type with this name already exists.", "Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    else if (ROOMS.MaxResidents(old_type, 1) > people)
                        MessageBox.Show("Max num of residents you assigned is less than max num of residents " +
                            "currently living in rooms of this type.", "Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    else
                    {
                        ROOMS.UpdateType(old_type, new_type, people, price1, price, area);
                        MessageBox.Show("Type has been updated successfully", "Type Update", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                        EndTypeUpdate();
                    }
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Price or area or number of people data is/are incorrect", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            ShowTypeTable();
        }

        private void Add_Type()
        {
            try
            {
                string type = AddTypeTextBox.Text;
                int people = Convert.ToUInt16(AddPeopleUpDown.Value.ToString());
                double price1 = Convert.ToDouble(AddPrice1TextBox.Text);
                double price = Convert.ToDouble(AddPriceTextBox.Text);
                double area = Convert.ToDouble(AddAreaTextBox.Text);
                if (type.Trim().Length == 0)
                    MessageBox.Show("Enter type name, please.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    DataTable dt = ROOMS.SelectType(type);
                    if (dt.Rows.Count > 0)
                        MessageBox.Show("Type with this name already exists.", "Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    else
                    {
                        ROOMS.AddType(type, people, price1, price, area);
                        MessageBox.Show("Type has been added successfully", "Type Add", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Price or area or number of people data is/are incorrect", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            ShowTypeTable();
        }

        private void Delete_Type()
        {
            if (ROOMS.CountRoomsOfThisType(DeleteTypeTextBox.Text)!=0)
                MessageBox.Show("You cannot delete this type because there are rooms of this type.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (MessageBox.Show("Are you sure you want to delete this Type?", "Confirm Deletion",
                MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                ROOMS.DeleteType(DeleteTypeTextBox.Text);
                MessageBox.Show("Successfully deleted", "Delete Type", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Deletion cancelled", "Delete Type", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ShowTypeTable();
        }

        private void OpenAboutBox()
        {
            AboutBox AboutBoxDialog = new AboutBox();
            AboutBoxDialog.ShowDialog();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            PageChanges();
        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogOut();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void SuperUser_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void ServiceTable_SelectionChanged(object sender, EventArgs e)
        {
            SelectedServiceID();
        }

        private void ClearAddServiceBoxesButton_Click(object sender, EventArgs e)
        {
            ClearTextBoxes(AddServiceNameTextBox, AddServicePriceTextBox);
        }

        private void ClearUpdateServiceBoxesButton_Click(object sender, EventArgs e)
        {
            ClearTextBoxes(UpdateServiceNameTextBox, UpdateServicePriceTextBox);
        }

        private void AddServiceButton_Click(object sender, EventArgs e)
        {
            Add_Service();
        }

        private void DeleteServiceButton_Click(object sender, EventArgs e)
        {
            Delete_Service();
        }

        private void BeginUpdateServiceButton_Click(object sender, EventArgs e)
        {
            Begin_Service_Update();
        }

        private void UpdateServiceButton_Click(object sender, EventArgs e)
        {
            Update_Service();
        }

        private void IncomeTable_SelectionChanged(object sender, EventArgs e)
        {
            SelectedIncomeID();
        }

        private void IncomeDelete_Click(object sender, EventArgs e)
        {
            Delete_Income_Record();
        }

        private void typeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeTabControlIndex(0);
        }

        private void roomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeTabControlIndex(1);
        }

        private void serviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeTabControlIndex(2);
        }

        private void incomeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeTabControlIndex(3);
        }

        private void RoomTable_SelectionChanged(object sender, EventArgs e)
        {
            SelectedRoomNum();
        }

        private void BeginRoomUpdateButton_Click(object sender, EventArgs e)
        {
            Begin_Room_Update();
        }

        private void UpdateRoomButton_Click(object sender, EventArgs e)
        {
            Update_Room();
        }

        private void ClearAddTypeBoxesButton_Click(object sender, EventArgs e)
        {
            ClearTextBoxes(AddTypeTextBox, AddPrice1TextBox, AddPriceTextBox, AddAreaTextBox);
        }

        private void ClearUpdateTypeBoxesButton_Click(object sender, EventArgs e)
        {
            ClearTextBoxes(UpdateTypeTextBox, UpdatePrice1TextBox, UpdatePriceTextBox, UpdateAreaTextBox);
        }

        private void TypeTable_SelectionChanged(object sender, EventArgs e)
        {
            SelectedType();
        }

        private void BeginUpdateTypeButton_Click(object sender, EventArgs e)
        {
            Begin_Type_Update();
        }

        private void UpdateTypeButton_Click(object sender, EventArgs e)
        {
            Update_Type();
        }

        private void AddTypeButton_Click(object sender, EventArgs e)
        {
            Add_Type();
        }

        private void DeleteTypeButton_Click(object sender, EventArgs e)
        {
            Delete_Type();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenAboutBox();
        }

        private void changeLoginStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChangeLoginData();
        }
    }
}
