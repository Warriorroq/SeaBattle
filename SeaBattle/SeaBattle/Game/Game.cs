using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SeaBattle
{
    public class Game
    {
        private IPEndPoint ipPoint;
        private Socket gameSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private Socket connectedSocket = null;
        private StringBuilder builder = new StringBuilder();
        public void SetUpConnection()
        {
            Console.WriteLine("Connect ? y/n");
            if (Console.ReadLine()[0] == 'y')
            {
                ConnectLobby();
                return;
            }
            else
            {
                Console.WriteLine("Create lobby ? y/n");
                if (Console.ReadLine()[0] != 'y')
                    return;
                CreateLobby();
            }
        }
        private void CreateIpPoint()
        {
            Console.WriteLine("Enter ip '0' - standart");

            string ip = Console.ReadLine();

            if (ip == "0")
                ip = Program.address;

            Console.WriteLine("Enter port '0' - standart");

            int port = int.Parse(Console.ReadLine());

            if (port == 0)
                port = Program.port;

            ipPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        }
        private void CreateLobby()
        {
            CreateIpPoint();
            gameSocket.Bind(ipPoint);
            gameSocket.Listen(4);

            while (connectedSocket is null)
                connectedSocket = gameSocket.Accept();

            byte[] data = new byte[256];
            while(true)
            {
                var bytes = connectedSocket.Receive(data);
                Console.WriteLine(GetStringFromBytes(data, bytes));
                data = new byte[256];
            }
            Console.WriteLine("Сервер запущен. Ожидание подключений...");
        }
        private void ConnectLobby()
        {
            CreateIpPoint();

            gameSocket.Connect(ipPoint);
            while (true)
            {
                byte[] message = EncodeString(Console.ReadLine());
                gameSocket.Send(message);
            }
            //socket.Send(data);
            //socket.Shutdown(SocketShutdown.Both);
            //socket.Close();
        }
        public void Start()
        {

        }
        private string GetStringFromBytes(byte[] data, int bytes)
        {
            builder.Clear();
            return builder.Append(Encoding.Unicode.GetString(data, 0, bytes)).ToString();
        }
        private int CountNumberOfBytes(byte[] data)
            => gameSocket.Receive(data, data.Length, 0);
        private byte[] EncodeString(string message)
            =>Encoding.Unicode.GetBytes(message);
        
    }
}
