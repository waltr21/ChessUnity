using UnityEngine;
using TMPro;
using Assets.Scripts.UI;
using Assets.Scripts.ServerClient;

public class WaitingPlayer : Slider
{
    public TMP_Text WaitingText;
    private string ogText;
    private string ellipseText;
    public int ellispeRate = 1000;
    private float stamp;
    public RectTransform rect;
    public Board board;

    // Start is called before the first frame update
    void Start()
    {
        ellipseText = "";
        ogText = WaitingText.text;
        stamp = Time.time * 1000;
        SetTransformObject(rect);
        SetOffScreen(new Vector3(rect.localPosition.x, -600f, rect.localPosition.z));
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (board.GameState == GameState.WaitingToStart)
        {
            float tempStamp = Time.time * 1000;
            if (tempStamp - stamp > ellispeRate)
            {
                stamp = tempStamp;
                IncrEllipse();
                WaitingText.text = ogText + ellipseText;
            }
        }
        else
        {
            ShowCanvas(false);
        }
    }

    void IncrEllipse()
    {
        ellipseText += ".";
        if (ellipseText.Length > 3)
        {
            ellipseText = "";
        }
    }
}
