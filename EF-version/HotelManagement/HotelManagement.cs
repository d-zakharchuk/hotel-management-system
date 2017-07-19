using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml.Linq;
using System.Xml.Xsl;

namespace HotelManagement
{
    public partial class HotelManagement : Form
    {
        private HotelDataEF.HotelManagementDatabaseEntities ctx;
        Clients CLIENTS = new Clients();
        Reservation RESERVATIONS = new Reservation();
        Room ROOMS = new Room();
        HotelIncome INCOME = new HotelIncome();
        
        public HotelManagement()
        {
            InitializeComponent();
            RoomTypeComboBoxes();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        public void FixEfProviderServicesProblem()
        {
            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            ctx.Dispose();
        }

        private void Add_Client()
        {
            int ClientsWithThisId = (int)(CLIENTS.CountClientsWithThisId(AddClientIdTextBox.Text));
            if (AddClientIdTextBox.Text.Trim().Length <= 0)
                MessageBox.Show("Enter Client's ID, please.", "Client's ID", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            else if (AddClientLastNameTextBox.Text.Trim().Length <= 0)
                MessageBox.Show("Enter Client's last name, please.", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            else if (AddClientFirstNameTextBox.Text.Trim().Length <= 0)
                MessageBox.Show("Enter Client's first name, please.", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            else if (ClientsWithThisId >= 1)
                MessageBox.Show("The following ID already exists.", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            else if (ClientsWithThisId >= 0)
            {
                CLIENTS.AddClients(AddClientIdTextBox.Text, AddClientFirstNameTextBox.Text,
                    AddClientMiddleNameTextBox.Text, AddClientLastNameTextBox.Text, AddClientDateTimePicker.Text,
                    AddClientTelTextBox.Text, AddClientCountryTextBox.Text);
                MessageBox.Show("Client has been added successfully", "Client Add", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            ShowAllClientsTable();
        }

        private void Update_Client()
        {
            int ClientsWithThisId = CLIENTS.CountClientsWithThisId(UpdateClientIDTextBox.Text);
            if ((UpdateClientLastNameTextBox.Text.Trim().Length == 0) ||
                    (UpdateClientFirstNameTextBox.Text.Trim().Length == 0))
                MessageBox.Show("You have left some field(s) empty.", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            else if (ClientsWithThisId >= 1)
            {
                CLIENTS.UpdateClients(UpdateClientIDTextBox.Text, UpdateClientFirstNameTextBox.Text,
                    UpdateClientMiddleNameTextBox.Text, UpdateClientLastNameTextBox.Text,
                    UpdateClientDateTimePicker.Text, UpdateClientTelTextBox.Text, UpdateClientCountryTextBox.Text);
                MessageBox.Show("Client has been updated successfully.", "Update Client", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else if (ClientsWithThisId == 0)
                MessageBox.Show("No such client to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                MessageBox.Show("An error occured during update.", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            ShowAllClientsTable();
        }

        private void Delete_Client()
        {
            if (DeleteClientIDTextBox.Text.Trim().Length == 0)
                MessageBox.Show("Client's ID field is empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (MessageBox.Show("Are you sure you want to delete this Client?", "Confirm deletion",
                MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                DeleteAllReservationsOfClient(DeleteClientIDTextBox.Text);
                CLIENTS.DeleteClients(DeleteClientIDTextBox.Text);
                MessageBox.Show("Successfully deleted", "Delete Client", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Deletion cancelled", "Delete Client", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            ShowAllClientsTable();
        }

        private void UpdateClientSearch()
        {
            int ClientWithThisId = (CLIENTS.CountClientsWithThisId(UpdateClientIDTextBox.Text));
            if (ClientWithThisId >= 1)
            {
                HotelDataEF.Client ClientToUpdate = (HotelDataEF.Client)
                    (CLIENTS.SelectClientWithThisId(UpdateClientIDTextBox.Text));
                UpdateClientFirstNameTextBox.Text = ClientToUpdate.Client_first_name.ToString();
                UpdateClientMiddleNameTextBox.Text = ClientToUpdate.Client_middle_name.ToString();
                UpdateClientLastNameTextBox.Text = ClientToUpdate.Client_last_name.ToString();
                UpdateClientDateTimePicker.Text = ClientToUpdate.Client_date_of_birth.ToString();
                UpdateClientTelTextBox.Text = ClientToUpdate.Client_tel.ToString();
                UpdateClientCountryTextBox.Text = ClientToUpdate.Client_country.ToString();
            }
            else MessageBox.Show("Client with the following ID does not exist.", "Info", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void DeleteClientSearch()
        {
            IsClientFoundLabel.Text = "";
            int ClientWithThisId = CLIENTS.CountClientsWithThisId(DeleteClientIDTextBox.Text);
            if (ClientWithThisId >= 1)
            {
                IsClientFoundLabel.ForeColor = Color.Green;
                IsClientFoundLabel.Text = "Client with this ID exists.";
            }
            else
            {
                IsClientFoundLabel.ForeColor = Color.Red;
                IsClientFoundLabel.Text = "Client with this ID doesn't exist.";
            }
        }

        private void ClearTextBoxes(params TextBox[] Fields)
        {
            foreach (TextBox tb in Fields)
                tb.Clear();
        }

        private void ConfigureClientTableHeaders()
        {
            AllClientsTable.Columns[0].HeaderText = "ID";
            AllClientsTable.Columns[1].HeaderText = "First name";
            AllClientsTable.Columns[2].HeaderText = "Middle name";
            AllClientsTable.Columns[3].HeaderText = "Last name";
            AllClientsTable.Columns[4].HeaderText = "Date of birth";
            AllClientsTable.Columns[5].HeaderText = "Phone";
            AllClientsTable.Columns[6].HeaderText = "Country";
        }

        private void ShowAllClientsTable()
        {
            ctx = new HotelDataEF.HotelManagementDatabaseEntities();
            ctx.Client.Load();
            this.clientBindingSource.DataSource = ctx.Client.Local.ToBindingList();
            ConfigureClientTableHeaders();
        }

        //Reservation menu

        private void RoomTypeComboBoxes()
        {
            var source = ROOMS.Room_Type();
            AddRoomTypeComboBox.DataSource = source;
            UpdateRoomTypeComboBox.DataSource = source;
            InvoiceRoomTypeComboBox.DataSource = source;
        }

        private void LoadClientsToComboBox()
        {
            var source = (RESERVATIONS.Clients());
            AddClientComboBox.DataSource = source;
            UpdateClientIDComboBox.DataSource = source;
        }

        private void LoadRoomsToComboBox()
        {
            if (AddClientComboBox.SelectedIndex > -1)
            {
                AddRoomNumComboBox.DataSource = ROOMS.Room_by_Type(AddRoomTypeComboBox.Text.ToString(),
                  AddClientComboBox.Text.ToString());
            }
        
            if (UpdateClientIDComboBox.SelectedIndex > -1)
            {
                if(UpdateReservationIDTextBox.Text=="")
                UpdateRoomNumComboBox.DataSource = ROOMS.Room_by_Type(UpdateRoomTypeComboBox.Text.ToString(),
                  UpdateClientIDComboBox.Text.ToString());
                else UpdateRoomNumComboBox.DataSource = ROOMS.Room_by_Type_for_Update(UpdateRoomTypeComboBox.Text.ToString(),
                  UpdateClientIDComboBox.Text.ToString(), UpdateReservationIDTextBox.Text.ToString());
            }
        }

        private void Add_Reservation()
        {
            int CountReservation = RESERVATIONS.CountReservationsWithThisId(AddReservationIDTextBox.Text);
            if (AddReservationIDTextBox.Text.Trim().Length == 0)
                MessageBox.Show("Enter Reservation ID, please.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (CountReservation >= 1)
                MessageBox.Show("Reservation with the following ID already exists.", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            else if (CountReservation >= 0)
            {
                RESERVATIONS.AddReservations(AddReservationIDTextBox.Text, AddClientComboBox.Text,
                    Convert.ToInt16(AddRoomNumComboBox.Text), Convert.ToDateTime(AddDateInPicker.Text));
                RESERVATIONS.DeleteServicesFromReservation(AddReservationIDTextBox.Text);
                for (int i = 0; i < SelectedServicesListBox1.Items.Count; i++)
                {
                    RESERVATIONS.AddServicesForReservation(AddReservationIDTextBox.Text,
                        Convert.ToInt16(SelectedServicesListBox1.Items[i].ToString()));
                }       
                MessageBox.Show("Reservation has been added successfully", "Add Reservation", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            SelectedServicesListBox1.Items.Clear();
            LoadRoomsToComboBox();
            AddReservationButtonEnable();
            ShowAllReservationsTable();
        }

        private void AddReservationButtonEnable()
        {
            if (AddClientComboBox.SelectedIndex < 0 || AddRoomNumComboBox.SelectedIndex < 0)
                AddReservationButton.Enabled = false;
            else AddReservationButton.Enabled = true;
            if(AddClientComboBox.SelectedIndex<0)
                AddRoomNumComboBox.Enabled = false;
            else AddRoomNumComboBox.Enabled = true;
        }

        private void UpdateReservation()
        {
            int ReservationsWithThisId = RESERVATIONS.CountReservationsWithThisId(UpdateReservationIDTextBox.Text);
            if (UpdateReservationIDTextBox.Text.Trim().Length == 0)
                MessageBox.Show("You have left some field(s) empty.", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            else if (ReservationsWithThisId >= 1)
            {
                RESERVATIONS.UpdateReservations(UpdateReservationIDTextBox.Text, UpdateClientIDComboBox.Text,
                    ROOMS.SelectRoomFromReservation(UpdateReservationIDTextBox.Text),
                    Convert.ToInt16(UpdateRoomNumComboBox.Text), Convert.ToDateTime(UpdateDateInPicker.Text));
                RESERVATIONS.DeleteServicesFromReservation(UpdateReservationIDTextBox.Text);
                for (int i = 0; i < SelectedServicesListBox2.Items.Count; i++)
                {
                    RESERVATIONS.AddServicesForReservation(UpdateReservationIDTextBox.Text,
                        Convert.ToInt16(SelectedServicesListBox2.Items[i].ToString()));
                }
                MessageBox.Show("Reservation has been updated successfully", "Update Reservation",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else MessageBox.Show("Reservation was not updated", "Update Reservation",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            SelectedServicesListBox2.Items.Clear();
            LoadRoomsToComboBox();
            ShowAllReservationsTable();
            UpdateReservationButton.Enabled = false;
            UpdateSelectServicesButton.Enabled = false;
        }

        private void DeleteReservation()
        {
            if (DeleteReservationIDTextBox.Text.Trim().Length == 0)
                MessageBox.Show("Enter Reservation ID, please", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if(!IsReservationToDeleteExist())
                MessageBox.Show("Reservation with this ID doesn't exist", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            else if (MessageBox.Show("Are you sure you want to delete this reservation?", "Confirm Deletion",
                MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                RESERVATIONS.DeleteReservations(DeleteReservationIDTextBox.Text, 
                    ROOMS.SelectRoomFromReservation(DeleteReservationIDTextBox.Text));
                MessageBox.Show("Successfully deleted", "Delete Reservation", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                LoadRoomsToComboBox();
            }
            else
                MessageBox.Show("Deletion cancelled", "Delete Reservation", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            ShowAllReservationsTable();
        }

        private void DeleteAllReservationsOfClient(string Client_id)
        {
            var list = (List<string>)(RESERVATIONS.SelectAllReservationsOfClient(Client_id));
            foreach (var item in list)
                RESERVATIONS.DeleteReservations(item.ToString(), ROOMS.SelectRoomFromReservation(item.ToString()));
        }

        private void ConfigureReservationTableHeaders()
        {
            AllReservationsTable.Columns[0].HeaderText = "Res ID";
            AllReservationsTable.Columns[1].HeaderText = "Client ID";
            AllReservationsTable.Columns[2].HeaderText = "Room";
            AllReservationsTable.Columns[3].HeaderText = "Date in";
        }

        private void ShowAllReservationsTable()
        {
            ctx = new HotelDataEF.HotelManagementDatabaseEntities();
            ctx.Reservation.Load();
            this.reservationBindingSource.DataSource = ctx.Reservation.Local.ToBindingList();
            ConfigureReservationTableHeaders();
        }

        private bool IsReservationToDeleteExist()
        {
            int ReservationsWithThisId = RESERVATIONS.CountReservationsWithThisId(DeleteReservationIDTextBox.Text);
            if (ReservationsWithThisId >= 1)
                return true;
            else return false;
        }

        private void UpdateReservationSearch()
        {
            List<HotelDataEF.Reservation> ReservationsList = (List<HotelDataEF.Reservation>)
                (RESERVATIONS.SelectReservationsWithThisId(UpdateReservationIDTextBox.Text));
            if (ReservationsList.Count >= 1)
            {
                HotelDataEF.Reservation DesiredReservation = ReservationsList[0];
                UpdateClientIDComboBox.Text = DesiredReservation.Client_id.ToString();
                UpdateRoomTypeComboBox.Text = ROOMS.GetTypeOfFrrom(Convert.ToInt16(DesiredReservation.Room_num));
                LoadRoomsToComboBox();
                UpdateRoomNumComboBox.Text = DesiredReservation.Room_num.ToString();
                try
                {
                    UpdateDateInPicker.Text = DesiredReservation.Date_in.ToString();
                }
                catch
                {
                    MessageBox.Show("'Date in' date was adjusted to today", "Info", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    UpdateDateInPicker.Value = DateTime.Today;
                }
                List<Services_of_res_query> serv = (List<Services_of_res_query>)
                    (RESERVATIONS.SelectAllServicesOfReservation(DesiredReservation.Res_id));
                SelectedServicesListBox2.Items.Clear();
                for (int i = 0; i < serv.Count; i++)
                    SelectedServicesListBox2.Items.Add(serv[i]._id_serv);
                UpdateReservationButton.Enabled = true;
                UpdateSelectServicesButton.Enabled = true;
            }
            else MessageBox.Show("Reservation with the following ID does not exist.", "Info", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void DeleteReservationSearch()
        {
            IsReservationFoundLabel.Text = "";
            List<HotelDataEF.Reservation> ReservationList = new List<HotelDataEF.Reservation>();
            if (ReservationList.Count >= 1)
            {
                IsReservationFoundLabel.ForeColor = Color.Green;
                IsReservationFoundLabel.Text = "Reservation exists.";
            }
            else
            {
                IsReservationFoundLabel.ForeColor = Color.Red;
                IsReservationFoundLabel.Text = "Reservation doesn't exist.";
            }
        }

        private void OpenServicesDialog(ListBox list, bool mode)
        {

            List<string> data = new List<string>();
            for (int i=0; i<list.Items.Count; i++)
            {
                data.Add(list.Items[i].ToString());
            }
            Services sv = new Services(data, mode);
            sv.Owner = HotelManagement.ActiveForm;
            sv.ShowDialog();
        }

        private void DateTimePickersAdjustment()
        {
            AddClientDateTimePicker.Value = DateTime.Today;
            UpdateClientDateTimePicker.Value = DateTime.Today;
            AddClientDateTimePicker.MaxDate = DateTime.Today.AddDays(2);
            UpdateClientDateTimePicker.MaxDate = DateTime.Today.AddDays(2);
            AddDateInPicker.MinDate = DateTime.Today;
        }

        //Rooms menu

        private  List<Button> Config_List_of_Rooms_Buttons()
        {
            List<Button> LRB = new List<Button>();
            LRB.Add(r101);
            LRB.Add(r102);
            LRB.Add(r103);
            LRB.Add(r104);
            LRB.Add(r105);
            LRB.Add(r106);
            LRB.Add(r107);
            LRB.Add(r108);
            LRB.Add(r109);
            LRB.Add(r110);
            LRB.Add(r111);
            LRB.Add(r201);
            LRB.Add(r202);
            LRB.Add(r203);
            LRB.Add(r204);
            LRB.Add(r205);
            LRB.Add(r206);
            LRB.Add(r207);
            LRB.Add(r208);
            LRB.Add(r209);
            LRB.Add(r210);
            LRB.Add(r301);
            LRB.Add(r302);
            LRB.Add(r303);
            LRB.Add(r304);
            LRB.Add(r305);
            LRB.Add(r306);
            LRB.Add(r307);
            LRB.Add(r308);
            LRB.Add(r309);
            LRB.Add(r310);
            LRB.Add(r311);
            LRB.Add(r401);
            LRB.Add(r402);
            LRB.Add(r403);
            LRB.Add(r404);
            LRB.Add(r405);
            LRB.Add(r406);
            LRB.Add(r407);
            LRB.Add(r408);
            LRB.Add(r409);
            LRB.Add(r410);
            return LRB;
        }
        
        private void RoomButtonsColor()
        {
            List<Button> ZZZ = Config_List_of_Rooms_Buttons();
            foreach( Button btn in ZZZ)
            {
                int NewName = Convert.ToInt16(btn.Name.Substring(1));
                if (ROOMS.CheckRoomState(NewName) == 0)
                    btn.BackColor = Color.SpringGreen;
                else if (ROOMS.CheckRoomState(NewName) == 1)
                    btn.BackColor = Color.Gold;
                else if (ROOMS.CheckRoomState(NewName) == 2)
                    btn.BackColor = Color.LightPink;
            }
        }

        private string ExpandServicesInfo(string res_id)
        {
            string result = "";
            bool IsService = false;
            List<Services_of_res_query> Services_of_Reservation = (List<Services_of_res_query>)
                    (RESERVATIONS.SelectAllServicesOfReservation(res_id));
            List <string> SelectedServices = new List<string>();
            List<HotelDataEF.Services> dt_Serv = new List<HotelDataEF.Services>();
            ctx.Services.Load();
            dt_Serv = (List<HotelDataEF.Services>)(from s in ctx.Services select s).ToList();
            for (int i = 0; i < Services_of_Reservation.Count; i++)
                SelectedServices.Add(Services_of_Reservation[i]._id_serv.ToString());
            for (int i = 0; i < SelectedServices.Count; i++)
            {
                for (int j = 0; j < dt_Serv.Count; j++)
                {
                    if (SelectedServices[i] == dt_Serv[j].Id.ToString())
                    {
                        if (!IsService)
                            IsService = true;
                        result += dt_Serv[j].Service.ToString() + " - " + dt_Serv[j].Price.ToString() + "\n";
                    }
                }

            }
            if (!IsService)
                result += "<No Services>\n";
            result += "\n";
            return result;
        }

        private void ShowRoomInfo(int room_num)
        {
            List<Room1Query> dt1 = (List<Room1Query>)(ROOMS.RoomInfo(room_num, "Room_Info_1"));
            RoomNumLabelInfo.Text = dt1.First()._room_num.ToString();
            RoomFloorLabelInfo.Text = dt1.First()._floor.ToString();
            RoomTypeLabelInfo.Text = dt1.First()._type.ToString();
            NumOfResidentsLabelInfo.Text = dt1.First()._people.ToString();
            MaxResidentsLabelInfo.Text = dt1.First()._C_people.ToString();
            Price1LabelInfo.Text = dt1.First()._price1.ToString();
            PriceLabelInfo.Text = dt1.First()._price.ToString();
            RoomAreaLabelInfo.Text = dt1.First()._area.ToString() + " sq. m.";
            int NumOfResidents = dt1.First()._people;
            var source2 = ROOMS.RoomInfo(room_num, "Room_Info_2");
            List<Room2Query> dt2 = (List<Room2Query>)(ROOMS.RoomInfo(room_num, "Room_Info_2"));
            string ServicesInfo = "";
            if (NumOfResidents==3)
            {
                Resident1GroupBox.Visible = true;
                Resident1GroupBox.Size = new Size(230, 247);
                Resident2GroupBox.Visible = true;
                Resident2GroupBox.Size = new Size(230, 247);
                Resident2GroupBox.Location = new Point(418, 0);
                Resident3GroupBox.Visible = true;
                Resident3GroupBox.Size = new Size(230, 247);
                Resident3GroupBox.Location = new Point(654, 0);
            }
            else if(NumOfResidents==2)
            {
                Resident1GroupBox.Visible = true;
                Resident1GroupBox.Size = new Size(348, 247);
                Resident2GroupBox.Visible = true;
                Resident2GroupBox.Size = new Size(348, 247);
                Resident2GroupBox.Location = new Point(536, 0);
                Resident3GroupBox.Visible = false;
            }
            else if(NumOfResidents==1)
            {
                Resident1GroupBox.Visible = true;
                Resident1GroupBox.Size = new Size(702, 247);
                Resident2GroupBox.Visible = false;
                Resident3GroupBox.Visible = false;
            }
            else if(NumOfResidents==0)
            {
                Resident1GroupBox.Visible = false;
                Resident2GroupBox.Visible = false;
                Resident3GroupBox.Visible = false;
                ServicesInfoButton.Enabled = false;
            }
            if(NumOfResidents>0)
            {
                ServicesInfoButton.Enabled = true;
                ServicesInfo += "Resident 1\n";
                ReservationIDLabelInfo1.Text = dt2[0]._res_id.ToString();
                ServicesInfo += ExpandServicesInfo(ReservationIDLabelInfo1.Text);
                ResidentIDLabelInfo1.Text = dt2[0]._client_id.ToString();
                FirstNameLabelInfo1.Text = dt2[0]._client_first_name.ToString();
                MiddleNameLabelInfo1.Text = dt2[0]._client_middle_name.ToString();
                LastNameLabelInfo1.Text = dt2[0]._client_last_name.ToString();
                DateOfBirthLabelInfo1.Text = Convert.ToDateTime(dt2[0]._client_dob).ToShortDateString();
                TelLabelInfo1.Text = dt2[0]._client_tel.ToString();
                CountryLabelInfo1.Text = dt2[0]._client_country.ToString();
                CheckedInOnLabelInfo1.Text = Convert.ToDateTime(dt2[0]._date_in).ToShortDateString();
            }
            if(NumOfResidents>1)
            {
                ServicesInfo += "Resident 2\n";
                ReservationIDLabelInfo2.Text = dt2[1]._res_id.ToString();
                ServicesInfo += ExpandServicesInfo(ReservationIDLabelInfo2.Text);
                ResidentIDLabelInfo2.Text = dt2[1]._client_id.ToString();
                FirstNameLabelInfo2.Text = dt2[1]._client_first_name.ToString();
                MiddleNameLabelInfo2.Text = dt2[1]._client_middle_name.ToString();
                LastNameLabelInfo2.Text = dt2[1]._client_last_name.ToString();
                DateOfBirthLabelInfo2.Text = Convert.ToDateTime(dt2[1]._client_dob).ToShortDateString();
                TelLabelInfo2.Text = dt2[1]._client_tel.ToString();
                CountryLabelInfo2.Text = dt2[1]._client_country.ToString();
                CheckedInOnLabelInfo2.Text = Convert.ToDateTime(dt2[1]._date_in).ToShortDateString();
            }
            if (NumOfResidents>2)
            {
                ServicesInfo += "Resident 3\n";
                ReservationIDLabelInfo3.Text = dt2[2]._res_id.ToString();
                ServicesInfo += ExpandServicesInfo(ReservationIDLabelInfo3.Text);
                ResidentIDLabelInfo3.Text = dt2[2]._client_id.ToString();
                FirstNameLabelInfo3.Text = dt2[2]._client_first_name.ToString();
                MiddleNameLabelInfo3.Text = dt2[2]._client_middle_name.ToString();
                LastNameLabelInfo3.Text = dt2[2]._client_last_name.ToString();
                DateOfBirthLabelInfo3.Text = Convert.ToDateTime(dt2[2]._client_dob).ToShortDateString();
                TelLabelInfo3.Text = dt2[2]._client_tel.ToString();
                CountryLabelInfo3.Text = dt2[2]._client_country.ToString();
                CheckedInOnLabelInfo3.Text = Convert.ToDateTime(dt2[2]._date_in).ToShortDateString();
            }
            ServicesInfoLabel.Text = ServicesInfo;        
        }

        private void ShowSelectedServices()
        {
            MessageBox.Show(ServicesInfoLabel.Text, "Selected Services Info", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void FreeRoomsCount()
        {
            var source = (List<string>)(ROOMS.Room_Type());
            DataTable dt = new DataTable();
            DataColumn col1 = new DataColumn();
            DataColumn col2 = new DataColumn();
            col1.DataType = Type.GetType("System.String");
            col2.DataType = Type.GetType("System.Int32");
            col1.ColumnName = "Type";
            col2.ColumnName = "";
            dt.Columns.Add(col1);
            dt.Columns.Add(col2);
            foreach (var room_type in source)
            {
                DataRow newrow = dt.NewRow();
                newrow[0] = room_type;
                newrow[1] = ROOMS.FreeRoomsCount(room_type);
                dt.Rows.Add(newrow);
            }
            FreeRoomsTable.DataSource = dt;
        }

        // Invoice menu

        private DataTable PrepareServicesTable()
        {
            DataTable dt = new DataTable("Services");
            DataColumn col1 = new DataColumn();
            DataColumn col2 = new DataColumn();
            DataColumn col3 = new DataColumn();
            DataColumn col4 = new DataColumn();   
            col1.DataType = Type.GetType("System.String");
            col2.DataType = Type.GetType("System.String");
            col3.DataType = Type.GetType("System.String");
            col4.DataType = Type.GetType("System.Double");
            dt.Columns.Add(col1);
            dt.Columns.Add(col2);
            dt.Columns.Add(col3);
            dt.Columns.Add(col4);
            return dt;
        }

        private void PrepareInvoice(int room)
        {
            List<Room1Query> dt1 = (List<Room1Query>)(ROOMS.RoomInfo(room, "Room_Info_1"));
            string RoomNum = dt1[0]._room_num.ToString();
            string RoomFloor = dt1[0]._floor.ToString();
            string RoomType = dt1[0]._type.ToString();
            List<Room2Query> dt2 = (List<Room2Query>)(ROOMS.RoomInfo(room, "Room_Info_2"));
            DateTime DateIn = new DateTime();
            if (dt2.Count == 1)
                DateIn = Convert.ToDateTime(dt2[0]._date_in);
            else
            {
                DateIn = Convert.ToDateTime(dt2[0]._date_in);
                for (int i=1; i<dt2.Count; i++)
                    if (Convert.ToDateTime(dt2[i]._date_in) < DateIn)
                        DateIn = Convert.ToDateTime(dt2[i]._date_in);
            }
            Services SERVICES = new Services(new List<string>(), false);
            DataTable ServicesTable = PrepareServicesTable();
            double TotalServiceCostPerDay = 0;
            for (int i = 0; i < dt2.Count; i++)
                if (Convert.ToDateTime(dt2[i]._date_in) <= DateTime.Today)
                {
                    double ServiceCost = 0;
                    List<Services_of_res_query> Serv_of_res = (List<Services_of_res_query>)
                        (RESERVATIONS.SelectAllServicesOfReservation(dt2[i]._res_id.ToString()));
                    for (int j = 0; j < Serv_of_res.Count; j++)
                    {
                           ServiceCost += Convert.ToDouble(Serv_of_res[j]._price.ToString());
                    }
                    DataRow row = ServicesTable.NewRow();
                    row[0] = dt2[i]._client_first_name;
                    row[1] = dt2[i]._client_middle_name;
                    row[2] = dt2[i]._client_last_name;
                    row[3] = ServiceCost;
                    TotalServiceCostPerDay += ServiceCost;
                    ServicesTable.Rows.Add(row);
                }
            int NumOfResidents = ServicesTable.Rows.Count;
            double PriceOfRoomPerDay = 0;
            if (NumOfResidents == 1)
                PriceOfRoomPerDay = Convert.ToDouble(dt1[0]._price1);
            else if (NumOfResidents > 1)
                PriceOfRoomPerDay = Convert.ToDouble(dt1[0]._price);
            DateTime DateOut = DateTime.Now;
            TimeSpan DateDif = DateOut - DateIn;
            double RoomTotalPrice = 0, ServiceTotalCost = 0;
            if (DateDif.TotalDays > 0)
            {
                RoomTotalPrice = PriceOfRoomPerDay * Convert.ToDouble((DateDif.TotalDays));
                ServiceTotalCost = TotalServiceCostPerDay * Convert.ToDouble((DateDif.TotalDays));
            }
            double TotalCost = RoomTotalPrice + ServiceTotalCost;
            MakeXML(RoomNum, RoomType, RoomFloor, NumOfResidents, PriceOfRoomPerDay, RoomTotalPrice, DateIn, DateOut,
                ServicesTable, TotalServiceCostPerDay, ServiceTotalCost, TotalCost);
        }

        private void GenerateInvoice(int room)
        {
            ExportInvoiceButton.Enabled = true;
            PrepareInvoice(room);
            MakeHTML("test_report.html");
            var outPutDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            var NewPath = Path.Combine(outPutDirectory, "test_report.html");
            webBrowser1.Navigate(new Uri(NewPath).LocalPath);
        }

        private void MakeHTML(string file_html)
        {
            XslCompiledTransform xslt = new XslCompiledTransform();
            string file_xslt = "transform.xslt";
            xslt.Load(file_xslt);
            string file_xml = "temp.xml";
            xslt.Transform(file_xml, file_html);
        }

        private void MakeXML(string RoomNum, string RoomType, string RoomFloor, int Residents, double Price1,
            double Price_t, DateTime DateIn, DateTime DateOut, DataTable Service, double ServicePerDay,
            double ServiceTotal, double Total)
        {
            StreamWriter sw = File.CreateText("temp.xml");
            sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
            sw.WriteLine(@"<Hotel>");
            sw.WriteLine(@"<HotelInvoice1>");
            sw.WriteLine(@"<room_num>" + RoomNum + @"</room_num>");
            sw.WriteLine(@"<room_type>" + RoomType + @"</room_type>");
            sw.WriteLine(@"<room_floor>" + RoomFloor + @"</room_floor>");
            sw.WriteLine(@"<residents>" + Residents + @"</residents>");
            sw.WriteLine(@"<date_in>" + DateIn.ToShortDateString() + @"</date_in>");
            sw.WriteLine(@"<date_out>" + DateOut.ToShortDateString() + @"</date_out>");
            sw.WriteLine(@"<price1>" + Math.Round(Price1, 2) + @"</price1>");
            sw.WriteLine(@"<price_t>" + Math.Round(Price_t, 2) + @"</price_t>");
            sw.WriteLine(@"<serv_day>" + Math.Round(ServicePerDay, 2) + @"</serv_day>");
            sw.WriteLine(@"<serv_total>" + Math.Round(ServiceTotal, 2) + @"</serv_total>");
            sw.WriteLine(@"<total>" + Math.Round(Total, 2) + @"</total>");
            sw.WriteLine(@"</HotelInvoice1>");
            sw.WriteLine(@"<HotelInvoice2>");
            for (int i=0; i<Service.Rows.Count; i++)
            {
                sw.WriteLine(@"<Resident>");
                sw.WriteLine(@"<first_name>" + Service.Rows[i][0] + "</first_name>");
                sw.WriteLine(@"<middle_name>" + Service.Rows[i][1] + "</middle_name>");
                sw.WriteLine(@"<last_name>" + Service.Rows[i][2] + "</last_name>");
                sw.WriteLine(@"<cost>" + Service.Rows[i][3] + "</cost>");
                sw.WriteLine(@"</Resident>");
            }
            sw.WriteLine(@"</HotelInvoice2>");
            sw.Write(@"</Hotel>");
            sw.Close();
        }

        private void LoadReservedRooms()
        {
            if (InvoiceRoomTypeComboBox.SelectedIndex > -1)
                InvoiceRoomNumComboBox.DataSource = ROOMS.Room_by_Type_for_Invoice(InvoiceRoomTypeComboBox.Text);
        }
    
        private void InvoiceButtonsEnable()
        {
            if(InvoiceRoomNumComboBox.SelectedIndex>-1)
            {
                GenerateInvoiceButton.Enabled = true;
                CheckOutButton.Enabled = true;
            }
            else
            {
                GenerateInvoiceButton.Enabled = false;
                CheckOutButton.Enabled = false;
            }
        }

        private void ExportReport()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "HTML File|*.html";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                if (saveFileDialog1.FileName != "")
                {
                    StreamWriter write_text = new StreamWriter(saveFileDialog1.OpenFile());
                    write_text.Write(webBrowser1.DocumentText);
                    write_text.Close();
                }
        }

        private void CheckOutReservations(int room_num)
        {
            PrepareInvoice(room_num);
            var ResForInvoice = (List<HotelDataEF.Reservation>)(RESERVATIONS.ReservationsToDeleteInvoice(room_num));
            List<String> income = LINQ();
            int pr0 = Convert.ToInt16(income[0]);
            int pr1 = Convert.ToInt16(income[1]);
            DateTime pr2 = Convert.ToDateTime(income[2]);
            DateTime pr3 = Convert.ToDateTime(income[3]);
            double pr4 = Convert.ToDouble(income[4]);
            double pr5 = Convert.ToDouble(income[5]);
            double pr6 = Convert.ToDouble(income[6]);
            if(pr1>0)
                INCOME.AddIncome(pr0, pr1, pr2, pr3, pr4, pr5, pr6);
            foreach (var Reservation in ResForInvoice)
                RESERVATIONS.DeleteReservations(Reservation.Res_id, Reservation.Room_num);
            string CheckOutPattern = " people were checked out";
            if (ResForInvoice.Count() == 1)
                CheckOutPattern = " person was checked out";
            MessageBox.Show(ResForInvoice.Count() + CheckOutPattern, "Check out", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private List<String> LINQ()
        {
            List<String> list = new List<String>();
            XDocument doc = XDocument.Load("temp.xml");
            try { doc = XDocument.Load("temp.xml"); }
            catch {  }
            var Elements = from elem in doc.Descendants("HotelInvoice1")
                           select new
                           {
                               room_num = elem.Element("room_num").Value,
                               residents = elem.Element("residents").Value,
                               date_in = elem.Element("date_in").Value,
                               date_out = elem.Element("date_out").Value,
                               room_cost = elem.Element("price_t").Value,
                               services_cost = elem.Element("serv_total").Value,
                               total = elem.Element("total").Value
                           };
            foreach(var elem in Elements)
            {
                list.Add(elem.room_num.ToString());
                list.Add(elem.residents.ToString());
                list.Add(elem.date_in.ToString());
                list.Add(elem.date_out.ToString());
                list.Add(elem.room_cost.ToString());
                list.Add(elem.services_cost.ToString());
                list.Add(elem.total.ToString());
            }
            return list;
        }

        private void ConfigureIncomeTableHeaders()
        {
            HotelIncomeTable.Columns[0].HeaderText = "ID";
            HotelIncomeTable.Columns[1].HeaderText = "Room";
            HotelIncomeTable.Columns[2].HeaderText = "Residents";
            HotelIncomeTable.Columns[3].HeaderText = "Date in";
            HotelIncomeTable.Columns[4].HeaderText = "Date out";
            HotelIncomeTable.Columns[5].HeaderText = "Room cost";
            HotelIncomeTable.Columns[6].HeaderText = "Services cost";
            HotelIncomeTable.Columns[7].HeaderText = "Total";
        }

        private void ShowIncomeTable()
        {
            ctx = new HotelDataEF.HotelManagementDatabaseEntities();
            ctx.HotelIncome.Load();
            this.hotelIncomeBindingSource.DataSource = ctx.HotelIncome.Local.ToBindingList();
            ConfigureIncomeTableHeaders();
        }

        private void MainFormLoad()
        {
            DateTimePickersAdjustment();
            PageChanges();
            ShowAllClientsTable();
        }

        private Color ChartLineColor = Color.Blue;

        private void CollectDataForGraph()
        {
            DateTime CurrentDateTime = DateTime.Now;
            int IncomeParameter = 7;
            if (IncomeCategoryForGraph.SelectedIndex == 0)
                IncomeParameter = 7;
            else if (IncomeCategoryForGraph.SelectedIndex == 1)
                IncomeParameter = 5;
            else if (IncomeCategoryForGraph.SelectedIndex == 2)
                IncomeParameter = 6;
            int month = Month1Combo.SelectedIndex;
            int year = Convert.ToInt32(Year1Combo.SelectedItem);
            List<string> monthsNames = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug",
                "Sep", "Oct", "Nov", "Dec","Jan", "Feb", "Mar", "Apr", "May","Jun", "Jul", "Aug", "Sep", "Oct",
                "Nov", "Dec"};
            double IncomeIncrease = 0;
            TableForGraph.Rows.Clear();
            for (int i = month; i < month + 12; i++)
            {
                if (month == 0)
                    TableForGraph.Rows.Add(monthsNames[i], 0, i);
                else
                {
                    if (i < 12)
                        TableForGraph.Rows.Add(monthsNames[i] + "\n" + (year).ToString(), 0, i);
                    else if (i >= 12)
                        TableForGraph.Rows.Add(monthsNames[i] + "\n" + (year + 1).ToString(), 0, i);
                }
            }
            for (int i = 0; i < TableForGraph.Rows.Count - 1; i++)
            {
                int GetMonthNum = Convert.ToInt32(TableForGraph[2, i].Value);
                GetMonthNum++;
                int current_month = GetMonthNum;
                int current_year = year;
                if (GetMonthNum > 12)
                {
                    current_year++;
                    current_month -= 12;
                }
                DateTime start_date = new DateTime(current_year, current_month, 1);
                int LastDayOfMonth = DateTime.DaysInMonth(current_year, current_month);
                DateTime end_date = new DateTime(current_year, current_month, LastDayOfMonth);
                double IncomeInCurrentMonth = 0;
                for (int j = 0; j < HotelIncomeTable.Rows.Count; j++)
                {
                    DateTime date = Convert.ToDateTime(HotelIncomeTable[4, j].Value);
                    if (date >= start_date && date <= end_date)
                    {
                        double CurrentIncome = Convert.ToDouble(HotelIncomeTable[IncomeParameter, j].Value);
                        IncomeInCurrentMonth += CurrentIncome;
                        IncomeIncrease += CurrentIncome;
                    }
                }
                if (TypeOfGraphCombo.SelectedIndex == 0)
                    TableForGraph[1, i].Value = IncomeIncrease;
                else TableForGraph[1, i].Value = IncomeInCurrentMonth;
            }
        }

        private void MakeIncomeGraph()
        {
            CollectDataForGraph();
            chart1.Titles.Clear();
            try
            {
                string period = "";
                if (Month1Combo.SelectedIndex < 1)
                    period = Year1Combo.SelectedItem.ToString();
                else
                    period = " (" + Month1Combo.SelectedItem.ToString() + " " + Year1Combo.SelectedItem.ToString() +
                        " – " + Month2Combo.SelectedItem.ToString() + " " + Year2Combo.SelectedItem.ToString() + ")";
                chart1.Titles.Add(TypeOfGraphCombo.SelectedItem.ToString() + " " + IncomeCategoryForGraph.SelectedItem.ToString() + period);
                FontFamily font = new FontFamily("Arial");
                chart1.Titles[0].Font = new Font(font, 14, FontStyle.Bold);
            }
            catch
            {

            }
            Axis XA = chart1.ChartAreas[0].AxisX;
            Axis YA = chart1.ChartAreas[0].AxisY;
            XA.Title = "Months";
            YA.Title = "Income";
            chart1.Series.Clear();
            chart1.Series.Add(new Series());
            Series S1 = chart1.Series[0];
            S1.ChartType = SeriesChartType.Line;
            S1.Color = ChartLineColor;
            S1.MarkerStyle = MarkerStyle.Circle;
            if (MarkersCheckBox.Checked)
                S1.MarkerSize = 10;
            else S1.MarkerSize = 0;
            S1.MarkerColor = ChartLineColor;
            chart1.Series[0].BorderWidth = 5;
            for (int i = 0; i < TableForGraph.Rows.Count - 1; i++)
            {
                int p = new int();
                if (ShowYValuesCheckBox.Checked)
                {
                    p = chart1.Series[0].Points.AddXY(TableForGraph[0, i].Value, TableForGraph[1, i].Value);
                    chart1.Series[0].Points[i].Label = TableForGraph[1, i].Value.ToString();
                }
                else
                    p = chart1.Series[0].Points.AddY(TableForGraph[1, i].Value);
                chart1.Series[0].Points[i].AxisLabel = TableForGraph[0, i].Value.ToString();
            }
            chart1.Series[0].IsVisibleInLegend = false;
            XA.Interval = 1;
            YA.LabelStyle.Format = "# ##0 $.";
        }

        private void LoadYearsToPeriodsCombo()
        {
            DateTime MinDate = DateTime.Now;
            for (int i = 0; i < HotelIncomeTable.RowCount; i++)
            {
                DateTime CurrentDate = Convert.ToDateTime(HotelIncomeTable[4, i].Value);
                if (CurrentDate < MinDate)
                    MinDate = CurrentDate;
            }
            int MinYear = MinDate.Year - 1;
            for (int i = MinYear; i <= DateTime.Now.Year; i++)
                Year1Combo.Items.Add(i);
            for (int i = MinYear + 1; i <= DateTime.Now.Year + 1; i++)
                Year2Combo.Items.Add(i);
        }

        private void ChangeGraphBasedOnDate()
        {
            if (Month1Combo.SelectedIndex == 0 && Year1Combo.SelectedIndex == 0)
                Month1Combo.SelectedIndex = 1;
            if (Month1Combo.SelectedIndex > 0)
            {
                Month2Combo.SelectedIndex = Month1Combo.SelectedIndex - 1;
                Year2Combo.SelectedIndex = Year1Combo.SelectedIndex;
            }
            else
            {
                Month2Combo.SelectedIndex = 11;
                Year2Combo.SelectedIndex = Year1Combo.SelectedIndex - 1;
            }
            if (TypeOfGraphCombo.SelectedIndex >= 0 && IncomeCategoryForGraph.SelectedIndex >= 0)
                MakeIncomeGraph();
        }

        private void OpenColorDialogForGraph()
        {
            ColorDialog MyDialog = new ColorDialog();
            MyDialog.AllowFullOpen = true;
            MyDialog.Color = ChartLineColor;
            if (MyDialog.ShowDialog() == DialogResult.OK)
            {
                ChartLineColor = MyDialog.Color;
                MakeIncomeGraph();
            }
        }

        private void OpenSaveDialogForGraph()
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif|Png Image|*.png";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    switch (saveFileDialog1.FilterIndex)
                    {
                        case 1:
                            chart1.SaveImage(myStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                            break;

                        case 2:
                            chart1.SaveImage(myStream, System.Drawing.Imaging.ImageFormat.Bmp);
                            break;

                        case 3:
                            chart1.SaveImage(myStream, System.Drawing.Imaging.ImageFormat.Gif);
                            break;
                        case 4:
                            chart1.SaveImage(myStream, System.Drawing.Imaging.ImageFormat.Png);
                            break;
                    }
                    myStream.Close();
                }
            }
        }

        private void PageChanges()
        {
            if (tabControl1.SelectedIndex == 0)
            {
                ShowAllClientsTable();
                clientsToolStripMenuItem1.Checked = true;
                reservationsToolStripMenuItem1.Checked = false;
                roomsToolStripMenuItem1.Checked = false;
                invoiceToolStripMenuItem.Checked = false;
                hotelIncomeToolStripMenuItem.Checked = false;
            }
            if (tabControl1.SelectedIndex == 1)
            {
                ShowAllReservationsTable();
                LoadClientsToComboBox();
                AddReservationButtonEnable();
                ShowCurrentReservationServices();
                clientsToolStripMenuItem1.Checked = false;
                reservationsToolStripMenuItem1.Checked = true;
                roomsToolStripMenuItem1.Checked = false;
                invoiceToolStripMenuItem.Checked = false;
                hotelIncomeToolStripMenuItem.Checked = false;
            }
            if (tabControl1.SelectedIndex == 2)
            {
                RoomButtonsColor();
                ShowRoomInfo(101);
                FreeRoomsCount();
                clientsToolStripMenuItem1.Checked = false;
                reservationsToolStripMenuItem1.Checked = false;
                roomsToolStripMenuItem1.Checked = true;
                invoiceToolStripMenuItem.Checked = false;
                hotelIncomeToolStripMenuItem.Checked = false;
            }
            if (tabControl1.SelectedIndex==3)
            {
                LoadReservedRooms();
                InvoiceButtonsEnable();
                clientsToolStripMenuItem1.Checked = false;
                reservationsToolStripMenuItem1.Checked = false;
                roomsToolStripMenuItem1.Checked = false;
                invoiceToolStripMenuItem.Checked = true;
                hotelIncomeToolStripMenuItem.Checked = false;
            }
            if (tabControl1.SelectedIndex == 4)
            {
                ShowIncomeTable();
                LoadYearsToPeriodsCombo();
                Month1Combo.SelectedIndex = DateTime.Now.Month;
                Month2Combo.SelectedIndex = DateTime.Now.Month - 1;
                Year1Combo.SelectedIndex = 0;
                Year2Combo.SelectedIndex = 0;
                TypeOfGraphCombo.SelectedIndex = 0;
                IncomeCategoryForGraph.SelectedIndex = 0;
                MakeIncomeGraph();
                clientsToolStripMenuItem1.Checked = false;
                reservationsToolStripMenuItem1.Checked = false;
                roomsToolStripMenuItem1.Checked = false;
                invoiceToolStripMenuItem.Checked = false;
                hotelIncomeToolStripMenuItem.Checked = true;
            }
        }

        private void ChangeTabControlIndex(int index)
        {
            tabControl1.SelectedIndex = index;
            PageChanges();
        }

        private void LogOut()
        {
            base.Dispose();
            Login lg = new Login(false);
            lg.Visible = true;
        }

        private void OpenAboutBox()
        {
            AboutBox AboutBoxDialog = new AboutBox();
            AboutBoxDialog.ShowDialog();
        }

        private void ShowCurrentReservationServices()
        {
            if (AllReservationsTable.Rows.Count>0)
            {
                int CurrentRow = AllReservationsTable.CurrentCell.RowIndex;
                string res_id = AllReservationsTable.Rows[CurrentRow].Cells[0].Value.ToString();
                CurrentReservationServicesGroupBox.Text = "Services of " + res_id;
                List<Services_of_res_query> dt = (List<Services_of_res_query>)
                    (RESERVATIONS.SelectAllServicesOfReservation(res_id));
                DataTable dt2 = new DataTable();
                DataColumn col1 = new DataColumn();
                DataColumn col2 = new DataColumn();
                col1.DataType = Type.GetType("System.String");
                col2.DataType = Type.GetType("System.Double");
                col1.ColumnName = "Service";
                col2.ColumnName = "Price";
                dt2.Columns.Add(col1);
                dt2.Columns.Add(col2);
                foreach (var item in dt)
                {
                    DataRow myrow = dt2.NewRow();
                    myrow[0] = item._service_name;
                    myrow[1] = item._price;
                    dt2.Rows.Add(myrow);
                }
                double TotalPrice = 0;
                for (int i = 0; i < dt2.Rows.Count; i++)
                    TotalPrice += Convert.ToDouble(dt2.Rows[i][1].ToString());
                DataRow row = dt2.NewRow();
                row[0] = "<TOTAL>";
                row[1] = TotalPrice;
                dt2.Rows.Add(row);
                CurrentReservationServicesTable.DataSource = dt2;
                int last = CurrentReservationServicesTable.Rows.Count - 1;
                CurrentReservationServicesTable.Rows[last].DefaultCellStyle.BackColor = Color.Yellow;
            }
        }

        private void HotelManagement_Load(object sender, EventArgs e)
        {
            MainFormLoad();
        }

        private void AddClientButton_Click(object sender, EventArgs e)
        {
            Add_Client();
        }

        private void UpdateClientButton_Click(object sender, EventArgs e)
        {
            Update_Client();
        }

        private void DeleteClientButton_Click(object sender, EventArgs e)
        {
            Delete_Client();
        }

        private void UpdateClientSearchButton_Click(object sender, EventArgs e)
        {
            UpdateClientSearch();
        }

        private void DeleteClientSearchButton_Click(object sender, EventArgs e)
        {
            DeleteClientSearch();
        }

        private void ClearAddBoxesButton_Click(object sender, EventArgs e)
        {
            ClearTextBoxes(AddClientIdTextBox, AddClientFirstNameTextBox, AddClientMiddleNameTextBox, 
                AddClientLastNameTextBox, AddClientTelTextBox, AddClientCountryTextBox);
        }

        private void ClearUpdateBoxesButton_Click(object sender, EventArgs e)
        {
            ClearTextBoxes(UpdateClientIDTextBox, UpdateClientFirstNameTextBox, UpdateClientMiddleNameTextBox,
                UpdateClientLastNameTextBox, UpdateClientTelTextBox, UpdateClientCountryTextBox);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            PageChanges();
        }

        private void AddRoomTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRoomsToComboBox();
        }

        private void AddClientComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRoomsToComboBox();
            AddReservationButtonEnable();
        }

        private void AddRoomNumComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddReservationButtonEnable();
        }

        private void SelectServicesAddButton_Click(object sender, EventArgs e)
        {
            OpenServicesDialog(SelectedServicesListBox1, false);
        }

        private void AddReservationButton_Click(object sender, EventArgs e)
        {
            Add_Reservation();
        }

        private void DeleteReservationButton_Click(object sender, EventArgs e)
        {
            DeleteReservation();
        }

        private void DeleteReservationSearchButon_Click(object sender, EventArgs e)
        {
            DeleteReservationSearch();
        }

        private void UpdateClientIDComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRoomsToComboBox();
            //UpdateReservationButtonEnable();
        }

        private void UpdateRoomTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRoomsToComboBox();
            //UpdateReservationButtonEnable();
        }

        private void UpdateReservationSearchButton_Click(object sender, EventArgs e)
        {
            UpdateReservationSearch();
        }

        private void UpdateSelectServicesButton_Click(object sender, EventArgs e)
        {
            OpenServicesDialog(SelectedServicesListBox2, true);
        }

        private void UpdateReservationButton_Click(object sender, EventArgs e)
        {
            UpdateReservation();
        }

        private void r101_Click(object sender, EventArgs e)
        {
            ShowRoomInfo(101);
        }

        private void r102_Click(object sender, EventArgs e)
        {
            ShowRoomInfo(102);
        }

        private void r103_Click(object sender, EventArgs e)
        {
            ShowRoomInfo(103);
        }

        private void r104_Click(object sender, EventArgs e)
        {
            ShowRoomInfo(104);
        }

        private void r105_Click(object sender, EventArgs e)
        {
            ShowRoomInfo(105);
        }

        private void r106_Click(object sender, EventArgs e)
        {
            ShowRoomInfo(106);
        }

        private void r107_Click(object sender, EventArgs e)
        {
            ShowRoomInfo(107);
        }

        private void r108_Click(object sender, EventArgs e)
        {
            ShowRoomInfo(108);
        }

        private void r109_Click(object sender, EventArgs e)
        {
            ShowRoomInfo(109);
        }

        private void r110_Click(object sender, EventArgs e)
        {
            ShowRoomInfo(110);
        }

        private void r111_Click(object sender, EventArgs e)
        {
            ShowRoomInfo(111);
        }

        private void r201_Click(object sender, EventArgs e)
        {
            ShowRoomInfo(201);
        }

        private void r202_Click(object sender, EventArgs e)
        {
            ShowRoomInfo(202);
        }

        private void r203_Click(object sender, EventArgs e)
        {
            ShowRoomInfo(203);
        }

        private void r204_Click(object sender, EventArgs e)
        {
            ShowRoomInfo(204);
        }

        private void r205_Click(object sender, EventArgs e)
        {
            ShowRoomInfo(205);
        }

        private void r206_Click(object sender, EventArgs e)
        {
            ShowRoomInfo(206);
        }

        private void r207_Click(object sender, EventArgs e)
        {
            ShowRoomInfo(207);
        }

        private void r208_Click(object sender, EventArgs e)
        {
            ShowRoomInfo(208);
        }

        private void r209_Click(object sender, EventArgs e)
        {
            ShowRoomInfo(209);
        }

        private void r210_Click(object sender, EventArgs e)
        {
            ShowRoomInfo(210);
        }

        private void r301_Click(object sender, EventArgs e)
        {
            ShowRoomInfo(301);
        }

        private void r302_Click(object sender, EventArgs e)
        {
            ShowRoomInfo(302);
        }

        private void r303_Click(object sender, EventArgs e)
        {
            ShowRoomInfo(303);
        }

        private void r304_Click(object sender, EventArgs e)
        {
            ShowRoomInfo(304);
        }

        private void r305_Click(object sender, EventArgs e)
        {
            ShowRoomInfo(305);
        }

        private void r306_Click(object sender, EventArgs e)
        {
            ShowRoomInfo(306);
        }

        private void r307_Click(object sender, EventArgs e)
        {
            ShowRoomInfo(307);
        }

        private void r308_Click(object sender, EventArgs e)
        {
            ShowRoomInfo(308);
        }

        private void r309_Click(object sender, EventArgs e)
        {
            ShowRoomInfo(309);
        }

        private void r310_Click(object sender, EventArgs e)
        {
            ShowRoomInfo(310);
        }

        private void r311_Click(object sender, EventArgs e)
        {
            ShowRoomInfo(311);
        }

        private void r401_Click(object sender, EventArgs e)
        {
            ShowRoomInfo(401);
        }

        private void r402_Click(object sender, EventArgs e)
        {
            ShowRoomInfo(402);
        }

        private void r403_Click(object sender, EventArgs e)
        {
            ShowRoomInfo(403);
        }

        private void r404_Click(object sender, EventArgs e)
        {
            ShowRoomInfo(404);
        }

        private void r405_Click(object sender, EventArgs e)
        {
            ShowRoomInfo(405);
        }

        private void r406_Click(object sender, EventArgs e)
        {
            ShowRoomInfo(406);
        }

        private void r407_Click(object sender, EventArgs e)
        {
            ShowRoomInfo(407);
        }

        private void r408_Click(object sender, EventArgs e)
        {
            ShowRoomInfo(408);
        }

        private void r409_Click(object sender, EventArgs e)
        {
            ShowRoomInfo(409);
        }

        private void r410_Click(object sender, EventArgs e)
        {
            ShowRoomInfo(410);
        }

        private void GenerateInvoiceButton_Click(object sender, EventArgs e)
        {
            GenerateInvoice(Convert.ToInt16(InvoiceRoomNumComboBox.Text));
        }

        private void InvoiceRoomTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadReservedRooms();
            InvoiceButtonsEnable();
        }

        private void ExportInvoiceButton_Click(object sender, EventArgs e)
        {
            ExportReport();
        }

        private void CheckOutButton_Click(object sender, EventArgs e)
        {
            CheckOutReservations(Convert.ToInt16(InvoiceRoomNumComboBox.Text));
        }

        private void ServicesInfoButton_Click(object sender, EventArgs e)
        {
            ShowSelectedServices();
        }

        private void HotelManagement_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();  
        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogOut();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void clientsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ChangeTabControlIndex(0);
        }

        private void reservationsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ChangeTabControlIndex(1);
        }

        private void roomsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ChangeTabControlIndex(2);
        }

        private void invoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeTabControlIndex(3);
        }

        private void hotelIncomeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeTabControlIndex(4);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenAboutBox();
        }

        private void AllReservationsTable_SelectionChanged(object sender, EventArgs e)
        {
            ShowCurrentReservationServices();
        }

        private void TypeOfGraphCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            MakeIncomeGraph();
        }

        private void IncomeCategoryForGraph_SelectedIndexChanged(object sender, EventArgs e)
        {
            MakeIncomeGraph();
        }

        private void Month1Combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeGraphBasedOnDate();
        }

        private void Year1Combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeGraphBasedOnDate();
        }

        private void Month2Combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeGraphBasedOnDate();
        }

        private void Year2Combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeGraphBasedOnDate();
        }

        private void ShowColorDialogButton_Click(object sender, EventArgs e)
        {
            OpenColorDialogForGraph();
        }

        private void SaveGraphDialogButton_Click(object sender, EventArgs e)
        {
            OpenSaveDialogForGraph();
        }

        private void ShowYValuesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MakeIncomeGraph();
        }

        private void MarkersCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MakeIncomeGraph();
        }
    }
}
