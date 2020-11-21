using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User_Actions : MonoBehaviour
{
    public Camera cam;
    public GameObject camTri;
    private Transform curHit;
    public float heightVal;
    public float speed;
    private float stamp;
    public Board board;
    public ChessPiece selected;

    // Start is called before the first frame update
    void Start()
    {
        stamp = Time.time;
        if (board.team == 1)
        {
            camTri.transform.Rotate(new Vector3(0,180,0));
        }
    }

    // Update is called once per frame
    void Update()
    {
        Animate();
        if (Input.GetMouseButtonDown(0))
            UserClick();
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
                    }
                }
            }
            if (hit.transform.tag.Equals("Chess_Piece"))
            {
                ChessPiece p = board.GetPiece(hit.transform.parent);
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
                selected = board.GetPiece(hit.transform.parent);
                board.ShowValidCells(selected);
            }
        }
        return;
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
        //else if (selected != null)
        //{
        //    selected.transform.position = new Vector3(selected.transform.position.x, (heightVal + (Mathf.Sin((Time.time - stamp) * speed) * heightVal)), selected.transform.position.z);
        //}
    }

    private void AnimateCells(RaycastHit hit)
    {
        if (hit.transform.tag.Equals("Cell"))
        {
            if ((hit.transform != curHit) && curHit)
            {
                var tempCell = this.board.GetCell(curHit);
                if (tempCell != null) this.board.GetCell(curHit).desiredPos = new Vector3(curHit.position.x, 0, curHit.position.z);
                
            }
            curHit = hit.transform;
            Cell cell = this.board.GetCell(curHit);
            if (cell.piece == null)
            {
                this.board.GetCell(curHit).desiredPos = new Vector3(curHit.position.x, 0.25f, curHit.position.z);
            }
        }
    }

    private void AnimatePieces(RaycastHit hit)
    {
        if (board.turn != board.team)
        {
            return;
        }
        if (hit.transform.tag.Equals("Chess_Piece"))
        {
            ChessPiece piece = this.board.GetPiece(hit.transform.parent);
            if (piece.captured) return;
            //if (selectehit.transform == selected.transform) return;
            if ((hit.transform.parent.transform != curHit) && curHit)
            {
                //normalize sin wave to start at 0. 
                stamp = Time.time - ((1.5f * Mathf.PI) / speed);
                this.board.ResetAllCellPos();
            }
            //Null safety for first hit.
            if (curHit == null)
            {
                stamp = Time.time - ((1.5f * Mathf.PI) / speed);
            }
            if (piece && piece.team != this.board.team)
            {
                return;
            }

            curHit = hit.transform.parent.transform;

            curHit.position = new Vector3(curHit.position.x, (heightVal + (Mathf.Sin((Time.time - stamp) * speed) * heightVal)), curHit.position.z);
        }
        //else if (selected != null)
        //{
        //    selected.transform.position = new Vector3(selected.transform.position.x, (heightVal + (Mathf.Sin((Time.time - stamp) * speed) * heightVal)), selected.transform.position.z);
        //}
    }
}
