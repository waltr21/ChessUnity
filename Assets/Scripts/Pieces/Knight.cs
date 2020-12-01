using Assets.Scripts.ServerClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : ChessPiece
{

    public Knight() : base() { }

    public override bool MoveValid(int toRow, int toCol, bool move)
    {
        if (board.GameState == GameState.InitialMoves)
        {
            if (base.InitMove(toRow, toCol))
            {
                if (move) base.Move(toRow, toCol);
                return true;
            }
            else return false;
        }

        if (!base.MoveValid(toRow, toCol, move)) return false;

        int rowShift = Mathf.Abs(this.row - toRow);
        int colShift = Mathf.Abs(this.col - toCol);

        if (colShift <= 2 && rowShift <= 2)
        {
            if (colShift == 0 || rowShift == 0) return false;
            if (Mathf.Abs(colShift - rowShift) != 1) return false;
        }
        else return false;

        if (move) base.Move(toRow, toCol);
        return true;
    }
}
