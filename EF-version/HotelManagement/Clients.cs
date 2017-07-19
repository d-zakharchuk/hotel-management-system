using System.Data;
using System.Linq;
using System.Data.Entity;
using System;

namespace HotelManagement
{
    class Clients
    {
        private HotelDataEF.HotelManagementDatabaseEntities ctx;
        public void AddClients(string Client_id, string Client_first_name, string Client_middle_name,
            string Client_last_name, string Client_date_of_birth, string Client_tel, string Client_country)
        {
            HotelDataEF.Client NewCLient = new HotelDataEF.Client();
            NewCLient.Client_id = Client_id;
            NewCLient.Client_first_name = Client_first_name;
            NewCLient.Client_middle_name = Client_middle_name;
            NewCLient.Client_last_name = Client_last_name;
            NewCLient.Client_date_of_birth = Convert.ToDateTime(Client_date_of_birth);
            NewCLient.Client_tel = Client_tel;
            NewCLient.Client_country = Client_country;
            ctx.Client.Add(NewCLient);
            ctx.SaveChanges();
        }

        public void DeleteClients(string Client_id)
        {
            HotelDataEF.Client DeleteCLient = (HotelDataEF.Client)(SelectClientWithThisId(Client_id));
            ctx.Client.Remove(DeleteCLient);
            ctx.SaveChanges();
        }

        public void UpdateClients(string Client_id, string Client_first_name, string Client_middle_name,
            string Client_last_name, string Client_date_of_birth, string Client_tel, string Client_country)
        {
            HotelDataEF.Client UpdateCLient = (HotelDataEF.Client)(SelectClientWithThisId(Client_id));
            UpdateCLient.Client_first_name = Client_first_name;
            UpdateCLient.Client_middle_name = Client_middle_name;
            UpdateCLient.Client_last_name = Client_last_name;
            UpdateCLient.Client_date_of_birth = Convert.ToDateTime(Client_date_of_birth);
            UpdateCLient.Client_tel = Client_tel;
            UpdateCLient.Client_country = Client_country;
            ctx.SaveChanges();
        }

        public int CountClientsWithThisId(string id)
        {
            ctx = new HotelDataEF.HotelManagementDatabaseEntities();
            ctx.Client.Load();
            int b = (int)(from c in ctx.Client where c.Client_id==id select c).Count();
            return b;
        }

        public object SelectClientWithThisId(string id)
        {
            ctx = new HotelDataEF.HotelManagementDatabaseEntities();
            ctx.Client.Load();
            var cl = (from c in ctx.Client where c.Client_id == id select c).First();
            return cl;
        }
    }
}
