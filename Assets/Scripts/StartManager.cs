using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    #region SingletonCode
    private static StartManager _instance;
    public static StartManager Instance { get { return _instance; } }

    public Queue<string> queueMessages;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion
    public InputField ipText;

    public bool loadGame = false;
    //public bool connected = false;

    //initial connection to server
    public void ConnectToServer()
    {
        if (ipText.text == "" || ipText.text == null)
        {
            NetworkManager.ConnectToServer("127.0.0.1");
        }
        else
        {
            NetworkManager.ConnectToServer(ipText.text);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (loadGame)
        {
            Debug.Log("Starting Game");
            UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
            loadGame = false;
        }
    }

    public void SetLoadGame(bool b)
    {
        Debug.Log("Setting Game" + b.ToString());
        loadGame = b;
    }
}