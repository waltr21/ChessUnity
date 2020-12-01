using Assets.Scripts.ServerClient;
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

        if (colShift == 0 && board.GetCell(toRow, toCol).piece != null) return false;

        if (CanAttack(toRow, toCol))
        {
            if (move)
            {
                base.Move(toRow, toCol);
                turnCount++;
            }
            return true;
        }

        if (colShift > 0) return false;

        if (this.team == 0)
        {
            //attacking pawn.
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

    //Only called for diagonal cells.
    private bool CanAttack(int r, int c)
    {
        Cell tempCell = board.GetCell(r, c);
        if (tempCell != null)
        {
            ChessPiece p = tempCell.piece;

            //There is a better way of doing this but it is 1am and I am tired. This should work though....
            if (team == 0)
            {
                if(r == row + 1 && c == col + 1)
                {
                    if (p != null)
                    {
                        if (p.team != team) return true;
                    }
                }
                if (r == row + 1 && c == col - 1)
                {
                    if (p != null)
                    {
                        if (p.team != team) return true;
                    }
                }
            }
            else
            {
                if (r == row - 1 && c == col + 1)
                {
                    if (p != null)
                    {
                        if (p.team != team) return true;
                    }
                }
                if (r == row - 1 && c == col - 1)
                {
                    if (p != null)
                    {
                        if (p.team != team) return true;
                    }
                }
            }
        }

        return false;
    }
}
