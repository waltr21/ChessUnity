using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.ServerClient
{
    //Not sure if we even need this thing...
    public class SetTurnPacket : PacketBase
    {
        public int turn;

        public SetTurnPacket()
        {
            return;
        }
    }
}
