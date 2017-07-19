using System;
using System.Data;
using System.Windows.Forms;

namespace HotelManagement
{
    public partial class Search : Form
    {
        DatabaseMethods dbme = new DatabaseMethods();
        Room ROOMS = new Room();
        Reservation RESERVATIONS = new Reservation();
        public Search()
        {
            InitializeComponent();
        }

        private void Search1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 3;
            comboBox2.SelectedIndex = 3;
            comboBox4.SelectedIndex = 0;
            comboBox5.SelectedIndex = 0;
            comboBox6.SelectedIndex = 0;
            comboBox7.SelectedIndex = 0;
            comboBox8.SelectedIndex = 0;
            comboBox9.SelectedIndex = 0;
            comboBox10.SelectedIndex = 0;
            comboBox13.SelectedIndex = 0;
            comboBox14.SelectedIndex = 0;
            comboBox15.SelectedIndex = 0;
            comboBox11.SelectedIndex = 3;
            comboBox12.SelectedIndex = 3;
            LoadClientsToComboBox();
            RoomTypeComboBoxes();
        }

        private string ComposeClientSearch()
        {
            string id = AddClientIdTextBox.Text;
            string first_name = AddClientFirstNameTextBox.Text;
            string middle_name = AddClientMiddleNameTextBox.Text;
            string last_name = AddClientLastNameTextBox.Text;
            string tel = AddClientTelTextBox.Text;
            string country = AddClientCountryTextBox.Text;
            DateTime date1 = AddClientDateTimePicker.Value;
            DateTime date2 = dateTimePicker1.Value;
            int date_param = comboBox1.SelectedIndex;
            string where_statement = "";
            if (id.Trim() == "")
                where_statement += " Client_id is not null ";
            else where_statement += " Client_id LIKE'%" + id + "%'";
            where_statement += " AND ";
            if (first_name.Trim() == "")
                where_statement += " Client_first_name is not null ";
            else where_statement += " Client_first_name LIKE'%" + first_name + "%'";
            where_statement += " AND ";
            if (middle_name.Trim() == "")
                where_statement += " Client_middle_name is not null ";
            else where_statement += " Client_middle_name LIKE'%" + middle_name + "%'";
            where_statement += " AND ";
            if (last_name.Trim() == "")
                where_statement += " Client_last_name is not null ";
            else where_statement += " Client_last_name LIKE'%" + last_name + "%'";
            where_statement += " AND ";
            if (tel.Trim() == "")
                where_statement += " Client_tel is not null ";
            else where_statement += " Client_tel LIKE'%" + tel + "%'";
            where_statement += " AND ";
            if (country.Trim() == "")
                where_statement += " Client_country is not null ";
            else where_statement += " Client_country LIKE'%" + country + "%'";
            if (date_param < 3)
                where_statement += " AND ";
            if (date_param == 0)
                where_statement += " Client_date_of_birth>=Convert(datetime, '" + date1.ToString() + "', 104)";
            else if (date_param == 1)
                where_statement += " Client_date_of_birth<=Convert(datetime, '" + date1.ToString() + "', 104)";
            else if (date_param == 2)
                where_statement += " Client_date_of_birth>=Convert(datetime, '" + date1.ToString() +
                    "', 104) AND Client_date_of_birth<=Convert(datetime, '" + date2.ToString() + "', 104)";
            if (where_statement == "")
                where_statement = "1";
            string Client_statement = "Select * from Client where " + where_statement;
            return Client_statement;
        }

