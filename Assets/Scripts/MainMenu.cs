using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    public void PlayButtonClick()
    {
        // in player prefs saved index of passed level, so we load next level of that
        if (CheckSaves() <= SceneManager.sceneCountInBuildSettings - 1)
        {
            loadingScreen.SetActive(true);
            SceneManager.LoadScene(CheckSaves() + 1);
        }
    }

    private int CheckSaves()
    {
        return PlayerPrefs.GetInt("LevelIndex", 0);
    }

    public void QuitButtonClick()
    {
        Application.Quit();
    }
}
