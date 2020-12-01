using Assets.Scripts.ServerClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : ChessPiece
{

    public King() : base() { }

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

        if (rowShift > 1) return false;
        if (colShift > 1) return false;

        if (move) base.Move(toRow, toCol);
        return true;
    }

    public override void Capture()
    {
        captured = true;
        desiredPos = capturedPos;

        board.SendLose();
    }
}