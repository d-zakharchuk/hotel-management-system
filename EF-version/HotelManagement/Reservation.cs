using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
namespace HotelManagement
{
    class Reservation
    {
        private HotelDataEF.HotelManagementDatabaseEntities ctx;
        private Room ROOMS = new Room();
        public object Clients()
        {
            HotelDataEF.HotelManagementDatabaseEntities ctx = new HotelDataEF.HotelManagementDatabaseEntities();
            ctx.Client.Load();
            var result = (from c in ctx.Client select c.Client_id).ToList();
            return result;
        }

        public void AddReservations (string Res_id, string Client_id,  int Room_num, DateTime Date_in)
        {
            HotelDataEF.Reservation NewReservation = new HotelDataEF.Reservation();
            NewReservation.Res_id = Res_id;
            NewReservation.Client_id = Client_id;
            NewReservation.Room_num = Room_num;
            NewReservation.Date_in = Date_in;
            NewReservation.Services = "000";
            ctx.Reservation.Add(NewReservation);
            ctx.SaveChanges();
            ROOMS.UpdatePeopleInRoom(Room_num, true);            
        }

        public void UpdateReservations (string Res_id, string Client_id, int Old_room, int New_room, DateTime Date_in)
        {
            ROOMS.UpdatePeopleInRoom(Old_room, false);
            List<HotelDataEF.Reservation> ReservationList = (List<HotelDataEF.Reservation>)
                (SelectReservationsWithThisId(Res_id));
            HotelDataEF.Reservation UpdateRes = ReservationList[0];
            UpdateRes.Client_id = Client_id;
            UpdateRes.Room_num = New_room;
            UpdateRes.Date_in = Date_in;
            ROOMS.UpdatePeopleInRoom(New_room, true);
            ctx.SaveChanges();
        }

        public void DeleteReservations(string Res_id, int room)
        {
            ROOMS.UpdatePeopleInRoom(room, false);
            HotelDataEF.Reservation DeleteRes = (HotelDataEF.Reservation)(SelectReservationsWithThisId(Res_id));
            ctx.Reservation.Remove(DeleteRes);
            ctx.SaveChanges();
        }

        public object SelectAllReservationsOfClient(string Client_id)
        {
            ctx = new HotelDataEF.HotelManagementDatabaseEntities();
            ctx.Reservation.Load();
            var result = (from r in ctx.Reservation where r.Client_id == Client_id select r.Res_id).ToList();
            return result;
        }

        public object ReservationsToDeleteInvoice(int room_num)
        {
            ctx = new HotelDataEF.HotelManagementDatabaseEntities();
            ctx.Reservation.Load();
            var result = (from r in ctx.Reservation where (r.Room_num == room_num)&&(r.Date_in<=DateTime.Today)
                          select r).ToList();
            return result;
        }

        public void AddServicesForReservation(string Res_id, int Serv_id)
        {
            HotelDataEF.Res_serv New_Res_serv = new HotelDataEF.Res_serv();
            New_Res_serv.Id_res = Res_id;
            New_Res_serv.Id_serv = Serv_id;
            ctx.Res_serv.Add(New_Res_serv);
            ctx.SaveChanges();
        }

        public void DeleteServicesFromReservation(string Res_id)
        {
            ctx.Res_serv.Load();
            var cl = (from r in ctx.Res_serv where r.Id_res == Res_id select r).ToList();
            foreach (var item in cl)
                ctx.Res_serv.Remove(item);
            ctx.SaveChanges();
        }

        public object SelectAllServicesOfReservation(string Res_id)
        {
            ctx = new HotelDataEF.HotelManagementDatabaseEntities();
            ctx.Res_serv.Load();
            ctx.Services.Load();
            var rs = (from r in ctx.Res_serv
                     join s in ctx.Services on r.Id_serv equals s.Id into r_s
                     from s in r_s.DefaultIfEmpty()
                     where r.Id_res == Res_id
                     select r_s).ToList();
            List<Services_of_res_query> List_Serv_of_Res_query = new List<Services_of_res_query>();
            foreach (var item in rs)
            {

                int id = item.First().Id;
                string service = item.First().Service;
                double price = item.First().Price;
                Services_of_res_query NewItem = new Services_of_res_query(id, service, price);
                List_Serv_of_Res_query.Add(NewItem);
            }
            return List_Serv_of_Res_query;
        }

        public int CountReservationsWithThisId(string id)
        {
            ctx = new HotelDataEF.HotelManagementDatabaseEntities();
            ctx.Reservation.Load();
            int num = (from r in ctx.Reservation where r.Res_id == id select r).Count();
            return num;
        }

        public object SelectReservationsWithThisId(string id)
        {
            ctx = new HotelDataEF.HotelManagementDatabaseEntities();
            ctx.Reservation.Load();
            var res = (from r in ctx.Reservation where r.Res_id == id select r).ToList();
            return res;
        }

    }

    class Services_of_res_query
    {
        public int _id_serv;
        public string _service_name;
        public double _price;
        public Services_of_res_query(int id, string service, double price)
        {
            _id_serv = id;
            _service_name = service;
            _price = price;
        } 
    }
}
