using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace HotelManagement
{
    class Room
    {
        private HotelDataEF.HotelManagementDatabaseEntities ctx;
        public object Room_Type()
        {
            ctx = new HotelDataEF.HotelManagementDatabaseEntities();
            ctx.Type.Load();
            var type = (from t in ctx.Type select t.type1).ToList();
            return type;
        }

        public object Room_by_Type(string type, string Client_id)
        {
            ctx = new HotelDataEF.HotelManagementDatabaseEntities();
            ctx.Client.Load();
            var query1 = (from re in ctx.Reservation where re.Client_id == Client_id select re.Room_num).ToList();
            var query2 = (from r in ctx.Room where r.people < (from t in ctx.Type where t.type1==type select t.C_people).FirstOrDefault() select r.room_num).ToList();
            var result = (from r in ctx.Room
                          where ((r.type==type)&&!query1.Contains(r.room_num)&&query2.Contains(r.room_num)) select r.room_num).ToList();
            
            return result;
        }

        public object Room_by_Type_for_Update(string type, string Client_id, string Res_id)
        {
            ctx = new HotelDataEF.HotelManagementDatabaseEntities();
            ctx.Reservation.Load();
            ctx.Room.Load();
            ctx.Type.Load();
            var query1 = (from re in ctx.Reservation where re.Client_id == Client_id select re.Room_num).ToList();
            var query2 = (from r in ctx.Room where r.people<
                          (from t in ctx.Type where t.type1==type select t.C_people).FirstOrDefault()
                          select r.room_num).ToList();
            var query3 = (from re in ctx.Reservation where (re.Res_id==Res_id&&re.Client_id==Client_id)select re.Room_num).ToList();
            var query4 = (from r in ctx.Room where r.type==type select r.room_num).ToList();
            
            var result = (from r in ctx.Room where (r.type==type&&!query1.Contains(r.room_num)&&query2.Contains(r.room_num))||(query3.Contains(r.room_num)&& query4.Contains(r.room_num)) select r.room_num).ToList();
            return result;
        }

        public object Room_by_Type_for_Invoice(string type)
        {
            ctx = new HotelDataEF.HotelManagementDatabaseEntities();
            ctx.Room.Load();
            var result = (from r in ctx.Room where r.people > 0 && r.type == type select r.room_num).ToList();
            return result;
        }

        public int SelectRoomFromReservation(string Res_id)
        {
            ctx = new HotelDataEF.HotelManagementDatabaseEntities();
            ctx.Reservation.Load();
            int result = (int)(from r in ctx.Reservation where r.Res_id == Res_id select r.Room_num).First();
            return result;
        }

        public string GetTypeOfFrrom(int room_num)
        {
            ctx = new HotelDataEF.HotelManagementDatabaseEntities();
            ctx.Room.Load();
            var query = (from r in ctx.Room where r.room_num == room_num select r.type).First();
            return query;
        }

        public int CheckRoomState(int room_num)
        {
            ctx = new HotelDataEF.HotelManagementDatabaseEntities();
            ctx.Room.Load();
            ctx.Type.Load();
            var query1 = (from r in ctx.Room where r.room_num == room_num select r.people).First();
            var query2 = (from r in ctx.Room where r.room_num == room_num select r.type).First();
            var query3 = (from t in ctx.Type where t.type1 == query2 select t.C_people).First();
            int result = 0;
            if (query1 == 0)
                result = 0;
            else if (query1 == query3)
                result = 2;
            else result = 1;
            return result;
        }

        public object RoomInfo(int room_num, string procedure)
        {
            ctx = new HotelDataEF.HotelManagementDatabaseEntities();
            ctx.Client.Load();
            ctx.Reservation.Load();
            ctx.Room.Load();
            ctx.Type.Load();

            if (procedure== "Room_Info_1")
            {
                var rt = (from r in ctx.Room
                          join t in ctx.Type
                          on r.type equals t.type1 into r_t
                          from t in r_t.DefaultIfEmpty()
                          where r.room_num == room_num
                          select new { r.room_num, r.floor, r.people, t.type1, t.C_people, t.Price_1, t.Price,
                              t.Area}).ToList();
                List<Room1Query> List_Room1Query = new List<Room1Query>();
                foreach (var item in rt)
                {
                    int room = item.room_num;
                    int floor = item.floor;
                    int people = (int)(item.people);
                    string type = item.type1;
                    int C_people = (int)(item.C_people);
                    double price1 = (double)(item.Price_1);
                    double price = (double)(item.Price);
                    double area = (double)(item.Area);
                    Room1Query newitem = new Room1Query(room, floor, people, type, C_people, price1, price, area);
                    List_Room1Query.Add(newitem);
                }
                return List_Room1Query;
            }
            else if (procedure== "Room_Info_2")
            {
                var rc = (from r in ctx.Reservation
                          join c in ctx.Client
                          on r.Client_id equals c.Client_id into r_c
                          from c in r_c.DefaultIfEmpty()
                          where r.Room_num == room_num
                          select new { r.Res_id, c.Client_id, r.Room_num, r.Date_in, c.Client_first_name,
                              c.Client_middle_name, c.Client_last_name, c.Client_date_of_birth, c.Client_tel,
                              c.Client_country}).ToList();
                List<Room2Query> List_Room2Query = new List<Room2Query>();
                foreach(var item in rc)
                {
                    string res_id = item.Res_id;
                    string client_id = item.Client_id;
                    int room = item.Room_num;
                    DateTime Date_in = item.Date_in;
                    string client_first_name = item.Client_first_name;
                    string client_middle_name = item.Client_middle_name;
                    string client_last_name = item.Client_last_name;
                    DateTime client_dob = item.Client_date_of_birth;
                    string client_tel = item.Client_tel;
                    string client_country = item.Client_country;
                    Room2Query newitem = new Room2Query(res_id, client_id, room, Date_in, client_first_name,
                        client_middle_name, client_last_name, client_dob, client_tel, client_country);
                    List_Room2Query.Add(newitem);
                }
                return List_Room2Query;

            }
            object ob = null;
            return ob;
        }

        public int FreeRoomsCount(string type)
        {
            ctx = new HotelDataEF.HotelManagementDatabaseEntities();
            ctx.Room.Load();
            int result = (int)(from r in ctx.Room where r.type == type && r.people == 0 select r).Count();
            return result;            
        }

        public void UpdatePeopleInRoom(int room, bool IsAdd)
        {
            HotelDataEF.Room DesiredRoom = (HotelDataEF.Room)(SelectRoomByNum(room));
            if (IsAdd)
                DesiredRoom.people++;
            else DesiredRoom.people--;
            ctx.SaveChanges();
        }

        public object SelectRoomByNum(int room)
        {
            ctx = new HotelDataEF.HotelManagementDatabaseEntities();
            ctx.Room.Load();
            var result = (from r in ctx.Room where r.room_num == room select r).First();
            return result;
        }

        public int MaxResidents(string type, int request)
        {
            ctx = new HotelDataEF.HotelManagementDatabaseEntities();
            ctx.Room.Load();
            ctx.Type.Load();
            int result = 0;
            if (request == 1)
                result = (int)(from r in ctx.Room where r.type == type select r.people).Max();
            else result = (int)(from t in ctx.Type where t.type1 == type select t.C_people).Max();
            return result;
        }

        public void UpdateRoom(int room_num, string new_type, int floor)
        {
            HotelDataEF.Room DesiredRoom = (HotelDataEF.Room)(SelectRoomByNum(room_num));
            DesiredRoom.type = new_type;
            DesiredRoom.floor = floor;
            ctx.SaveChanges();
        }

        public void AddType(string type, int people, double price1, double price, double area)
        {
            HotelDataEF.Type newtype = new HotelDataEF.Type();
            newtype.type1 = type;
            newtype.C_people = people;
            newtype.Price_1 = price1;
            newtype.Price = price;
            newtype.Area = area;
            ctx.Type.Add(newtype);
            ctx.SaveChanges();
        }

        public void UpdateType(string old_type, string new_type, int people, double price1, double price, double area)
        {
            List<HotelDataEF.Type> TypesList = (List<HotelDataEF.Type>)(SelectType(old_type));
            HotelDataEF.Type DesiredType = TypesList[0];
            DesiredType.type1 = new_type;
            DesiredType.C_people = people;
            DesiredType.Price_1 = price1;
            DesiredType.Price = price;
            DesiredType.Area = area;
            ctx.SaveChanges();
        }

        public object SelectType(string type)
        {
            var query = (from t in ctx.Type where t.type1 == type select t).ToList();
            return query;
        }

        public int CountRoomsOfThisType(string type)
        {
            int result = 0;
            ctx = new HotelDataEF.HotelManagementDatabaseEntities();
            ctx.Room.Load();
            result = (int)(from r in ctx.Room where r.type == type select r.room_num).Count();
            return result;
        }

        public void DeleteType(string type)
        {
            List<HotelDataEF.Type> TypesList = (List<HotelDataEF.Type>)(SelectType(type));
            HotelDataEF.Type DesiredType = TypesList[0];
            ctx.Type.Remove(DesiredType);
            ctx.SaveChanges();
        }
    }
    class Room1Query
    {
        public int _room_num, _floor, _people, _C_people;
        public string _type;
        public double _price1, _price, _area;
        public Room1Query(int room_num, int floor, int people, string type, int C_people, double price1, double price,
            double area)
        {
            _room_num = room_num;
            _floor = floor;
            _people = people;
            _type = type;
            _C_people = C_people;
            _price1 = price1;
            _price = price;
            _area = area;
        }
    }
    class Room2Query
    {
        public string _res_id, _client_id, _client_first_name, _client_middle_name, _client_last_name, _client_tel,
            _client_country;
        public int _room_num;
        public DateTime _date_in, _client_dob;
        public Room2Query(string Res_id, string Client_id, int Room_num, DateTime Date_in, string Client_first_name,
            string Client_middle_name, string Client_last_name, DateTime Client_date_of_birth, string Client_tel,
            string Client_country)
        {
            _res_id = Res_id;
            _client_id = Client_id;
            _room_num = Room_num;
            _date_in = Date_in;
            _client_first_name = Client_first_name;
            _client_middle_name = Client_middle_name;
            _client_last_name = Client_last_name;
            _client_dob = Client_date_of_birth;
            _client_tel = Client_tel;
            _client_country = Client_country;
        }
    }
}
