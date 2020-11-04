using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.ServerClient
{
    class GameStartPacket : PacketBase
    {
        public string ServerName;
        public string Password;

        public GameStartPacket(string ServerName, string Password)
        {
            this.ServerName = ServerName;
            this.Password = Password;
            this.type = PacketType.Host;
        }
    }
}
