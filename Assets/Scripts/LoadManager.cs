using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LoadManager : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private List<Tooltip> tooltips = new List<Tooltip>();
    [SerializeField] private Text tooltipPlaceholder;

    private int _currentTooltipIndex;

    public void LoadLevel(string levelName)
    {
        loadingScreen.SetActive(true);
        //SceneManager.LoadSceneAsync(levelName);
        StartCoroutine(LoadLevelCoroutine(levelName));
        int randomTooltipIndex = Random.Range(0, tooltips.Count);
        tooltipPlaceholder.text = tooltips[randomTooltipIndex].text;
    }
    
    private static IEnumerator LoadLevelCoroutine(string sceneName)
    {
        yield return new WaitForSeconds(3f);
        var asyncLoadLevel = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        while (!asyncLoadLevel.isDone){
            yield return null;
        }
    }

    public void ShowNextTooltip()
    {
        tooltipPlaceholder.text = tooltips[_currentTooltipIndex++ % tooltips.Count].text;
    }

    public void ShowPreviousTooltip()
    {
        _currentTooltipIndex--;
        if (_currentTooltipIndex < 0)
        {
            _currentTooltipIndex = tooltips.Count - 1;
        }
        tooltipPlaceholder.text = tooltips[_currentTooltipIndex].text;
    }
}

[Serializable]
public class Tooltip
{
    public string text;
}
