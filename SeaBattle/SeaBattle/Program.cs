using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SeaBattle
{
    class Program
    {
        public static int port = 8005;
        public static string address = "127.0.0.1";
        static void Main(string[] args)
        {
            Console.WriteLine($"What game you want (online|offline)");
            var type = Console.ReadLine().ToLower();
            if (type == "online")
                new Game().SetUpConnection();

        }
    }
}
