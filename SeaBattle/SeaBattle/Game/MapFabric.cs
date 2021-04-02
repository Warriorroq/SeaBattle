using System;
using System.Collections.Generic;
using System.Text;
namespace SeaBattle
{
    public static class MapFabric
    {
        public static char[,] CreateMap(int height,int width)
        {
            var map = CreateEmptyMap(height, width);
            for(int i =0;i<30;i++)
            {
                map[Game.random.Next(0, 10), Game.random.Next(0, 10)] = '■';
            }
            return map;
        }
        public static char[,] CreateEmptyMap(int height, int width)
        {
            var map = new char[height, width];
            for (int i = 0; i < height; i++)
                for (int ii = 0; ii < width; ii++)
                    map[i, ii] = '░';
            return map;
        }
    }
}
