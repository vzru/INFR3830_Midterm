// Script taken from: https://www.youtube.com/watch?v=QbqnDbexrCw

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesScript : MonoBehaviour
{
    public static int lives = 3;
    Text livesText;

    // Start is called before the first frame update
    void Start()
    {
        livesText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        livesText.text = lives + "\nLives Remaining";

        if (lives <= 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Login");
        }
    }
}
