using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User_Actions : MonoBehaviour
{
    public Camera cam;
    public GameObject camTri;
    public float heightVal;
    public float speed;
    private float stamp;
    public Board board;
    public ChessPiece selected;
    public DiamondMark diamond;

    public Texture2D basic;
    public Texture2D pointer;
    private CursorMode cursorMode = CursorMode.ForceSoftware;
    private Vector2 hotSpot = Vector2.zero;

    //curHit for the cells
    private Transform curHit;
    private int cursorIndex;

    // Start is called before the first frame update
    void Start()
    {
        stamp = Time.time;
        if (board.team == 1)
        {
            camTri.transform.Rotate(new Vector3(0,180,0));
        }
        SetBasic();
    }

    // Update is called once per frame
    void Update()
    {
        Animate();
        if (Input.GetMouseButtonDown(0))
            UserClick();
    }

    //private void SetPointer()
    //{
    //    if (cursorIndex != 1)
    //    {
    //        cursorIndex = 1;
    //        Cursor.SetCursor(pointer, hotSpot, cursorMode);
    //    }
    //}

    private void SetBasic()
    {
        if (cursorIndex != 0)
        {
            cursorIndex = 0;
            Cursor.SetCursor(basic, hotSpot, cursorMode);
        }
    }

    private void UserClick()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag.Equals("Cell"))
            {
                Cell temp = board.GetCell(hit.transform);
                if (selected)
                {
                    bool moved = selected.MoveValid(temp.row, temp.col, true);
                    if (moved)
                    {
                        selected = null;
                        board.ShowValidCells(selected);
                        this.board.ResetAllCellPos();
                        this.board.UpdateGameState();
                        this.board.ResetAllAnimations();
                        this.board.CheckPromotion();
                    }
                }
                else
                {
                    if (temp.piece.team == board.team)
                    {
                        selected = temp.piece;
                        board.ShowValidCells(selected);
                        temp.piece.SetAnimate();
                    }
                }
            }
            if (hit.transform.tag.Equals("Chess_Piece"))
            {
                ChessPiece p = board.GetPiece(hit.transform.parent);
                this.board.ResetAllAnimations();
                
                // Checking for turn + team of piece.
                if (board.turn != board.team)
                {
                    return;
                }

                if (p.team != board.team)
                {
                    if (selected)
                    {
                        bool moved = selected.MoveValid(p.row, p.col, true);
                        if (moved)
                        {
                            selected = null;
                            board.ShowValidCells(selected);
                            this.board.ResetAllCellPos();
                            this.board.UpdateGameState();
                            this.board.CheckPromotion();
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    p.SetAnimate();
                }
                selected = board.GetPiece(hit.transform.parent);
                board.ShowValidCells(selected);
            }
        }
        else
        {
            this.board.ResetAllAnimations();
        }
    }

    /* For the love of god redo this.
     * Set an animation variable within a chess piece rather than doing the work here. 
     * 
     * For later: Animate the cells that are valid moves. 
     */
    private void Animate()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            AnimatePieces(hit);
            AnimateCells(hit);
        }
        else
        {
            SetBasic();
            diamond.Hide(true);
        }
    }

    private void AnimateCells(RaycastHit hit)
    {
        if (hit.transform.tag.Equals("Cell"))
        {
            if ((hit.transform != curHit) && curHit)
            {
                var tempCell = this.board.GetCell(curHit);
                if (tempCell != null)
                {
                    tempCell.desiredPos = new Vector3(curHit.position.x, 0, curHit.position.z);
                }
            }
            curHit = hit.transform;
            Cell cell = this.board.GetCell(hit.transform);

            //Diamond logic
            if (cell.piece != null)
            {
                diamond.SetDesired(cell.piece);
            }
            else
            {
                diamond.Hide(true);
            }

            //Pointer logic
            if (cell.piece == null && cell.isHighlighted)
            {
                cell.desiredPos = new Vector3(curHit.position.x, 0.25f, curHit.position.z);
                //SetPointer();
            }
            else
            {
                SetBasic();
            }
            
        }
    }

    private void AnimatePieces(RaycastHit hit)
    {
        if (board.turn != board.team)
        {
            SetBasic();
            return;
        }
        if (hit.transform.tag.Equals("Chess_Piece"))
        {
            ChessPiece piece = this.board.GetPiece(hit.transform.parent);
            if (piece.captured) return;
            diamond.SetDesired(piece);
            //SetPointer();
        }
    }
}
