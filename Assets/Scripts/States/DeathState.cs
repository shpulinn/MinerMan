using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : BaseState
{
    public override void Construct()
    {
        stateName = "Death";
    }

    public override void Transition()
    {
        ShowDeathScreen();
    }

    private void ShowDeathScreen()
    {
        UIController.Instance.ShowDeathScreen();
    }
}
