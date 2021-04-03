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
        public static Game game = null;
        static void Main(string[] args)
        {
            Console.WriteLine("0 - server | else - connect");
            try
            {
                game = new Game();
                int a = int.Parse(Console.ReadLine());
                if (a == 0)
                    game.SetUpServer();
                else
                    game.SetUpConnection();
            }
            catch
            {
                new Game().SetUpConnection();
            }
        }
    }
}
