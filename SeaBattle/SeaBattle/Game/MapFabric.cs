using System;
using System.Collections.Generic;
using System.Text;
namespace SeaBattle
{
    public static class MapFabric
    {
        public enum ShipType{ 
            left = 0,
            bottom = 1
        }
        public static char[,] CreatePlayersMap(int height, int width)
        {
            var map = CreateEmptyMap(height, width);
            CreatePlayerShips(map, 5, 2);
            CreatePlayerShips(map, 4, 3);
            CreatePlayerShips(map, 3, 4);
            CreatePlayerShips(map, 2, 5);
            CreatePlayerShips(map, 1, 6);
            return map;
        }
        public static char[,] CreateRandomMap(int height, int width)
        {
            var map = CreateEmptyMap(height, width);
            CreateRandomShips(map, 5, 2);
            CreateRandomShips(map, 4, 3);
            CreateRandomShips(map, 3, 4);
            CreateRandomShips(map, 2, 5);
            CreateRandomShips(map, 1, 6);
            return map;
        }
        public static char[,] CreateEmptyMap(int height, int width)
        {
            var map = new char[height, width];
            for (int i = 0; i < height; i++)
                for (int ii = 0; ii < width; ii++)
                    map[i, ii] = Symbols.water;
            return map;
        }
        private static void CreateRandomShips(char[,] map, int shipSize,int count)
        {
            while(count > 0)
            {
                var y = Game.random.Next(0, Game.Height - shipSize);
                var x = Game.random.Next(0, Game.Width - shipSize);
                if (CreateShip(map, shipSize, y, x, (ShipType)Game.random.Next(0, 2)))
                    count--;
            }
        }
        private static void CreatePlayerShips(char[,] map, int shipSize, int count)
        {
            while (count > 0)
            {
                try
                {
                    Console.WriteLine("y:");
                    int y = int.Parse(Console.ReadLine());

                    Console.WriteLine("x:");
                    int x = int.Parse(Console.ReadLine());

                    Console.WriteLine("way left | bottom:");
                    var type = Console.ReadLine();
                    var shipType = ShipType.bottom;
                    if (type.ToLower() == "left")
                        shipType = ShipType.left;
                    else if (type.ToLower() == "bottom")
                        shipType = ShipType.bottom;

                    if (CreateShip(map, shipSize, y, x, shipType))
                    {
                        count--;
                        Console.WriteLine("Поставлено");
                    }
                    else
                        Console.WriteLine("На пути препятствие");
                }
                catch
                {
                    Console.WriteLine("Неправильнный ввод");
                }
            }
        }
        private static bool CreateShip(char[,] map, int shipSize, int y, int x, ShipType shipType)
        {
            (int, int)[] ship = null;
            if (shipType == ShipType.bottom)
                ship = PlaceToBottomShip(map, shipSize, y, x);
            else
                ship = PlaceLeftShip(map, shipSize, y, x);

            if (!(ship is null))
                PlaceShip(map, ship);

            return !(ship is null);
        }
        private static void PlaceShip(char[,] map, (int,int)[] ship)
        {
            foreach (var place in ship)
                map[place.Item1, place.Item2] = Symbols.ship;
        }
        private static (int, int)[] PlaceLeftShip(char[,] map, int shipSize, int y, int x)
        {
            var poses = new (int, int)[shipSize + 1];
            for (int j = 0, i = x; i < x + shipSize; i++, j++)
            {
                if (map[y, i] != Symbols.water)
                    return null;
            }
            return poses;
        }
        private static (int,int)[] PlaceToBottomShip(char[,] map, int shipSize, int y, int x)
        {
            var poses = new (int, int)[shipSize];
            for (int j = 0, i = y; i < y + shipSize; i++, j++)
            {
                if (map[i, x] != Symbols.water)
                    return null;
                poses[j] = (i, x);
            }
            return poses;
        }
    }
}
