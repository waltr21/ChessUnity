using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts.ServerClient
{
    class ChessServer
    {
        private TcpListener Listener;
        private Thread ListenerThread;
        private TcpClient CurClient;
        private int GameState;

        public ChessServer()
        {
            GameState = 0;
            //More here later.
            return;
        }

        public void Init()
        {
            ListenerThread = new Thread(new ThreadStart(Listen));
            ListenerThread.IsBackground = true;
            ListenerThread.Start();
            return;
        }

        private void Listen()
        {
            try
            {
                // Create listener on localhost port 8052. 			
                Listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 9876);
                Listener.Start();
                //Debug.Log("Server is listening");
                Byte[] bytes = new Byte[1024];
                while (true)
                {
                    using (CurClient = Listener.AcceptTcpClient())
                    {
                        // Get a stream object for reading 					
                        using (NetworkStream stream = CurClient.GetStream())
                        {
                            int length;
                            // Read incomming stream into byte arrary. 						
                            while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                            {
                                var incommingData = new byte[length];
                                Array.Copy(bytes, 0, incommingData, 0, length);
                                // Convert byte array to string message. 							
                                string clientMessage = Encoding.ASCII.GetString(incommingData);
                                //Debug.Log("client message received as: " + clientMessage);
                            }
                        }
                    }
                    
                }
            }
            catch (SocketException e)
            {
                Debug.Log("SocketException " + e.ToString());
            }
        }


    }
}
