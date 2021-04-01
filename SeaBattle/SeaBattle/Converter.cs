using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle
{
    public static class Converter
    {
        private static StringBuilder stringBuilder = new StringBuilder();
        public static byte[] StringToBytes(string message)
            => Encoding.Unicode.GetBytes(message);
        public static string BytesToString(byte[] data, int bytes)
        {
            stringBuilder.Clear();
            return stringBuilder.Append(Encoding.Unicode.GetString(data, 0, bytes)).ToString();
        }
    }
}
