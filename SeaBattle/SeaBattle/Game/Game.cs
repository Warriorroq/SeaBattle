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
        public void SetUpServer()
        {
            Protocol.SetUpServer();
            Start();
        }
        public void SetUpConnection()
        {
            Protocol.SetUpConnection();
            Start();
        }
        public void Start()
        {
            Thread write = new Thread(Protocol.WriteTask);
            write.Start();
            Update();
            Protocol.Disconnect();
        }
        public static void GetMessage(string message)
        {
            Console.WriteLine(message);
        }
        private void Update()
        {
            while (true)
            {
                Protocol.UpdateConnection();
            }
        }
    }
}
