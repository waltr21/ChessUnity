using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts.ServerClient
{
    public class UserClient
    {
        private TcpClient socketConnection;
        private Thread clientReceiveThread;
        private int port;
        private string IP;
        public StartCanvas StartMenu;
        public int UserTeam;
        public Board BoardRef;

        public UserClient()
        {
            port = 9876;
            IP = "127.0.0.1";
        }

        public UserClient(string ip, int port)
        {
            this.port = port;
            this.IP = ip;
        }

        public bool isReady()
        {
            if (socketConnection != null) return true;
            return false;
        }

        public bool ConnectToServer()
        {
            try
            {
                clientReceiveThread = new Thread(new ThreadStart(Listen));
                clientReceiveThread.IsBackground = true;
                clientReceiveThread.Start();
                return true;
            }
            catch (Exception e)
            {
                Debug.Log("On client connect exception " + e);
                return false;
            }
        }

        public void Listen()
        {
            try
            {
                socketConnection = new TcpClient(IP, port);
                Byte[] bytes = new Byte[1024];
                while (true)
                {
                    // Get a stream object for reading 				
                    using (NetworkStream stream = socketConnection.GetStream())
                    {
                        int length;
                        // Read incomming stream into byte arrary. 					
                        while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            var incommingData = new byte[length];
                            Array.Copy(bytes, 0, incommingData, 0, length);
                            // Convert byte array to string message. 						
                            string serverMessage = Encoding.ASCII.GetString(incommingData);
                            ProcessPacket(serverMessage);
                        }
                    }
                }
            }
            catch (SocketException socketException)
            {
                Debug.Log("Socket exception: " + socketException);
                throw socketException;
            }
        }

        public void Send(object message)
        {
            if (socketConnection == null)
            {
                return;
            }
            try
            {
                string res = JsonUtility.ToJson(message);
                // Get a stream object for writing. 			
                NetworkStream stream = socketConnection.GetStream();
                if (stream.CanWrite)
                {
                    // Convert string message to byte array.                 
                    byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(res);
                    // Write byte array to socketConnection stream.                 
                    stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
                    Debug.Log("Sent Data: " + message);
                }
            }
            catch (SocketException socketException)
            {
                Debug.Log("Socket exception: " + socketException);
            }
        }

        private void ProcessPacket(string data)
        {
            try
            {
                Debug.Log("Received data: " + data);
                PacketBase temp = JsonUtility.FromJson<PacketBase>(data);
                if (temp.type == PacketType.Found)
                {
                    StartCanvas.SetGameFound(true);
                }
                if (temp.type == PacketType.NotFound)
                {
                    StartCanvas.GameNotFound();
                }
                if (temp.type == PacketType.InitMove)
                {
                    MovePacket curMove = JsonUtility.FromJson<MovePacket>(data);
                    BoardRef.AddPiece(BoardRef.King, curMove.toRow, curMove.toCol);
                }
            }
            catch(Exception e)
            {
                Debug.Log("Error processing packet: " + e);
            }
        }
    }
}
