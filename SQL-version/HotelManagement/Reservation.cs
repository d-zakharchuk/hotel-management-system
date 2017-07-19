using System;
using System.Data;
using System.Data.SqlClient;

namespace HotelManagement
{
    class Reservation
    {
        public DataTable Clients()
        {
            DatabaseMethods dbme = new DatabaseMethods();
            DataTable dt = new DataTable();
            dbme.ConOpen();
            dt = dbme.ReadData("Select_Clients_List", null);
            dbme.ConClose();
            return dt;
        }

        public void AddReservations (string Res_id, string Client_id,  int Room_num, DateTime Date_in)
        {
            DatabaseMethods dbme = new DatabaseMethods();
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@Res_id", SqlDbType.VarChar, 10);
            param[0].Value = Res_id;
            param[1] = new SqlParameter("@Client_id", SqlDbType.VarChar, 10);
            param[1].Value = Client_id;
            param[2] = new SqlParameter("@Room_num", SqlDbType.Int);
            param[2].Value = Room_num;
            param[3] = new SqlParameter("@Date_in", SqlDbType.Date);
            param[3].Value = Date_in;
            //param[4] = new SqlParameter("@Services", SqlDbType.VarChar);
            //param[4].Value = Services;
            dbme.ConOpen();
            dbme.Query("Add_Reservation", param);
            dbme.ConClose();
        }

        public void UpdateReservations (string Res_id, string Client_id, int Old_room, int New_room, DateTime Date_in)
        {
            DatabaseMethods dbme = new DatabaseMethods();
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@Res_id", SqlDbType.VarChar, 10);
            param[0].Value = Res_id;
            param[1] = new SqlParameter("@Client_id", SqlDbType.VarChar, 10);
            param[1].Value = Client_id;
            param[2] = new SqlParameter("@Old_room", SqlDbType.Int);
            param[2].Value = Old_room;
            param[3] = new SqlParameter("@New_room", SqlDbType.Int);
            param[3].Value = New_room;
            param[4] = new SqlParameter("@Date_in", SqlDbType.Date);
            param[4].Value = Date_in;
            dbme.ConOpen();
            dbme.Query("Update_Reservation", param);
            dbme.ConClose();
        }

        public void DeleteReservations(string Res_id, int room)
        {
            DatabaseMethods dbme = new DatabaseMethods();
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@Res_id", SqlDbType.VarChar, 10);
            param[0].Value = Res_id;
            param[1] = new SqlParameter("@Room_num", SqlDbType.Int);
            param[1].Value = room;
            dbme.ConOpen();
            dbme.Query("Delete_Reservation", param);
            dbme.ConClose();
        }

        public DataTable SelectAllReservationsOfClient(string Client_id)
        {
            DatabaseMethods dbme = new DatabaseMethods();
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@Client_id", SqlDbType.VarChar, 10);
            param[0].Value = Client_id;
            DataTable dt = new DataTable();
            dbme.ConOpen();
            dt = dbme.ReadData("Select_All_Reservations_of_a_Given_Client", param);
            dbme.ConClose();
            return dt;
        }

        public DataTable ReservationsToDeleteInvoice(int room_num)
        {
            DatabaseMethods dbme = new DatabaseMethods();
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@Room_num", SqlDbType.Int);
            param[0].Value = room_num;
            param[1] = new SqlParameter("@today", SqlDbType.Date);
            param[1].Value = DateTime.Today;
            DataTable dt = new DataTable();
            dbme.ConOpen();
            dt = dbme.ReadData("Reservations_for_Invoice", param);
            dbme.ConClose();
            return dt;
        }

        public void AddServicesForReservation(string Res_id, int Serv_id)
        {
            DatabaseMethods dbme = new DatabaseMethods();
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@Res_id", SqlDbType.VarChar, 10);
            param[0].Value = Res_id;
            param[1] = new SqlParameter("@Serv_id", SqlDbType.Int);
            param[1].Value = Serv_id;
            dbme.ConOpen();
            dbme.Query("Add_Services_for_Reservation", param);
            dbme.ConClose();
        }

        public void DeleteServicesFromReservation(string Res_id)
        {
            DatabaseMethods dbme = new DatabaseMethods();
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@Res_id", SqlDbType.VarChar, 10);
            param[0].Value = Res_id;
            dbme.ConOpen();
            dbme.Query("Delete_Services_from_Reservation", param);
            dbme.ConClose();
        }

        public DataTable SelectAllServicesOfReservation(string Res_id)
        {
            DatabaseMethods dbme = new DatabaseMethods();
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@Res_id", SqlDbType.VarChar, 10);
            param[0].Value = Res_id;
            DataTable dt = new DataTable();
            dbme.ConOpen();
            dt = dbme.ReadData("Select_All_Services_of_Reservation", param);
            dbme.ConClose();
            return dt;
        }
    }
}
