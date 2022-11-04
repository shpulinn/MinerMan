using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [SerializeField] private PlayerMotor playerMotor;
    [SerializeField] private GameObject gameOverDeathScreen;
    [SerializeField] private GameObject gameOverWinScreen;
    [SerializeField] private Text goalAmountText;
    [SerializeField] private Text currentAmountText;

    // GameObjects references
    [SerializeField] private GameObject pickaxeGameObject;
    [SerializeField] private GameObject gunGameObject;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            InputManager.Instance.ToggleControl(true);
        }
        else
        {
            InputManager.Instance.ToggleControl(false);
        }
    }

    public void ShowDeathScreen()
    {
        gameOverDeathScreen.SetActive(true);
    }

    public void ShowWinScreen()
    {
        gameOverWinScreen.SetActive(true);
    }

    public void SetGoalAmount(int amount)
    {
        goalAmountText.text = amount.ToString();
    }

    public void UpdateCurrentScore(int amount)
    {
        currentAmountText.text = amount.ToString();
    }

    public void PlayAgainButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitButtonClick()
    {
        Application.Quit();
    }

    public void ActionToggle(bool value)
    {
        if (value) // True == mining / False == fighting
        {
            gunGameObject.SetActive(false);
            pickaxeGameObject.SetActive(true);
            playerMotor.TakePickaxe();
        }
        else
        {
            pickaxeGameObject.SetActive(false);
            gunGameObject.SetActive(true);
            playerMotor.TakeGun();
        }
    }

    public void RocketMissileToggle(bool value)
    {
        if (value) // True == rocket / False == nothing
        {
            playerMotor.StartRocketing();
        }
        else
        {
            playerMotor.StopRocketing();
        }
    }
}
