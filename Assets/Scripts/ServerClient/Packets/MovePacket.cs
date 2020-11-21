using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.ServerClient
{
    public class MovePacket : PacketBase
    {
        public string pieceId;
        public int fromRow;
        public int fromCol;
        public int toRow;
        public int toCol;

        public MovePacket(string pieceId, int fromRow, int fromCol, int toRow, int toCol)
        {
            this.pieceId = pieceId;
            this.fromRow = fromRow;
            this.fromCol = fromCol;
            this.toRow = toRow;
            this.toCol = toCol;
        }
    }
}
