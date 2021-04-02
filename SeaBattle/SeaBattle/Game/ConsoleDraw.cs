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
                for (int ii = 0; ii < 10; ii++)
                {
                    Console.Write(map[i, ii]);
                }
                Console.Write(" ");
                Console.Write($"{i} ");
                for (int ii = 0; ii < 10; ii++)
                {
                    Console.Write(mapEnemy[i, ii]);
                }
                Console.WriteLine();
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
