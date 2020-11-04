using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts.ServerClient;
using System.Collections;
using System.IO;

public class StartCanvas : MonoBehaviour
{
    private AssetBundle assets;
    private string[] scenePaths;
    public Button HostButton;
    public Button JoinButton;
    public InputField ServerName;
    public InputField Password;
    public Scene GameScene;
    public static UserClient uc;
    private static bool GameFound;

    // Start is called before the first frame update
    void Start()
    {
        uc = new UserClient();
        uc.StartMenu = this;
        GameFound = false;
        HostButton.onClick.AddListener(HostClicked);
        JoinButton.onClick.AddListener(JoinClicked);
    }

    private void Update()
    {
        if (GameFound)
        {
            SceneManager.LoadScene(1);
        }
    }

    void HostClicked()
    {
        Debug.Log("Host clicked");
        bool res = uc.ConnectToServer();
        if (res)
        {
            GameStartPacket gsp = new GameStartPacket(ServerName.text, Password.text);
            //Wait for client thread to set up socket connections. 
            while (!uc.isReady())
            {
                continue;
            }
            uc.UserTeam = 0;
            uc.Send(gsp);
            SceneManager.LoadScene(1);
        }
    }
    
    void JoinClicked()
    {
        Debug.Log("Join clicked");
        bool res = uc.ConnectToServer();
        if (res)
        {
            GameStartPacket gsp = new GameStartPacket(ServerName.text, Password.text);
            gsp.type = PacketType.Join;
            //Wait for client thread to set up socket connections. 
            while (!uc.isReady())
            {
                continue;
            }
            uc.UserTeam = 1;
            uc.Send(gsp);
        }
    }

    public static void SetGameFound(bool b)
    {
        GameFound = b;
    }

    public static void GameNotFound()
    {
        //Some error message to the user..
        Debug.Log("No game found.");
        return;
    }
}
