using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroUIController : MonoBehaviour
{
    [SerializeField] private GameObject gunArrowImage;
    [SerializeField] private GameObject rocketArrowImage;
    [SerializeField] private GameObject enemiesZone;
    [SerializeField] private Toggle gunToggle;
    [SerializeField] private GameObject gunImage;
    [SerializeField] private Toggle rocketToggle;

    private void Start()
    {
        // IntroMiningEvent.OnFirstCrystallMined += ShowGunArrowImage;
        // IntroMiningEvent.OnSecondCrystallMined += ShowRocketArrowImage;
        // IntroMiningEvent.OnGunSelected += GunSelected;
        // IntroMiningEvent.OnRocketSelected += RocketSelected;
        // IntroMiningEvent.OnRocketLaunched += RocketLaunched;
        //
        // gunToggle.interactable = false;
        // gunImage.SetActive(false);
        // rocketToggle.interactable = false;
    }

    private void OnEnable()
    {
        IntroMiningEvent.OnFirstCrystallMined += ShowGunArrowImage;
        IntroMiningEvent.OnSecondCrystallMined += ShowRocketArrowImage;
        IntroMiningEvent.OnGunSelected += GunSelected;
        IntroMiningEvent.OnRocketSelected += RocketSelected;
        IntroMiningEvent.OnRocketLaunched += RocketLaunched;

        gunToggle.interactable = false;
        gunImage.SetActive(false);
        rocketToggle.interactable = false;
    }

    private void OnDestroy()
    {
        IntroMiningEvent.OnFirstCrystallMined -= ShowGunArrowImage;
        IntroMiningEvent.OnSecondCrystallMined -= ShowRocketArrowImage;
        IntroMiningEvent.OnGunSelected -= GunSelected;
        IntroMiningEvent.OnRocketSelected -= RocketSelected;
        IntroMiningEvent.OnRocketLaunched -= RocketLaunched;
    }

    private void ShowGunArrowImage()
    {
        gunArrowImage.SetActive(true);
        gunImage.SetActive(true);
        gunToggle.interactable = true;
    }

    private void ShowRocketArrowImage()
    {
        if (gunArrowImage.activeSelf)
        {
            gunArrowImage.SetActive(false);
        }
        rocketArrowImage.SetActive(true);
        rocketToggle.interactable = true;
    }

    private void GunSelected()
    {
        gunArrowImage.SetActive(false);
    }

    private void RocketSelected()
    {
        rocketArrowImage.SetActive(false);
        enemiesZone.SetActive(true);
    }

    private void RocketLaunched()
    {
        enemiesZone.SetActive(false);
    }
}
