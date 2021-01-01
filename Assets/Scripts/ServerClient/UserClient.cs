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
        public string IP;
        public StartCanvas StartMenu;
        public int UserTeam;
        public Board BoardRef;
        public bool connected;

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
                connected = true;
            }
            catch (Exception e)
            {
                Debug.Log("On client connect exception " + e);
                connected = false;
            }
            return connected;
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
            Thread sendThread = new Thread(new ParameterizedThreadStart(SpawnSendThread));
            sendThread.IsBackground = true;
            sendThread.Start(message);
        }

        public void SpawnSendThread(object message)
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
                switch (temp.type)
                {
                    case PacketType.Join:
                        StartCanvas.SetGameFound(true);
                        break;
                    //Host packet will be sent back if we have a successful game creation.
                    case PacketType.Host:
                        StartCanvas.SetGameFound(true);
                        break;
                    case PacketType.Error:
                        ErrorPacket ep = JsonUtility.FromJson<ErrorPacket>(data);
                        StartCanvas.SetError(ep.message);
                        break;
                    case PacketType.InitMove:
                        lock (BoardRef.ServerMove)
                        {
                            BoardRef.ServerMove = JsonUtility.FromJson<MovePacket>(data);
                        }
                        break;
                    case PacketType.StandardMove:
                        lock (BoardRef.ServerMove)
                        {
                            BoardRef.ServerMove = JsonUtility.FromJson<MovePacket>(data);
                        }
                        break;
                    case PacketType.SetTurn:
                        SetTurnPacket stp;
                        stp = JsonUtility.FromJson<SetTurnPacket>(data);
                        BoardRef.turn = stp.turn;
                        break;
                    case PacketType.GameStart:
                        if (BoardRef.GameState == GameState.WaitingToStart)
                        {
                            BoardRef.GameState = GameState.InitialMoves;
                        }
                        break;
                    case PacketType.ILose:
                        lock (BoardRef.ServerMove)
                        {
                            BoardRef.ServerMove = JsonUtility.FromJson<MovePacket>(data);
                        }
                        break;
                    // Unknown. Thow it out.
                    default:
                        break;
                }
                
            }
            catch(Exception e)
            {
                Debug.Log("Error processing packet: " + e);
            }
        }
    }
}
