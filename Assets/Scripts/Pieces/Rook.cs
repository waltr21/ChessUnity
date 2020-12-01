using Assets.Scripts.ServerClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : ChessPiece
{

    public Rook() : base() { }

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
        if (rowShift > 0  && colShift > 0) return false;

        if (!CanSlide(toRow, toCol)) return false;

        if (move) base.Move(toRow, toCol);
        return true;
    }

    private bool CanSlide(int toRow, int toCol)
    {
        int start;
        int end;

        int rowShift = Mathf.Abs(this.row - toRow);
        int colShift = Mathf.Abs(this.col - toCol);

        if (rowShift == 1 || colShift == 1) return true;

        if (rowShift > 0)
        {
            if (toRow > this.row)
            {
                start = this.row;
                end = toRow;
            }
            else
            {
                start = toRow;
                end = this.row;
            }
            for (int i = start + 1; i < end; i++)
            {
                if (board.GetCell(i, this.col).piece != null) return false;
            }
        }
        else
        {
            if (toCol > this.col)
            {
                start = this.col;
                end = toCol;
            }
            else
            {
                start = toCol;
                end = this.col;
            }
            for (int i = start + 1; i < end; i++)
            {
                if (board.GetCell(this.row, i).piece != null) return false;                
            }
        }
        return true;
    }
}