        private string ComposeReservationSearch()
        {
            try
            {
                string Res_id = AddReservationIDTextBox.Text;
                string Client_id = AddClientComboBox.Text;
                string Room_type = AddRoomTypeComboBox.Text;
                string strnum = RoomNumTextBox.Text;
                int RoomNum = 0;
                if (strnum.Trim() != "")
                    RoomNum = Convert.ToInt16(RoomNumTextBox.Text);
                DateTime date1 = AddDateInPicker.Value;
                DateTime date2 = dateTimePicker2.Value;
                int date_param = comboBox2.SelectedIndex;
                string where_statement = "";
                if (Res_id.Trim() == "")
                    where_statement += " Res_id is not null ";
                else where_statement += " Res_id LIKE'%" + Res_id + "%'";
                where_statement += " AND ";
                if (AddClientComboBox.SelectedIndex < 0 || checkBox1.Checked == false)
                    where_statement += " Client_id is not null ";
                else where_statement += " Client_id LIKE'%" + Client_id + "%'";
                where_statement += " AND ";
                if (AddRoomTypeComboBox.SelectedIndex < 0 || checkBox2.Checked == false)
                    where_statement += " Room_num is not null ";
                else where_statement += " Room_num in(select Room.room_num from Room where Room.type='"
                        + Room_type + "')";
                where_statement += " AND ";
                if (RoomNum == 0)
                    where_statement += " Room_num is not null ";
                else where_statement += " Room_num LIKE'%" + RoomNum + "%'";
                if (date_param < 3)
                    where_statement += " AND ";
                if (date_param == 0)
                    where_statement += " Date_in>=Convert(datetime, '" + date1.ToString() + "', 104)";
                else if (date_param == 1)
                    where_statement += " Date_in<=Convert(datetime, '" + date1.ToString() + "', 104)";
                else if (date_param == 2)
                    where_statement += " Date_in>=Convert(datetime, '" + date1.ToString() +
                        "', 104) AND Date_in<=Convert(datetime, '" + date2.ToString() + "', 104)";
                if (where_statement == "")
                    where_statement = "1";
                string Reservation_statement = "Select Res_id, Client_id, Room_num, Date_in from Reservation where " +
                    where_statement;
                return Reservation_statement;
            }
            catch
            {
                MessageBox.Show("Room number must be integer", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "0";
            }
        }

        private string ComposeRoomSearch()
        {
            try
            {
                string Room_type = comboBox3.Text;
                string strnum = textBox1.Text;
                int RoomNum = 0;
                if (strnum.Trim() != "")
                    RoomNum = Convert.ToInt16(textBox1.Text);
                int floor_param = comboBox4.SelectedIndex;
                int people_param = comboBox5.SelectedIndex;
                int floor = Convert.ToInt16(numericUpDown1.Value);
                int people = Convert.ToInt16(numericUpDown2.Value);
                string where_statement = "";
                if (comboBox3.SelectedIndex < 0 || checkBox3.Checked == false)
                    where_statement += " room_num is not null ";
                else where_statement += " room_num in(select Room.room_num from Room where Room.type='" +
                        Room_type + "')";
                where_statement += " AND ";
                if (RoomNum == 0)
                    where_statement += " room_num is not null ";
                else where_statement += " room_num LIKE'%" + RoomNum + "%'";
                where_statement += " AND ";
                if (floor_param == 0)
                    where_statement += " floor=" + floor;
                else if (floor_param == 1)
                    where_statement += " floor<>" + floor;
                else if (floor_param == 2)
                    where_statement += " floor>=" + floor;
                else if (floor_param == 0)
                    where_statement += " floor<=" + floor;
                where_statement += " AND ";
                if (people_param == 0)
                    where_statement += " people=" + people;
                else if (people_param == 1)
                    where_statement += " people<>" + people;
                else if (people_param == 2)
                    where_statement += " people>=" + people;
                else if (people_param == 0)
                    where_statement += " people<=" + people;
                if (where_statement == "")
                    where_statement = "1";
                string Room_statement = "Select * from Room where " + where_statement;
                return Room_statement;
            }
            catch
            {
                MessageBox.Show("Room number must be integer", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "0";
            }
        }

        private string ComposeServicesSearch()
        {
            try
            {
                string ServiceName = textBox2.Text;
                string pricenum = textBox3.Text;
                int Price = 0;
                if (pricenum.Trim() != "")
                    Price = Convert.ToInt16(textBox3.Text);
                string res_id = textBox4.Text;
                string where_statement = "";
                if (ServiceName.Trim() == "")
                    where_statement += " Service is not null ";
                else where_statement += " Service LIKE'%" + ServiceName + "%'";
                where_statement += " AND ";
                if (Price <= 0)
                    where_statement += " Price is not null ";
                else where_statement += " Price LIKE'%" + Price + "%'";
                where_statement += " AND ";
                if (res_id.Trim() == "")
                    where_statement += " Service is not null ";
                else where_statement += " Services.Id in (select distinct Res_serv.Id_serv from Res_serv where " +
                        "Res_serv.Id_res LIKE'%" + res_id + "%')";
                if (where_statement == "")
                    where_statement = "1";
                string Service_statement = "Select * from Services where " + where_statement;
                return Service_statement;
            }
            catch
            {
                MessageBox.Show("Room number must be integer", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "0";
            }
        }

        private string ComposeTypeSearch()
        {
            try
            {
                string TypeName = textBox5.Text;
                int MaxPeopleParam = comboBox6.SelectedIndex;
                string MaxPeopleStr = textBox6.Text;
                int MaxPeople = -1;
                int AreaParam = comboBox7.SelectedIndex;
                string AreaStr = textBox7.Text;
                double Area = -1;
                int Price1Param = comboBox8.SelectedIndex;
                string Price1Str = textBox8.Text;
                double Price1 = -1;
                int PriceParam = comboBox9.SelectedIndex;
                string PriceStr = textBox9.Text;
                double Price = -1;
                if (MaxPeopleStr.Trim() != "")
                    MaxPeople = Convert.ToInt16(MaxPeopleStr);
                if (AreaStr.Trim() != "")
                    Area = Convert.ToDouble(AreaStr);
                string res_id = textBox4.Text;
                string where_statement = "";
                if (TypeName.Trim() == "")
                    where_statement += " type1 is not null ";
                else where_statement += " type1 LIKE'%" + TypeName + "%'";
                where_statement += " AND ";
                if (MaxPeople <= -1)
                    where_statement += " C_people is not null ";
                else if (MaxPeopleParam == 0)
                    where_statement += " C_people=" + MaxPeople;
                else if (MaxPeopleParam == 1)
                    where_statement += " C_people<>" + MaxPeople;
                else if (MaxPeopleParam == 2)
                    where_statement += " C_people>=" + MaxPeople;
                else if (MaxPeopleParam == 3)
                    where_statement += " C_people<=" + MaxPeople;
                where_statement += " AND ";
                if (Area <= -1)
                    where_statement += " Area is not null ";
                else if (AreaParam == 0)
                    where_statement += " Area=" + Area;
                else if (AreaParam == 1)
                    where_statement += " Area<>" + Area;
                else if (AreaParam == 2)
                    where_statement += " Area>=" + Area;
                else if (AreaParam == 3)
                    where_statement += " Area<=" + Area;
                where_statement += " AND ";
                if (Price <= -1)
                    where_statement += " Price is not null ";
                else if (PriceParam == 0)
                    where_statement += " Price=" + Price;
                else if (PriceParam == 1)
                    where_statement += " Price<>" + Price;
                else if (PriceParam == 2)
                    where_statement += " Price>=" + Price;
                else if (PriceParam == 3)
                    where_statement += " Price<=" + Price;
                where_statement += " AND ";
                if (Price1 <= -1)
                    where_statement += " Price_1 is not null ";
                else if (Price1Param == 0)
                    where_statement += " Price_1=" + Price1;
                else if (Price1Param == 1)
                    where_statement += " Price_1<>" + Price1;
                else if (Price1Param == 2)
                    where_statement += " Price_1>=" + Price1;
                else if (Price1Param == 3)
                    where_statement += " Price_1<=" + Price1;
                if (where_statement == "")
                    where_statement = "1";
                string Type_statement = "Select * from Type where " + where_statement;
                return Type_statement;
            }
            catch
            {
                MessageBox.Show("Error occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "0";
            }
        }

        private string ComposeIncomeSearch()
        {
            try
            {
                string RoomStr = textBox10.Text;
                int RoomNum = 0;
                int ResidentsParam = comboBox10.SelectedIndex;
                string ResidentsStr = textBox11.Text;
                int Residents = -1;
                int RoomCostParam = comboBox13.SelectedIndex;
                string RoomCostStr = textBox12.Text;
                double RoomCost = -1;
                int ServicesCostParam = comboBox14.SelectedIndex;
                string ServicesCostStr = textBox13.Text;
                double ServicesCost = -1;
                int TotalCostParam = comboBox15.SelectedIndex;
                string TotalCostStr = textBox14.Text;
                double TotalCost = -1;
                DateTime date1 = dateTimePicker4.Value;
                DateTime date2 = dateTimePicker3.Value;
                int date_param1 = comboBox11.SelectedIndex;
                DateTime date3 = dateTimePicker6.Value;
                DateTime date4 = dateTimePicker5.Value;
                int date_param2 = comboBox12.SelectedIndex;
                if (RoomStr.Trim() != "")
                    RoomNum = Convert.ToInt16(RoomStr);
                if (RoomCostStr.Trim() != "")
                    RoomCost = Convert.ToDouble(RoomCostStr);
                if (ServicesCostStr.Trim() != "")
                    ServicesCost = Convert.ToDouble(ServicesCostStr);
                if (TotalCostStr.Trim() != "")
                    TotalCost = Convert.ToDouble(TotalCostStr);
                string where_statement = "";
                if (RoomStr.Trim() == "")
                    where_statement += " Room_num is not null ";
                else where_statement += " Room_num LIKE'%" + RoomNum + "%'";
                where_statement += " AND ";
                if (Residents <= -1)
                    where_statement += " Residents is not null ";
                else if (ResidentsParam == 0)
                    where_statement += " Residents=" + Residents;
                else if (ResidentsParam == 1)
                    where_statement += " Residents<>" + Residents;
                else if (ResidentsParam == 2)
                    where_statement += " Residents>=" + Residents;
                else if (ResidentsParam == 3)
                    where_statement += " Residents<=" + Residents;
                where_statement += " AND ";
                if (RoomCost <= -1)
                    where_statement += " Room_cost is not null ";
                else if (RoomCostParam == 0)
                    where_statement += " Room_cost=" + RoomCost;
                else if (RoomCostParam == 1)
                    where_statement += " Room_cost<>" + RoomCost;
                else if (RoomCostParam == 2)
                    where_statement += " Room_cost>=" + RoomCost;
                else if (RoomCostParam == 3)
                    where_statement += " Room_cost<=" + RoomCost;
                where_statement += " AND ";
                if (ServicesCost <= -1)
                    where_statement += " Services_cost is not null ";
                else if (ServicesCostParam == 0)
                    where_statement += " Services_cost=" + ServicesCost;
                else if (ServicesCostParam == 1)
                    where_statement += " Services_cost<>" + ServicesCost;
                else if (ServicesCostParam == 2)
                    where_statement += " Services_cost>=" + ServicesCost;
                else if (ServicesCostParam == 3)
                    where_statement += " Services_cost<=" + ServicesCost;
                where_statement += " AND ";
                if (TotalCost <= -1)
                    where_statement += " Total is not null ";
                else if (TotalCostParam == 0)
                    where_statement += " Total=" + TotalCost;
                else if (TotalCostParam == 1)
                    where_statement += " Total<>" + TotalCost;
                else if (TotalCostParam == 2)
                    where_statement += " Total>=" + TotalCost;
                else if (TotalCostParam == 3)
                    where_statement += " Total<=" + TotalCost;
                if (date_param1 < 3)
                    where_statement += " AND ";
                if (date_param1 == 0)
                    where_statement += " Date_in>=Convert(datetime, '" + date1.ToString() + "', 104)";
                else if (date_param1 == 1)
                    where_statement += " Date_in<=Convert(datetime, '" + date1.ToString() + "', 104)";
                else if (date_param1 == 2)
                    where_statement += " Date_in>=Convert(datetime, '" + date1.ToString() +
                        "', 104) AND Date_in<=Convert(datetime, '" + date2.ToString() + "', 104)";
                if (date_param2 < 3)
                    where_statement += " AND ";
                if (date_param2 == 0)
                    where_statement += " Date_out>=Convert(datetime, '" + date3.ToString() + "', 104)";
                else if (date_param2 == 1)
                    where_statement += " Date_out<=Convert(datetime, '" + date3.ToString() + "', 104)";
                else if (date_param2 == 2)
                    where_statement += " Date_out>=Convert(datetime, '" + date3.ToString() +
                        "', 104) AND Date_out<=Convert(datetime, '" + date4.ToString() + "', 104)";
                if (where_statement == "")
                    where_statement = "1";
                string Income_statement = "Select * from HotelIncome where " + where_statement;
                return Income_statement;
            }
            catch
            {
                MessageBox.Show("Error occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "0";
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex < 2)
                dateTimePicker1.Enabled = false;
            else dateTimePicker1.Enabled = true;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex < 2)
                dateTimePicker2.Enabled = false;
            else dateTimePicker2.Enabled = true;
        }

        private void UpdateClientSearchButton_Click(object sender, EventArgs e)
        {
            SearchClients.DataSource = dbme.ExecuteSQL(ComposeClientSearch());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string criteria = ComposeReservationSearch();
            if (criteria != "0")
                SearchReservationsTable.DataSource = dbme.ExecuteSQL(criteria);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                AddClientComboBox.Enabled = true;
            else AddClientComboBox.Enabled = false;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
                AddRoomTypeComboBox.Enabled = true;
            else AddRoomTypeComboBox.Enabled = false;
        }

        private void RoomTypeComboBoxes()
        {
            DataTable source = ROOMS.Room_Type();
            AddRoomTypeComboBox.DataSource = source;
            AddRoomTypeComboBox.DisplayMember = "type1";
            AddRoomTypeComboBox.ValueMember = "type1";
            comboBox3.DataSource = source;
            comboBox3.DisplayMember = "type1";
            comboBox3.ValueMember = "type1";
        }

        private void LoadClientsToComboBox()
        {
            AddClientComboBox.DataSource = RESERVATIONS.Clients();
            AddClientComboBox.DisplayMember = "Client_id";
            AddClientComboBox.ValueMember = "Client_id";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string criteria = ComposeRoomSearch();
            if (criteria != "0")
                RoomSearch.DataSource = dbme.ExecuteSQL(criteria);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string criteria = ComposeServicesSearch();
            if (criteria != "0")
                ServicesSearch.DataSource = dbme.ExecuteSQL(criteria);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string criteria = ComposeTypeSearch();
            if (criteria != "0")
                TypesSearch.DataSource = dbme.ExecuteSQL(criteria);
        }

        private void comboBox11_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox11.SelectedIndex < 2)
                dateTimePicker3.Enabled = false;
            else dateTimePicker3.Enabled = true;
        }

        private void comboBox12_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox12.SelectedIndex < 2)
                dateTimePicker5.Enabled = false;
            else dateTimePicker5.Enabled = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string criteria = ComposeIncomeSearch();
            if (criteria != "0")
                HotelIncomeTable.DataSource = dbme.ExecuteSQL(criteria);
        }
    }
}
