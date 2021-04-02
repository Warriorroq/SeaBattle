using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SeaBattle
{
    public static class Protocol
    {
        private static IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(Program.address), Program.port);
        public static Socket mainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static List<Socket> sockets = null;
        public static void SetUpServer()
        {
            sockets = new List<Socket>();
            try
            {
                mainSocket.Bind(ipPoint);

                mainSocket.Listen(10);

                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (sockets.Count == 0)
                {
                    sockets.Add(mainSocket.Accept());
                }
                Console.WriteLine("Подключилось 4 человека...");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static void SetUpConnection()
        {
            try
            {
                mainSocket.Connect(ipPoint);
                Console.WriteLine("Подключилось...");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private static void UpdateSocketsInfo()
        {
            foreach (var socket in sockets)
            {
                if (socket.Available > 0)
                {
                    var data = ListenSocketReceive(socket);
                    ReSendData(data.Item1, socket);
                    Game.GetMessage(Converter.BytesToString(data.Item1, data.Item2));
                }
            }
        }
        private static void WriteNewInformation()
        {
            if (mainSocket.Available > 0)
            {
                var data = ListenSocketReceive(mainSocket);
                Game.GetMessage(Converter.BytesToString(data.Item1, data.Item2));
            }
        }
        public static void WriteTask()
        {
            while (sockets is null)
                Write(Converter.StringToBytes(Console.ReadLine()));
            while (!(sockets is null))
                ReSendData(Converter.StringToBytes(Console.ReadLine()), null);
        }
        private static void Write(byte[] data)
        {
            if (sockets is null)
            {
                mainSocket.Send(data);
                return;
            }
        }
        private static void ReSendData(byte[] data, Socket sender)
        {
            foreach (var socket in sockets)
                if (socket != sender)
                    socket.Send(data);
        }
        private static (byte[], int) ListenSocketReceive(Socket socket)
        {

            byte[] data = new byte[512];
            int bytes = 0;
            do
            {
                bytes = socket.Receive(data, data.Length, 0);
            }
            while (socket.Available > 0);
            return (data, bytes);
        }
        public static void UpdateConnection()
        {
            if (!(sockets is null))
            {
                UpdateSocketsInfo();
            }
            WriteNewInformation();
        }
        public static void Disconnect()
        {
            mainSocket.Shutdown(SocketShutdown.Receive);
            mainSocket.Close();
            Console.Read();
        }
    }
}
