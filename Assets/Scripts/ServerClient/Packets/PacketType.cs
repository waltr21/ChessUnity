using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.ServerClient
{
    enum PacketType
    {
        Host,
        Join,
        Checkup,
        Found,
        NotFound,
        Error,
        InitMove,
        StandardMove
    }
}
