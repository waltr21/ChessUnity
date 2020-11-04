using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User_Actions : MonoBehaviour
{
    public Camera cam;
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
        if (hit.transform.tag.Equals("Chess_Piece"))
        {
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

            curHit = hit.transform.parent.transform;

            curHit.position = new Vector3(curHit.position.x, (heightVal + (Mathf.Sin((Time.time - stamp) * speed) * heightVal)), curHit.position.z);
        }
        //else if (selected != null)
        //{
        //    selected.transform.position = new Vector3(selected.transform.position.x, (heightVal + (Mathf.Sin((Time.time - stamp) * speed) * heightVal)), selected.transform.position.z);
        //}
    }
}
