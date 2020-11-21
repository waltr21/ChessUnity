using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPiece : MonoBehaviour
{
    public int row;
    public int col;
    public Board board;
    public int team;
    public Vector3 desiredPos;
    public string Id;
    public Vector3 capturedPos;
    public bool captured = false;

    /**
     * 0 - Dead
     * 1 - alive;
     * Need more?
     **/
    public int state;

    public void Update()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, this.desiredPos, 5.0f * Time.deltaTime);
    }

    public ChessPiece()
    {
        this.row = 0;
        this.col = 0;
        this.team = 0;
    }

    public void SetId(int n)
    {
        this.Id = this.team + "|" + n;
        SetCapturedPos();
    }

    public void Capture()
    {
        captured = true;
        desiredPos = capturedPos;
    }

    public void SetCapturedPos()
    {
        string objName = this.Id.Replace("|", "");
        GameObject pos = GameObject.Find(objName);
        capturedPos = pos.transform.position;
    }

    public void Travel(Vector3 desired)
    {
        this.desiredPos = new Vector3(desired.x, 0, desired.z);
    }

    public bool Move(int r, int c, bool sendPacket=true)
    {
        bool success = true;
        if (sendPacket)
        {
            success = this.board.SendMove(Id, this.row, this.col, r, c);
        }

        if (board.GetCell(r, c).piece != null)
        {
            board.GetCell(r, c).piece.Capture();
        }

        this.board.GetCell(this.row, this.col).piece = null;
        this.board.GetCell(r, c).piece = this;
        this.row = r;
        this.col = c;

        this.Travel(this.board.GetCell(r,c).cellRef.transform.position);
        return true; 
    }

    public virtual bool MoveValid(int toRow, int toCol, bool move)
    {
        if (captured) return false;
        if (toRow > board.size - 1 || toRow < 0) return false;
        if (toCol > board.size - 1 || toCol < 0) return false;
        if (this.row == toRow && this.col == toCol) return false;
        if (board.GetCell(toRow, toCol).piece != null)
        {
            if (board.GetCell(toRow, toCol).piece.team == this.team) return false;
        }
        return true;
    }

    public bool InitMove(int toRow, int toCol)
    {
        if (board.GameState != 0) return false;

        if (team == 0)
        {
            if (toRow <= 1 && board.GetCell(toRow, toCol).piece == null) return true;
        }
        else
        {
            if (toRow >= 6 && board.GetCell(toRow, toCol).piece == null) return true;
        }
        return false;
    }
}
