using Assets.Scripts.ServerClient;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public Cell[,] board;
    public int size;
    public Material Dark_Mat;
    public Material Light_Mat;
    public Material Highlight_Mat;
    public int GameState;

    public GameObject Bishop;
    public GameObject Rook;
    public GameObject Queen;
    public GameObject Knight;
    public GameObject King;
    public GameObject Pawn;
    private int PieceCount;
    private int team;
    private static UserClient ClientEngine;

    private void Start()
    {
        this.GameState = 0;
        size = 8;
        board = new Cell[size, size];
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                board[i, j] = new Cell(i, j, CellLookup(i, j));
            }
        }

        ClientEngine = StartCanvas.uc;
        ClientEngine.BoardRef = this;
        this.team = ClientEngine.UserTeam;
        InitPieces();
    }

    public void Update()
    {
        foreach (Cell c in board)
        {
            c.Travel();
        }
    }

    public void ResetAllCellPos()
    {
        foreach (Cell c in board)
        {
            c.desiredPos = new Vector3(c.cellRef.position.x, 0, c.cellRef.position.z);
        }
    }

    private void InitPieces()
    {
        if (team == 0)
        {
            for (int i = 0; i < size; i++)
            {
                AddPiece(Pawn, 3, i);
            }
            AddPiece(Rook, 2, 0);
            AddPiece(Rook, 2, 7);
            AddPiece(Knight, 2, 1);
            AddPiece(Knight, 2, 6);
            AddPiece(Bishop, 2, 2);
            AddPiece(Bishop, 2, 5);

            AddPiece(Queen, 2, 4);
            AddPiece(King, 2, 3);
        }
        else if(team == 1)
        {
            for (int i = 0; i < size; i++)
            {
                AddPiece(Pawn, 4, i);
            }
            AddPiece(Rook, 5, 0);
            AddPiece(Rook, 5, 7);
            AddPiece(Knight, 5, 1);
            AddPiece(Knight, 5, 6);
            AddPiece(Bishop, 5, 2);
            AddPiece(Bishop, 5, 5);

            AddPiece(Queen, 5, 3);
            AddPiece(King, 5, 4);
        }
        else
        {
            throw new System.Exception("Team value invalid.");
        }
    }

    public void AddPiece(GameObject type, int row, int col)
    {
        var result = Instantiate(type).GetComponent<ChessPiece>();
        result.team = this.team;
        result.row = row;
        result.col = col;
        result.board = this;
        result.Move(result.row, result.col, false);
        board[result.row, result.col].piece = result;
        result.SetId(PieceCount);
        PieceCount++;
    }

    public Cell GetCell(int r, int c)
    {
        return board[r, c];
    }

    public ChessPiece GetPiece(Transform pieceRef)
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                Cell tempCell = board[i, j];
                if (tempCell.piece && tempCell.piece.transform == pieceRef)
                {
                    return tempCell.piece;
                }
            }
        }
        return null;
    }

    public Cell GetCell(Transform cellRef)
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                Cell tempCell = board[i, j];
                if (tempCell.cellRef == cellRef)
                {
                    return tempCell;
                }
            }
        }
        return null;
    }

    private Transform CellLookup(int r, int c)
    {
        List<Transform> rows = new List<Transform>();
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Transform row = this.transform.GetChild(i);
            if (row.name.Contains(r + ""))
            {
                for (int j = 0; i < row.childCount; j++)
                {
                    Transform boardCell = row.GetChild(j);
                    if (boardCell.name.Contains(c + ""))
                    {
                        return boardCell;
                    }
                }
            }
        }
        return null;
    }

    public void ShowValidCells(ChessPiece p)
    {
        foreach (Cell c in board)
        {
            if (p == null) c.ResetHighlight(Light_Mat, Dark_Mat);
            else
            {
                if (p.MoveValid(c.row, c.col, false)) c.SetMaterial(Highlight_Mat);
                else c.ResetHighlight(Light_Mat, Dark_Mat);
            }
        }
    }

    public void UpdateGameState()
    {
        //Just for one side for now. 
        if (GameState == 0) {
            bool filled = true;
            for (int r = 0; r < 2; r++)
            {
                for (int c = 0; c < size; c++)
                {
                    if (GetCell(r, c).piece == null) filled = false;
                }
            }
            if (filled) GameState = 1;
        }
    }

    public bool SendMove(string pieceId, int fromRow, int fromCol, int toRow, int toCol)
    {
        if (ClientEngine != null)
        {
            MovePacket curMove = new MovePacket(pieceId, fromRow, fromCol, toRow, toCol);
            if (GameState == 0)
            {
                curMove.type = PacketType.InitMove;
            }
            else
            {
                curMove.type = PacketType.StandardMove;
            }
            ClientEngine.Send(curMove);
        }
        return true;
    }
}
