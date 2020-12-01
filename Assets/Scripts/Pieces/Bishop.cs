using Assets.Scripts.ServerClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : ChessPiece
{

    public Bishop() : base() { }

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
        if (rowShift != colShift) return false;

        if (!CanSlide(toRow, toCol)) return false;

        if (move) base.Move(toRow, toCol);
        return true;
    }

    private bool CanSlide(int toRow, int toCol)
    {
        int rowInc = 1;
        int colInc = 1;
        int tempR = this.row;
        int tempC = this.col;

        int rowShift = Mathf.Abs(this.row - toRow);

        if (this.row > toRow) rowInc = -1;

        if (this.col > toCol) colInc = -1;

        for (int i = 0; i < rowShift; i++)
        {
            tempR += rowInc;
            tempC += colInc;
            if (i == rowShift - 1)
            {
                ChessPiece p = board.GetCell(tempR, tempC).piece;
                if (p)
                {
                    if (p.team != board.team) return true;
                }
            }
            if (board.GetCell(tempR, tempC).piece != null) return false;
        }
        return true;
    }
}
