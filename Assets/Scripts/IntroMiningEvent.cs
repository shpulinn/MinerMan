using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroMiningEvent : MonoBehaviour
{
    public static Action OnFirstCrystallMined;
    public static Action OnSecondCrystallMined;
    public static Action OnRocketLaunched;

    public static Action OnGunSelected;
    public static Action OnRocketSelected;

    public static void SendFirstCrystallMined()
    {
        OnFirstCrystallMined?.Invoke();
    }
    
    public static void SendSecondCrystallMined()
    {
        OnSecondCrystallMined?.Invoke();
    }
    
    public static void SendRocketLaunched()
    {
        OnRocketLaunched?.Invoke();
    }

    public static void SendGunSelected()
    {
        OnGunSelected?.Invoke();
    }
    
    public static void SendRocketSelected()
    {
        OnRocketSelected?.Invoke();
    }
}
