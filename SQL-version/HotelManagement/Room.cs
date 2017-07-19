using System;
using System.Data;
using System.Data.SqlClient;

namespace HotelManagement
{
    class Room
    {
        public DataTable Room_Type()
        {
            DatabaseMethods dbme = new DatabaseMethods();
            DataTable dt = new DataTable();
            dbme.ConOpen();
            dt = dbme.ReadData("Room_Type", null);
            dbme.ConClose();
            return dt;
        }

        public DataTable Room_by_Type(string type, string Client_id)
        {
            DatabaseMethods dbme = new DatabaseMethods();
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@type", SqlDbType.VarChar, 10);
            param[0].Value = type;
            param[1] = new SqlParameter("@Client_id", SqlDbType.VarChar, 10);
            param[1].Value = Client_id;
            DataTable dt = new DataTable();
            dbme.ConOpen();
            dt = dbme.ReadData("Room_by_Type", param);
            dbme.ConClose();
            return dt;
        }

        public DataTable Room_by_Type_for_Update(string type, string Client_id, string Res_id)
        {
            DatabaseMethods dbme = new DatabaseMethods();
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@type", SqlDbType.VarChar, 10);
            param[0].Value = type;
            param[1] = new SqlParameter("@Client_id", SqlDbType.VarChar, 10);
            param[1].Value = Client_id;
            param[2] = new SqlParameter("@Res_id", SqlDbType.VarChar, 10);
            param[2].Value = Res_id;
            DataTable dt = new DataTable();
            dbme.ConOpen();
            dt = dbme.ReadData("Room_by_Type_for_Update", param);
            dbme.ConClose();
            return dt;
        }

        public DataTable Room_by_Type_for_Invoice(string type)
        {
            DatabaseMethods dbme = new DatabaseMethods();
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@type", SqlDbType.VarChar, 10);
            param[0].Value = type;
            DataTable dt = new DataTable();
            dbme.ConOpen();
            dt = dbme.ReadData("Reserved_Rooms", param);
            dbme.ConClose();
            return dt;
        }

        public int SelectRoomFromReservation(string Res_id)
        {
            DatabaseMethods dbme = new DatabaseMethods();
            DataTable dt = new DataTable();
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@Res_id", SqlDbType.VarChar, 10);
            param[0].Value = Res_id;
            dbme.ConOpen();
            dt = dbme.ReadData("Select_Room_from_Reservation", param);
            dbme.ConClose();
            int result = Convert.ToInt16(dt.Rows[0][0]);
            return result;
        }

        public string GetTypeOfFrrom(int room_num)
        {
            DatabaseMethods dbme = new DatabaseMethods();
            DataTable dt = new DataTable();
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@room_num", SqlDbType.Int);
            param[0].Value = room_num;
            dbme.ConOpen();
            dt = dbme.ReadData("Get_Type_of_Room", param);
            dbme.ConClose();
            string result = dt.Rows[0][0].ToString();
            return result;
        }

        public int CheckRoomState(int room_num)
        {
            DatabaseMethods dbme = new DatabaseMethods();
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@room_num", SqlDbType.Int);
            param[0].Value = room_num;
            dbme.ConOpen();
            int result = Convert.ToInt16(dbme.ReturnValue("Room_State", param));
            dbme.ConClose();
            return result;
        }

        public DataTable RoomInfo(int room_num, string procedure)
        {
            DatabaseMethods dbme = new DatabaseMethods();
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@Room_num", SqlDbType.VarChar, 10);
            param[0].Value = room_num;
            DataTable dt = new DataTable();
            dbme.ConOpen();
            dt = dbme.ReadData(procedure, param);
            dbme.ConClose();
            return dt;
        }

        public int FreeRoomsCount(string type)
        {
            DatabaseMethods dbme = new DatabaseMethods();
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@t", SqlDbType.VarChar, 10);
            param[0].Value = type;
            //DataTable dt = new DataTable();
            dbme.ConOpen();
            int result  = Convert.ToInt32(dbme.ReturnValue("Free_Rooms", param));
            dbme.ConClose();
            //int result = Convert.ToInt32(dt.Rows[0][0].ToString());
            return result;
        }

        public DataTable SelectRoomByNum(int room)
        {
            DatabaseMethods dbme = new DatabaseMethods();
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@room_num", SqlDbType.Int);
            param[0].Value = room;
            DataTable dt = new DataTable();
            dbme.ConOpen();
            dt = dbme.ReadData("SuperUser_Select_Room_by_Num", param);
            dbme.ConClose();
            return dt;
        }

