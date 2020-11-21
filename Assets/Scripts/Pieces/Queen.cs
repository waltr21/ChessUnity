using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : ChessPiece
{
    public Queen() : base() { }

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

        bool valid = false;

        int rowShift = Mathf.Abs(this.row - toRow);
        int colShift = Mathf.Abs(this.col - toCol);
        if (rowShift == colShift) valid = true;
        if (rowShift > 0 && colShift == 0) valid = true;
        if (colShift > 0 && rowShift == 0) valid = true;

        if (!valid) return false;

        if (!CanSlide(toRow, toCol)) return false;

        if (move) base.Move(toRow, toCol);
        return true;
    }

    private bool CanSlide(int toRow, int toCol)
    {
        int rowShift = Mathf.Abs(this.row - toRow);
        int colShift = Mathf.Abs(this.col - toCol);

        if (rowShift == colShift)
        {
            if (!CanSlideBishop(toRow, toCol)) return false;
        }
        else
        {
            if (!CanSlideRook(toRow, toCol)) return false;
        }

        return true;
    }

    private bool CanSlideBishop(int toRow, int toCol)
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

    private bool CanSlideRook(int toRow, int toCol)
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
