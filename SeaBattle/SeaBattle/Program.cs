using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SeaBattle
{
    class Program
    {
        public static int port = 8005; 
        public static string address = "127.0.0.1";
        static void Main(string[] args)
        {
            Console.WriteLine("0 - server | else - connect");
            try
            {
                int a = int.Parse(Console.ReadLine());
                if (a == 0)
                    new Game().SetUpServer();
                else
                    new Game().SetUpConnection();
            }
            catch
            {
                new Game().SetUpConnection();
            }
        }
    }
}
