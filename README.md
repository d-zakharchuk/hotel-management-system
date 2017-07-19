# Hotel Management System
ver. 2017.7.18.1526

This app is developed for hotel reception and administration, providing them an opportunity to manage clients’ base, booking state of rooms and hotel income info.

There are two modes: Reception Mode and Admin (SuperUser) Mode.

Reception Mode:
- registering clients, filling their name, date of birth, phone and country.
- booking a room based on room type, room number and check in date.
- customers can also order some services offered by the hotel.
- creating an invoice when checking out the clients (invoice can be saved as HTML file for future printing).
- viewing hotel income info. A chart will help to analyze the growth dynamics of income of the last year.

Admin (SuperUser) Mode:
- modifying room types which involves: maximum number of people allowed to stay in a room, pricing preferences, room area.
- modifying services (service name and price).
- deleting income info.
- changing login data.

There are two versions: the first one with SQL, another one uses Entity Framework technology.

Database issues:
Copy database file Hotel.mdf to location you prefer.
For SQL version, find file /SQL-version/HotelManagement/bin/Debug/DatabasePath.txt and type the path to your database.
For Entity Framework version, find the following two files:
1) /EF-version/HotelDataEF/App.config
2) /EF-version/HotelManagement/App.config

Go to connection strings section (first one: string #8, second one: string #11) and replace the initial database file path with your own where your database is located.

