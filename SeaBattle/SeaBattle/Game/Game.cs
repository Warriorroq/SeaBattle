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
        public static Random random = new Random();
        public static int Height = 10;
        public static int Width = 10;

        private Thread writeToNet = new Thread(Protocol.UseConsoleAsChat);
        private static char[,] map = null;
        private static char[,] mapEnemy = MapFabric.CreateEmptyMap(Game.Height, Game.Width);
        private static bool shoot = false;
        private static int lives = 0;
        private static int wins = 0;
        private static int loses = 0;
        public void SetUpServer()
        {
            Protocol.SetUpServer();
            shoot = true;
            Start();
        }
        public void SetUpConnection()
        {
            Protocol.SetUpConnection();
            Start();
        }
        public void Start()
        {
            CreateMap();
            writeToNet.Start();
            ConsoleDraw.Draw(map, mapEnemy);
            Update();
            Protocol.Disconnect();
        }
        public static string ReadData()
        {
            return Console.ReadLine();
        }
        public static void GetMessage(string message)
        {
            var info = message.Split();
            try
            {
                TakeShot(info);
                ReadShot(info);
            }
            catch
            {
                return;
            }
            Console.WriteLine(message);
        }
        private static int CountLives(char[,] map)
        {
            int lives = 0;
            for (int i = 0; i < Game.Height; i++)
            {
                for (int j = 0; j < Game.Width; j++)
                {
                    if (map[i, j] == Symbols.ship)
                        lives++;
                }
            }
            return lives;
        }
        private static void CreateMap()
        {
            Console.WriteLine("'your' ? or 'auto'");
            var createCommand = Console.ReadLine().ToLower();
            if (createCommand == "your" || createCommand == "mine")
                map = MapFabric.CreatePlayersMap(Game.Height, Game.Width);
            else if(createCommand == "auto")
                map = MapFabric.CreateRandomMap(Game.Height, Game.Width);
            lives = CountLives(map);
        }
        private static void TakeShot(string[] shot)
        {
            if (shot.Length == 3 && !shoot)
            {
                int y = int.Parse(shot[0]);
                int x = int.Parse(shot[1]);
                if (map[y, x] == Symbols.ship)
                {
                    map[y, x] = Symbols.brokenShip;
                    shoot = false;
                    lives--;
                }
                else if (map[y, x] == Symbols.water)
                {
                    map[y, x] = Symbols.blushedWater;
                    shoot = true;
                }
                Protocol.Write($"{map[y, x]} {y} {x} {lives}");
                Console.Clear();
                ConsoleDraw.Draw(map, mapEnemy);
            }
        }
        private static void ReadShot(string[] shot)
        {
            if (shot.Length == 5 && shoot)
            {
                char a1 = shot[0][0];
                int y = int.Parse(shot[1]);
                int x = int.Parse(shot[2]);
                int lives = int.Parse(shot[3]);

                if (lives == 0)
                {
                    wins++;
                    RestartGame();
                }

                mapEnemy[y, x] = a1;
                if (a1 == Symbols.brokenShip)
                    shoot = true;
                else
                    shoot = false;
                Console.Clear();
                ConsoleDraw.Draw(map, mapEnemy);
            }
        }
        private static void RestartGame()
        {
            Console.Clear();
            Console.WriteLine("Restarting...");
            CreateMap();
            mapEnemy = MapFabric.CreateEmptyMap(Game.Height, Game.Width);
            ConsoleDraw.Draw(map,mapEnemy);
        }
        private void Update()
        {
            while (wins != 3 || loses != 3)
            {
                Protocol.UpdateConnection();
                if (lives == 0)
                {
                    loses++;
                    RestartGame();
                }
            }
        }
    }
}
