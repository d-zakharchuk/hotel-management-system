namespace HotelManagement
{
    class DataSource
    {
        internal string source =
@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + System.IO.File.ReadAllText(@"DatabasePath.txt") + ";Integrated Security=True;Connect Timeout=30;";
    }
}
