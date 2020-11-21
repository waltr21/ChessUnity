using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public int row, col;
    public Transform cellRef;
    public ChessPiece piece;
    public int OriginalMaterial;
    public Vector3 desiredPos;

    public Cell(int row, int col, Transform cellRef)
    {
        this.row = row;
        this.col = col;
        this.cellRef = cellRef;
        this.desiredPos = cellRef.position;
        string matName = cellRef.GetComponent<Renderer>().material.name;
        if (matName.ToLower().Contains("dark")) OriginalMaterial = 0;
        else OriginalMaterial = 1;
    }

    public void SetMaterial(Material m)
    {
        cellRef.GetComponent<Renderer>().material = m;
    }

    public void ResetHighlight(Material light, Material dark)
    {
        if (OriginalMaterial == 0) cellRef.GetComponent<Renderer>().material = dark;
        else cellRef.GetComponent<Renderer>().material = light;
    }

    public void Travel()
    {
        cellRef.position = Vector3.Lerp(cellRef.position, this.desiredPos, 15f * Time.deltaTime);
    }
}
