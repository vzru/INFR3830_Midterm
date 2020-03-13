using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class SceneSwitcher : MonoBehaviour
{
    public void SwitchScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
}
