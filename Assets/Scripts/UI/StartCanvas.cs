using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts.ServerClient;
using System.Collections;
using System.IO;
using System;
using TMPro;
using System.Collections.Generic;

public class StartCanvas : MonoBehaviour
{
    private AssetBundle assets;
    private string[] scenePaths;
    public Button HostButton;
    public Button JoinButton;
    public TMP_InputField ServerName;
    public TMP_InputField Password;
    public InputField IpOverride;
    public Scene GameScene;
    public static UserClient uc;
    private static bool GameFound;
    public Text ErrorMessageText;
    public static string errorMessage;
    public Texture2D basic;
    public Texture2D pointer;
    private CursorMode cursorMode = CursorMode.ForceSoftware;
    private Vector2 hotSpot = Vector2.zero;

    private int buttonIndex;
    private List<TMP_InputField> allFields;

    // Start is called before the first frame update
    void Start()
    {
        uc = new UserClient();
        uc.StartMenu = this;
        GameFound = false;
        HostButton.onClick.AddListener(HostClicked);
        JoinButton.onClick.AddListener(JoinClicked);
        errorMessage = "";
        IpOverride.text = "10.0.0.18";
        Cursor.SetCursor(basic, hotSpot, cursorMode);
        buttonIndex = 0;
        allFields = new List<TMP_InputField>();
        allFields.Add(ServerName);
        allFields.Add(Password);
        TabInputs();
    }

    private void TabInputs()
    {
        allFields[buttonIndex].Select();
        buttonIndex++;
        if (buttonIndex >= allFields.Count) buttonIndex = 0;
    }

    private void Update()
    {
        if (GameFound)
        {
            SceneManager.LoadScene(1);
            GameFound = false;
        }
        if (!errorMessage.Equals(ErrorMessageText.text))
        {
            ErrorMessageText.text = errorMessage;
        }
        if (Input.GetKeyDown("tab"))
        {
            TabInputs();
        }
    }

    void HostClicked()
    {
        Debug.Log("Host clicked");
        if (!uc.connected)
        {
            OverrideIp();
            uc.ConnectToServer();
        }
        if (uc.connected)
        {
            GameStartPacket gsp = new GameStartPacket(ServerName.text, Password.text);
            //Wait for client thread to set up socket connections. 
            float stamp = Time.realtimeSinceStartup;
            while (!uc.isReady())
            {
                if (Time.realtimeSinceStartup - stamp > 5) return;
                continue;
            }
            uc.UserTeam = 0;
            uc.Send(gsp);
        }
    }
    
    void JoinClicked()
    {
        Debug.Log("Join clicked");
        bool res;
        if (!uc.connected)
        {
            OverrideIp();
            res = uc.ConnectToServer();
        }
        else
        {
            res = true;
        }
        if (res)
        {
            GameStartPacket gsp = new GameStartPacket(ServerName.text, Password.text);
            gsp.type = PacketType.Join;
            //Wait for client thread to set up socket connections. 
            float stamp = Time.realtimeSinceStartup;
            while (!uc.isReady())
            {
                if (Time.realtimeSinceStartup - stamp > 5) return;
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

    public static void SetError(string m)
    {
        //Some error message to the user..
        errorMessage = m;
    }

    public void OverrideIp()
    {
        if (IpOverride.text.Length > 0)
        {
            uc.IP = IpOverride.text;
        }
    }
}
