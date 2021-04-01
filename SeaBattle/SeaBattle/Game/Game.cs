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
        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(Program.address), Program.port);
        Socket mainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        Socket seconPlayer = null;
        public void SetUpServer()
        {
            try
            {
                // связываем сокет с локальной точкой, по которой будем принимать данные
                mainSocket.Bind(ipPoint);

                // начинаем прослушивание
                mainSocket.Listen(10);

                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (seconPlayer is null)
                {
                    seconPlayer = mainSocket.Accept();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Start();
        }
        public void SetUpConnection()
        {
            try
            {
                mainSocket.Connect(ipPoint);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Start();
        }
        public void Start()
        {
            Thread write = new Thread(WriteTask);
            write.Start();
            while (true)
            {
                if (!(seconPlayer is null))
                    if (seconPlayer.Available > 0)
                    {
                        var data = ListenSocketReceive();
                        Console.WriteLine(Converter.BytesToString(data.Item1, data.Item2));
                    }
                if (mainSocket.Available > 0)
                {
                    var data = ListenSocketReceive();
                    Console.WriteLine(Converter.BytesToString(data.Item1, data.Item2));
                }
            }
            Exit();
        }
        public void Exit()
        {
            mainSocket.Shutdown(SocketShutdown.Both);
            mainSocket.Close();
            Console.Read();
        }
        private void WriteTask()
        {
            while(true)
                Write(Converter.StringToBytes(Console.ReadLine() + " "));
        }
        private void Write(byte[] data)
        {
            if (seconPlayer is null)
            {
                mainSocket.Send(data);
                return;
            }
            seconPlayer.Send(data);
        }
        private (byte[], int) ListenSocketReceive()
        {

            byte[] data = new byte[256];
            int bytes = 0;
            if(seconPlayer is null)
            {
                do
                {
                    bytes = mainSocket.Receive(data, data.Length, 0);
                }
                while (mainSocket.Available > 0);
            }
            else
                do
                {
                    bytes = seconPlayer.Receive(data, data.Length, 0);
                }
                while (seconPlayer.Available > 0);

            return (data, bytes);
        }
    }
}
