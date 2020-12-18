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
    private Vector3 NormalPos;
    private Vector3 OffScreenPos;
    private Vector3 DesiredPos;

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
        //NormalPos = this;
        OffScreenPos = new Vector3(NormalPos.x, -500f, NormalPos.z);
        DesiredPos = OffScreenPos;
        ShowCanvas(false);
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, DesiredPos, 5.0f * Time.deltaTime);
    }

    public void ShowCanvas(bool b)
    {
        EndGameCanvas.SetActive(b);
        return;
        if (b)
        {
            DesiredPos = NormalPos;
            return;
        }
        DesiredPos = OffScreenPos;
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
