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
        private IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(Program.address), Program.port);
        private Socket mainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private List<Socket> sockets = null;
        public void SetUpServer()
        {
            sockets = new List<Socket>();
            try
            {
                // связываем сокет с локальной точкой, по которой будем принимать данные
                mainSocket.Bind(ipPoint);

                // начинаем прослушивание
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
            Start();
        }
        public void SetUpConnection()
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
            Start();
        }
        private void UpdateSocketsInfo()
        {
            foreach (var socket in sockets)
            {
                if (socket.Available > 0)
                {
                    var data = ListenSocketReceive(socket);
                    ReSendData(data.Item1, socket);
                    Console.WriteLine(Converter.BytesToString(data.Item1, data.Item2));
                }
            }
        }
        private void WriteNewInformation()
        {
            if (mainSocket.Available > 0)
            {
                var data = ListenSocketReceive(mainSocket);
                Console.WriteLine(Converter.BytesToString(data.Item1, data.Item2));
            }
        }
        private void WriteTask()
        {
            while(sockets is null)
                Write(Converter.StringToBytes(Console.ReadLine()));
            while (!(sockets is null))
                ReSendData(Converter.StringToBytes(Console.ReadLine()), null);
        }
        private void Write(byte[] data)
        {
            if (sockets is null)
            {
                mainSocket.Send(data);
                return;
            }
        }
        private void ReSendData(byte[] data, Socket sender)
        {
            foreach (var socket in sockets)
                if (socket != sender)
                    socket.Send(data);
        }
        private (byte[], int) ListenSocketReceive(Socket socket)
        {

            byte[] data = new byte[256];
            int bytes = 0;
            do
            {
                bytes = socket.Receive(data, data.Length, 0);
            }
            while (socket.Available > 0);
            return (data, bytes);
        }
        private void UpdateConnection()
        {
            if(!(sockets is null))
            {
                UpdateSocketsInfo();
            }
            WriteNewInformation();
        }
        public void Start()
        {
            Thread write = new Thread(WriteTask);
            write.Start();
            Update();
            Exit();
        }
        private void Update()
        {
            while (true)
            {
                UpdateConnection();
            }
        }
        private void Exit()
        {
            mainSocket.Shutdown(SocketShutdown.Both);
            mainSocket.Close();
            Console.Read();
        }
    }
}
