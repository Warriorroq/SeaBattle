using System;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace SeaBattle
{
    public class Game
    {
        private Thread writeToNet = new Thread(Protocol.UseConsoleAsChat);
        public static Random random = new Random();
        private static char[,] map = null;
        private static char[,] mapEnemy = MapFabric.CreateEmptyMap(10,10);
        static bool shoot = true;
        public void SetUpServer()
        {
            Protocol.SetUpServer();
            shoot = true;
            Start();
        }
        public void SetUpConnection()
        {
            Protocol.SetUpConnection();
            shoot = false;
            Start();
        }
        public void Start()
        {
            map = MapFabric.CreateMap(10, 10);
            writeToNet.Start();
            ConsoleDraw.Draw(map, mapEnemy);
            Update();
            Protocol.Disconnect();
        }
        public static void GetMessage(string message)
        {
            var a = message.Split();
            try
            {
                if(!shoot)
                {
                    if(a.Length == 3)
                    {
                        int y = int.Parse(a[0]);
                        int x = int.Parse(a[1]);

                        if (map[y, x] == '■')
                            map[y, x] = '♂';
                        else if (map[y, x] == '░')
                            map[y, x] = '☼';

                        Protocol.Write($"{map[y, x]} {y} {x}");
                        shoot = true;
                    }
                }
                else
                {
                    if(a.Length == 4)
                    {
                        char a1 = a[0][0];
                        int y = int.Parse(a[1]);
                        int x = int.Parse(a[2]);
                        mapEnemy[y, x] = a1;
                        shoot = false;
                    }
                }
                Console.Clear();
                ConsoleDraw.Draw(map, mapEnemy);
            }
            catch
            {
                return;
            }
            Console.WriteLine(message);
        }
        private void Update()
        {
            while (true)
            {
                Protocol.UpdateConnection();
            }
        }
    }
}
