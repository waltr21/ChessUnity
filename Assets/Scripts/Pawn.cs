using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece
{
    private int turnCount;

    public Pawn() : base()
    {
        turnCount = 0;
    }

    public override bool MoveValid(int toRow, int toCol, bool move)
    {
        if (board.GameState == 0)
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

        //Changes for attacking...
        if (colShift > 0) return false;

        if (this.team == 0)
        {
            if (this.row > toRow) return false;
        }
        else
        {
            if (this.row < toRow) return false;
        }
        
        if (turnCount == 0)
        {
            if (rowShift > 2) return false;
        }
        else
        {
            if (rowShift > 1) return false;
        }

        if (rowShift > 1) {
            if (!CanSlide(toRow, toCol)) return false;
        }
        if (move)
        {
            base.Move(toRow, toCol);
            turnCount++;
        }
        return true;
    }

    private bool CanSlide(int toRow, int toCol)
    {
        if (this.team == 0)
        {
            if (board.GetCell(toRow  - 1, toCol).piece != null) return false;
        }
        else
        {
            if (board.GetCell(toRow + 1, toCol).piece != null) return false;
        }
        return true;
    }
}
