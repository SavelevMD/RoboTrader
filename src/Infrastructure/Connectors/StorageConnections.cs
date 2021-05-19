namespace Connectors
{

    public class RedisConnections
    {
        public static string DebugConnection = "localhost:32768,abortConnect=false";
    }

    public class PGConnections
    {
        public static string DebugConnection = "Host=localhost;Port=5432;Database=ticker;User ID=postgres;Password=norobo007";
    }
}
