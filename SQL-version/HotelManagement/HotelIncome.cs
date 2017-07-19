using System;
using System.Data;
using System.Data.SqlClient;

namespace HotelManagement
{
    class HotelIncome
    {
        public void AddIncome(int room_num, int residents, DateTime date_in, DateTime date_out, double r_cost,
            double s_cost, double total)
        {
            DatabaseMethods dbme = new DatabaseMethods();
            SqlParameter[] param = new SqlParameter[7];
            param[0] = new SqlParameter("@room_num", SqlDbType.Int);
            param[0].Value = room_num;
            param[1] = new SqlParameter("@residents", SqlDbType.Int);
            param[1].Value = residents;
            param[2] = new SqlParameter("@date_in", SqlDbType.Date);
            param[2].Value = date_in;
            param[3] = new SqlParameter("@date_out", SqlDbType.Date);
            param[3].Value = date_out;
            param[4] = new SqlParameter("@r_cost", SqlDbType.Float);
            param[4].Value = r_cost;
            param[5] = new SqlParameter("@s_cost", SqlDbType.Float);
            param[5].Value = s_cost;
            param[6] = new SqlParameter("@total", SqlDbType.Float);
            param[6].Value = total;
            dbme.ConOpen();
            dbme.Query("Add_Income", param);
            dbme.ConClose();
        }

        public void DeleteIncome(int id)
        {
            DatabaseMethods dbme = new DatabaseMethods();
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@id", SqlDbType.Int);
            param[0].Value = id;
            dbme.ConOpen();
            dbme.Query("SuperUser_Delete_Income", param);
            dbme.ConClose();
        }
    }
}
