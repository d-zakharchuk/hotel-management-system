using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HotelManagement
{
    class DatabaseMethods
    {
        SqlConnection conn;
        DataSource ds = new DataSource();
        public DatabaseMethods()
        {
            conn = new SqlConnection(ds.source);
        }

        public void ConOpen()
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
        }

        public void ConClose()
        {
            if (conn.State != ConnectionState.Closed)
                conn.Close();
        }

        public DataTable ReadData(string st_proc, SqlParameter[] param)
        {
            SqlCommand command = new SqlCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = st_proc;
            command.Connection = conn;
            if (param != null)
                command.Parameters.AddRange(param);
            SqlDataAdapter db = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            db.Fill(dt);
            return dt;
        }

        public void Query(string st_proc, SqlParameter[] param)
        {
            SqlCommand command = new SqlCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = st_proc;
            command.Connection = conn;
            if (param != null)
                command.Parameters.AddRange(param);
            command.ExecuteNonQuery();
        }

        public object ReturnValue(string st_proc, SqlParameter[] param)
        {
            Object ReturnValue;
            SqlCommand command = new SqlCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = st_proc;
            command.Connection = conn;
            if (param != null)
                command.Parameters.AddRange(param);
            ReturnValue = command.ExecuteScalar();
            return ReturnValue;
        }

        public DataTable ExecuteSQL(string text)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = text;
                command.Connection = conn;
                SqlDataAdapter db = new SqlDataAdapter(command);
                db.Fill(dt);
            }
            catch
            {
                
                string str = "The action failed.";
                MessageBox.Show(str);
            }
            return dt;
        }
    }
}
