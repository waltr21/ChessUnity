using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEnd : MonoBehaviour
{
    private Color GreenCol, RedCol;
    public TextMeshProUGUI Label;
    public GameObject EndGameCanvas;
    public Button MainMenu;

    // Start is called before the first frame update
    void Start()
    {
        GreenCol = new Color(0.23f, 1f, 0.35f, 1f);
        RedCol = new Color(1f, 0.38f, 0.28f, 1f);
        ShowCanvas(false);
        MainMenu.onClick.AddListener(MainClicked);
    }

    public void ShowCanvas(bool b)
    {
        EndGameCanvas.SetActive(b);
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
