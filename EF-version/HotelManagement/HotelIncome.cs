using System;
using System.Data;
using System.Linq;
using System.Data.Entity;

namespace HotelManagement
{
    class HotelIncome
    {
        private HotelDataEF.HotelManagementDatabaseEntities ctx;
        public void AddIncome(int room_num, int residents, DateTime date_in, DateTime date_out, double r_cost,
            double s_cost, double total)
        {
            ctx = new HotelDataEF.HotelManagementDatabaseEntities();
            HotelDataEF.HotelIncome NewIncome = new HotelDataEF.HotelIncome();
            NewIncome.Room_num = room_num;
            NewIncome.Residents = residents;
            NewIncome.Date_in = date_in;
            NewIncome.Date_out = date_out;
            NewIncome.Room_cost = r_cost;
            NewIncome.Services_cost = s_cost;
            NewIncome.Total = total;
            ctx.HotelIncome.Add(NewIncome);
            ctx.SaveChanges();
        }

        public void DeleteIncome(int id)
        {
            ctx.HotelIncome.Load();
            HotelDataEF.HotelIncome IncomeToDelete = (from hi in ctx.HotelIncome where hi.Id == id select hi).First();
            ctx.HotelIncome.Remove(IncomeToDelete);
            ctx.SaveChanges();
        }
    }
}
