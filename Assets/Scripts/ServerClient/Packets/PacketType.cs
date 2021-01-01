using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.ServerClient
{
    public enum PacketType
    {
        Host,
        Join,
        Checkup,
        Error,
        PlayerReady,
        SetTurn,
        InitMove,
        StandardMove,
        ILose,
        GameStart,
        Unused = -1
    }
}
