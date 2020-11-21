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
    public int team;
    public int turn;

    public GameObject Bishop;
    public GameObject Rook;
    public GameObject Queen;
    public GameObject Knight;
    public GameObject King;
    public GameObject Pawn;
    public GameObject Unknown;
    private int PieceCount;
    private static UserClient ClientEngine;

    private List<ChessPiece> allPieces;

    /*
     * This represents the current move from the server. Put here to allow the Unity to process this move 
     * on the main thread. Will be set to null after the main thread does what it needs from its information. 
     * 
     * (Only the main thread can play around with MonoBehaviour inherrited objects)
     */
    public MovePacket ServerMove;

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
        allPieces = new List<ChessPiece>();

        ClientEngine = StartCanvas.uc;
        ClientEngine.BoardRef = this;
        this.team = ClientEngine.UserTeam;
        this.turn = this.team;
        ServerMove = new MovePacket("",0,0,0,0);
        ServerMove.type = PacketType.Unused;
        InitPieces();
    }

    public void Update()
    {
        foreach (Cell c in board)
        {
            c.Travel();
        }
        ProcessServerMove();
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
        PieceCount = 0;
    }

    public ChessPiece AddPiece(GameObject type, int row, int col)
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
        allPieces.Add(result);
        SetPieceMaterial(result);
        return result;
    }

    private void SetPieceMaterial(ChessPiece p)
    {
        if (p.team == 0)
        {
            Material mat = Resources.Load<Material>("PieceMaterials/Piece_Black");
            GameObject temp  = p.transform.GetChild(0).gameObject;
            temp.GetComponent<MeshRenderer>().material = mat;
           
        }
        if (p.team == 1)
        {
            Material mat = Resources.Load<Material>("PieceMaterials/Piece_White");
            GameObject temp = p.transform.GetChild(0).gameObject;
            temp.GetComponent<MeshRenderer>().material = mat;
        }
    }

    public Cell GetCell(int r, int c)
    {
        if (r >= size || r < 0)
        {
            return null;
        }
        if (c >= size || c < 0)
        {
            return null;
        }
        return board[r, c];
    }

    public ChessPiece GetPiece(Transform pieceRef)
    {
        foreach (ChessPiece p in allPieces)
        {
            if (pieceRef == p.transform) return p;
        }
        return null;
    }

    public ChessPiece GetPiece(string id)
    {
        foreach (ChessPiece p in allPieces)
        {
            if (p.Id.Equals(id)) return p;
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

    // Should really be done by the server.
    public void UpdateGameState()
    {
        //Just for one side for now. 
        if (GameState == 0) {
            bool filled = true;
            if (team == 0)
            {
                for (int r = 0; r < 2; r++)
                {
                    for (int c = 0; c < size; c++)
                    {
                        if (GetCell(r, c).piece == null) filled = false;
                    }
                }
            }
            else
            {
                for (int r = this.size - 1; r > 5; r--)
                {
                    for (int c = 0; c < size; c++)
                    {
                        if (GetCell(r, c).piece == null) filled = false;
                    }
                }
            }
            if (filled)
            {
                GameState = 1;
                turn = -1;
                PacketBase pb = new PacketBase
                {
                    type = PacketType.PlayerReady
                };
                ClientEngine.Send(pb);
            }
        }
    }

    public void ProcessServerMove()
    {
        // Thread safe and stuff.
        lock (ServerMove)
        {
            if (ServerMove.type != PacketType.Unused)
            {
                ChessPiece p = null;
                switch (ServerMove.type){
                    case PacketType.InitMove:
                        p = GetPiece(ServerMove.pieceId);
                        if (p != null)
                        {
                            //Because we are updating an oponent do not send info to server (false).
                            p.Move(ServerMove.toRow, ServerMove.toCol, false);
                        }
                        else
                        {
                            int team = int.Parse(ServerMove.pieceId.Split('|')[0]);
                            p = AddPiece(Unknown, ServerMove.toRow, ServerMove.toCol);
                            p.team = team;
                            p.Id = ServerMove.pieceId;
                            SetPieceMaterial(p);
                            p.SetCapturedPos();
                        }
                        break;
                    case PacketType.StandardMove:
                        p = GetPiece(ServerMove.pieceId);
                        if (p != null)
                        {
                            //Because we are updating an oponent do not send info to server (false).
                            p.Move(ServerMove.toRow, ServerMove.toCol, false);
                            IncrTurn();
                        }
                        else
                        {
                            Debug.Log("Something is very wrong... Unknown ID: " + ServerMove.pieceId);
                        }
                        break;
                    case PacketType.SetTurn:

                    //Some unhandled packet information? Toss out?
                    default:
                        break;
                    
                }
                //Reset server move
                ServerMove.type = PacketType.Unused;
            }
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
            IncrTurn();
        }
        return true;
    }

    public void IncrTurn()
    {
        if (GameState == 1)
        {
            turn++;
            if (turn >= 2)
            {
                turn = 0;
            }
        }
    }
}