        /*public bool MaxResidentsCompareChangingRoomType(string old_type, string new_type)
        {
            DatabaseMethods dbme = new DatabaseMethods();
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@old_type", SqlDbType.VarChar, 10);
            param[0].Value = old_type;
            param[1] = new SqlParameter("@new_type", SqlDbType.VarChar, 10);
            param[1].Value = new_type;
            dbme.ConOpen();
            bool result = Convert.ToBoolean(dbme.ReturnValue("SuperUser_MaxResidentsCompareChangingRoomType", param));
            dbme.ConClose();
            return result;
        }*/
        
        public int MaxResidents(string type, int request)
        {
            DatabaseMethods dbme = new DatabaseMethods();
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@type", SqlDbType.VarChar, 10);
            param[0].Value = type;
            string procedure = "SuperUser_MaxResidents_GivenType";
            if (request == 1)
                procedure = "SuperUser_MaxResidentsLivingInRooms_ofGivenType";
            dbme.ConOpen();
            int result = Convert.ToInt16(dbme.ReturnValue(procedure, param));
            dbme.ConClose();
            return result;
        }

        public void UpdateRoom(int room_num, string new_type, int floor)
        {
            DatabaseMethods dbme = new DatabaseMethods();
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@room_num", SqlDbType.Int);
            param[0].Value = room_num;
            param[1] = new SqlParameter("@new_type", SqlDbType.VarChar, 10);
            param[1].Value = new_type;
            param[2] = new SqlParameter("@floor", SqlDbType.Int);
            param[2].Value = floor;
            dbme.ConOpen();
            dbme.Query("SuperUser_Update_Room", param);
            dbme.ConClose();
        }

        public void AddType(string type, int people, double price1, double price, double area)
        {
            DatabaseMethods dbme = new DatabaseMethods();
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@type", SqlDbType.VarChar, 10);
            param[0].Value = type;
            param[1] = new SqlParameter("@people", SqlDbType.Int);
            param[1].Value = people;
            param[2] = new SqlParameter("@price1", SqlDbType.Float);
            param[2].Value = price1;
            param[3] = new SqlParameter("@price", SqlDbType.Float);
            param[3].Value = price;
            param[4] = new SqlParameter("@area", SqlDbType.Float);
            param[4].Value = area;
            dbme.ConOpen();
            dbme.Query("SuperUser_Add_Type", param);
            dbme.ConClose();
        }

        public void UpdateType(string old_type, string new_type, int people, double price1, double price, double area)
        {
            DatabaseMethods dbme = new DatabaseMethods();
            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@old_type", SqlDbType.VarChar, 10);
            param[0].Value = old_type;
            param[1] = new SqlParameter("@new_type", SqlDbType.VarChar, 10);
            param[1].Value = new_type;
            param[2] = new SqlParameter("@people", SqlDbType.Int);
            param[2].Value = people;
            param[3] = new SqlParameter("@price1", SqlDbType.Float);
            param[3].Value = price1;
            param[4] = new SqlParameter("@price", SqlDbType.Float);
            param[4].Value = price;
            param[5] = new SqlParameter("@area", SqlDbType.Float);
            param[5].Value = area;
            dbme.ConOpen();
            dbme.Query("SuperUser_Update_Type", param);
            dbme.ConClose();
        }

        public DataTable SelectType(string type)
        {
            DatabaseMethods dbme = new DatabaseMethods();
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@type", SqlDbType.VarChar, 10);
            param[0].Value = type;
            DataTable dt = new DataTable();
            dbme.ConOpen();
            dt = dbme.ReadData("SuperUser_Select_Type", param);
            dbme.ConClose();
            return dt;
        }

        public int CountRoomsOfThisType(string type)
        {
            DatabaseMethods dbme = new DatabaseMethods();
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@type", SqlDbType.VarChar, 10);
            param[0].Value = type;
            dbme.ConOpen();
            int result = Convert.ToInt16(dbme.ReturnValue("SuperUser_CountRooms_ofGivenType", param));
            dbme.ConClose();
            return result;
        }

        public void DeleteType(string type)
        {
            DatabaseMethods dbme = new DatabaseMethods();
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@type", SqlDbType.VarChar, 10);
            param[0].Value = type;
            dbme.ConOpen();
            dbme.Query("SuperUser_Delete_Type", param);
            dbme.ConClose();
        }
    }
}
