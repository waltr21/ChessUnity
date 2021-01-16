using Assets.Scripts.ServerClient;
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
    public bool selected = false;
    public float animateStamp;
    private float speed = 5.0f;
    private float heightVal = 0.25f;

    // private float placeSpeed = 0.75f;
    private Vector3 startPlace;
    private Vector3 endPlace;
    private float placeTime;
    private float placeHeight = 5.0f;

    /**
     * 0 - Dead
     * 1 - alive;
     * Need more?
     **/
    public int state;

    public void Start()
    {
        this.transform.position = new Vector3(this.transform.position.x, 50F, this.transform.position.z);
        //startPlace = new Vector3(0, 0, 0);
        //endPlace = new Vector3(0, 0, 0);
    }

    public ChessPiece()
    {
        this.row = 0;
        this.col = 0;
        this.team = 0;
    }

    // Animation for lifting the pieces up and placing them down on the board.
    private void GetAnimationPos()
    {
        placeTime += Time.deltaTime * 1.5f;
        if (1 - placeTime > 0)
        {
            Vector3 midPlace = (startPlace + endPlace) * 0.5f;
            midPlace.y += placeHeight;
            var temp = Mathf.Pow(1 - placeTime, 2) * startPlace + 2 * (1 - placeTime) * placeTime * midPlace + Mathf.Pow(placeTime, 2) * endPlace;
            desiredPos = temp;
        }
        else
        {
            startPlace = transform.position;
        }
    }

    public void Update()
    {
        GetAnimationPos();
        this.transform.position = Vector3.Lerp(this.transform.position, this.desiredPos, 5.0f * Time.deltaTime);
        if (selected)
        {
            SelectedAnimate();
        }
    }

    public void SelectedAnimate()
    {
        transform.position = new Vector3(transform.position.x, (heightVal + (Mathf.Sin((Time.time - animateStamp) * speed) * heightVal)), transform.position.z);
    }

    public void SetAnimate()
    {
        animateStamp = Time.time - ((1.5f * Mathf.PI) / speed);
        selected = true;
    }

    public void ResetAnimation()
    {
        selected = false;
    }

    public void SetId(int n)
    {
        this.Id = this.team + "|" + n;
        SetCapturedPos();
    }

    public virtual void Capture()
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
        placeTime = 0f;
        endPlace = new Vector3(desired.x, 0, desired.z);
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
        if (board.GameState == GameState.WaitingToStart) return false;
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
        if (board.GameState != GameState.InitialMoves) return false;

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

    public void DeleteObject()
    {
        Destroy(gameObject);
    }
}
