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
    
    [SerializeField] private GameObject joystickUI;
    [SerializeField] private GameObject gameOverDeathScreen;
    [SerializeField] private GameObject gameOverWinScreen;
    [Space]
    [SerializeField] private Text goalAmountText;
    [SerializeField] private Text currentAmountText;
    [SerializeField] private Toggle rocketToggle;
    [Space]
    [SerializeField] private GameObject infoScreen;
    [SerializeField] private float infoShowingTime = 2.0f;
    [Space]
    // GameObjects references
    [SerializeField] private GameObject pickaxeGameObject;
    [SerializeField] private GameObject gunGameObject;
    [SerializeField] private GameObject missileLauncher;

    private GameObject _hiddenItem = null;
    
    private PlayerMotor _playerMotor;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;

        _playerMotor = GameObject.Find("Player").GetComponent<PlayerMotor>();
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
        joystickUI.SetActive(false);
    }

    public void ShowWinScreen()
    {
        gameOverWinScreen.SetActive(true);
        joystickUI.SetActive(false);
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
            _playerMotor.TakePickaxe();
        }
        else
        {
            pickaxeGameObject.SetActive(false);
            gunGameObject.SetActive(true);
            _playerMotor.TakeGun();

            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                IntroMiningEvent.SendGunSelected();
            }
        }
    }

    public void RocketMissileToggle(bool value)
    {
        if (value) // True == rocket / False == nothing
        {
            _playerMotor.StartRocketing();
            missileLauncher.SetActive(true);
            
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                IntroMiningEvent.SendRocketSelected();
            }
        }
        else
        {
            _playerMotor.StopRocketing();
            missileLauncher.SetActive(false);
        }
    }

    public void ShowInfoScreen()
    {
        infoScreen.SetActive(true);
        if (_playerMotor.IsRocketing)
        {
            _playerMotor.StopRocketing();
            ExitRocketing();
        }

        if (_playerMotor.IsFighting)
        {
            gunGameObject.SetActive(false);
            pickaxeGameObject.SetActive(true);
            _playerMotor.TakePickaxe();
        }
        Invoke(nameof(HideInfoScreen), infoShowingTime);
    }

    private void HideInfoScreen()
    {
        infoScreen.SetActive(false);
    }

    public void ExitRocketing()
    {
        rocketToggle.isOn = false;
    }

    public void HideActiveItem()
    {
        if (pickaxeGameObject.activeSelf)
        {
            _hiddenItem = pickaxeGameObject;
        }
        else _hiddenItem = gunGameObject;
        
        _hiddenItem.gameObject.SetActive(false);
    }

    public void ShowHiddenItem()
    {
        _hiddenItem.gameObject.SetActive(true);
    }
}
