using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle
{
    public static class ConsoleDraw
    {
        public static void Draw(char[,] map, char[,] mapEnemy)
        {
            Console.Write($"  ");
            DrawNums();
            Console.Write($"   ");
            DrawNums();
            Console.WriteLine();
            for (int i = 0; i < 10; i++)
            {
                Console.Write($"{i} ");
                DrawMapLine(i, map);
                Console.Write(" ");
                Console.Write($"{i} ");
                DrawMapLine(i, mapEnemy);
                Console.WriteLine();
            }
        }
        private static void DrawMapLine(int line,char[,]map)
        {
            for (int i = 0; i < 10; i++)
            {
                Console.Write(map[line, i]);
            }
        }
        private static void DrawNums()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.Write($"{i}");
            }
        }
    }
}
