using System.Data;
using System.Data.SqlClient;

namespace HotelManagement
{
    class Clients
    {
        public void AddClients(string Client_id, string Client_first_name, string Client_middle_name,
            string Client_last_name, string Client_date_of_birth, string Client_tel, string Client_country)
        {
            DatabaseMethods dbme = new DatabaseMethods();
            SqlParameter[] param = new SqlParameter[7];
            param[0] = new SqlParameter("@Client_id", SqlDbType.VarChar, 10);
            param[0].Value = Client_id;
            param[1] = new SqlParameter("@Client_first_name", SqlDbType.VarChar, 30);
            param[1].Value = Client_first_name;
            param[2] = new SqlParameter("@Client_middle_name", SqlDbType.VarChar, 30);
            param[2].Value = Client_middle_name;
            param[3] = new SqlParameter("@Client_last_name", SqlDbType.VarChar, 30);
            param[3].Value = Client_last_name;
            param[4] = new SqlParameter("@Client_date_of_birth", SqlDbType.Date);
            param[4].Value = Client_date_of_birth;
            param[5] = new SqlParameter("@Client_tel", SqlDbType.VarChar, 30);
            param[5].Value = Client_tel;
            param[6] = new SqlParameter("@Client_country", SqlDbType.VarChar, 30);
            param[6].Value = Client_country;
            dbme.ConOpen();
            dbme.Query("Add_Client", param);
            dbme.ConClose();
        }

        public void DeleteClients(string Client_id)
        {
            DatabaseMethods dbme = new DatabaseMethods();
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@Client_id", SqlDbType.VarChar, 10);
            param[0].Value = Client_id;
            dbme.ConOpen();
            dbme.Query("Delete_Client", param);
            dbme.ConClose();
        }

        public void UpdateClients(string Client_id, string Client_first_name, string Client_middle_name,
            string Client_last_name, string Client_date_of_birth, string Client_tel, string Client_country)
        {
            DatabaseMethods dbme = new DatabaseMethods();
            SqlParameter[] param = new SqlParameter[7];
            param[0] = new SqlParameter("@Client_id", SqlDbType.VarChar, 10);
            param[0].Value = Client_id;
            param[1] = new SqlParameter("@Client_first_name", SqlDbType.VarChar, 30);
            param[1].Value = Client_first_name;
            param[2] = new SqlParameter("@Client_middle_name", SqlDbType.VarChar, 30);
            param[2].Value = Client_middle_name;
            param[3] = new SqlParameter("@Client_last_name", SqlDbType.VarChar, 30);
            param[3].Value = Client_last_name;
            param[4] = new SqlParameter("@Client_date_of_birth", SqlDbType.Date);
            param[4].Value = Client_date_of_birth;
            param[5] = new SqlParameter("@Client_tel", SqlDbType.VarChar, 30);
            param[5].Value = Client_tel;
            param[6] = new SqlParameter("@Client_country", SqlDbType.VarChar, 30);
            param[6].Value = Client_country;
            dbme.ConOpen();
            dbme.Query("Update_Client", param);
            dbme.ConClose();
        }
    }
}
