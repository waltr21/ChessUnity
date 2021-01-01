using Assets.Scripts.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEnd : UISlider
{
    private Color GreenCol, RedCol;
    public TextMeshProUGUI Label;
    public GameObject EndGameCanvas;
    public Button MainMenu;
    public RectTransform EndGameTransform;

    // Start is called before the first frame update
    void Start()
    {
        //GreenCol = new Color(0.23f, 1f, 0.35f, 1f);
        //RedCol = new Color(1f, 0.38f, 0.28f, 1f);
        //GreenCol = new Color(1f, 0.99f, 0.84f, 1f);
        //RedCol = new Color(1f, 0.99f, 0.84f, 1f);
        RedCol = new Color(1f, 1f, 1f, 1f);
        GreenCol = new Color(1f, 1f, 1f, 1f);
        MainMenu.onClick.AddListener(MainClicked);

        SetTransformObject(EndGameTransform);
        ShowCanvas(false);
        EndGameTransform.localPosition = OffScreenPos;
    }

    public void SetEndMessage(string m)
    {
        Label.text = m;
    }

    public void SetEndMessageGreen()
    {
        Label.faceColor = GreenCol;
    }

    public void SetEndMessageRed()
    {
        Label.faceColor = RedCol;
    }

    public void MainClicked()
    {
        SceneManager.LoadScene(0);
    }
}
