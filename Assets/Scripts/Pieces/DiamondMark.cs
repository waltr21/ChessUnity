using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondMark : MonoBehaviour
{
    public Vector3 desiredPos;
    public Vector3 desiredScale;
    public Vector3 originalScale;
    private float heightBuffer;
    private float lerpSpeed;

    public Material friendMat;
    public Material enemyMat;
    private int matIndex;
    private List<Material> mats;

    void Start()
    {
        desiredPos = transform.position;
        originalScale = transform.localScale;
        desiredScale = originalScale;
        heightBuffer = 0.5f;
        lerpSpeed = 7.0f;
        mats = new List<Material>();
        mats.Add(friendMat);
        mats.Add(enemyMat);
        matIndex = 0;
        SetMaterial(matIndex);
    }

    void Update()
    {
        this.transform.position = Vector3.Lerp(transform.position, desiredPos, lerpSpeed * Time.deltaTime);
        this.transform.localScale = Vector3.Lerp(transform.localScale, desiredScale, lerpSpeed * Time.deltaTime);
    }

    private void SetMaterial(int i)
    {
        if (i != matIndex)
        {
            transform.GetChild(0).GetComponent<Renderer>().material = mats[i];
            matIndex = i;
        }
    }

    // Pos of the piece. Will be increased to float above.
    public void SetDesired(ChessPiece p)
    {
        Vector3 d = p.transform.position;
        float height = p.transform.GetChild(0).gameObject.GetComponent<Renderer>().bounds.size.y;
        d += new Vector3(0, height + heightBuffer, 0);
        desiredPos = d;
        Hide(false);
        if (p.team == p.board.team)
        {
            SetMaterial(0);
            return;
        }
        SetMaterial(1);
    }

    public void Hide(bool b)
    {
        if (b) desiredScale = new Vector3(0, 0, 0);
        else desiredScale = originalScale;
    }
}
