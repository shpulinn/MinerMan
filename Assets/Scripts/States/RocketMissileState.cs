using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMissileState : BaseState
{
    [SerializeField] private GameObject rocketPrefab;
    [SerializeField] private float energyCost = 0.6f;

    private PlayerEnergy _playerEnergy;

    public override void Construct()
    {
        stateName = "Rocket missile";
        _playerEnergy = GetComponent<PlayerEnergy>();
    }
    
    // _______------------____________
    // Animation: player with telephone (radio) calls for air strike
    // ---------_________-------------

    public override void Transition()
    {
        if (InputManager.Instance.Tap)
        {
            Ray ray;
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                ray = Camera.main.ScreenPointToRay(InputManager.Instance.MousePosition);
            } else 
                ray = Camera.main.ScreenPointToRay(UnityEngine.InputSystem.Touchscreen.current.touches[0].position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Instantiate rocket above this point 
                Quaternion rot = new Quaternion(0, 0, 180, -1);
                if (_playerEnergy.CurrentEnergy >= energyCost)
                {
                    _playerEnergy.DecreaseEnergy(energyCost);
                    Instantiate(rocketPrefab, hit.point + Vector3.up * 10, rot);
                } else Debug.Log("Not enough energy!");
            }
        }

        if (playerMotor.IsDead)
        {
            playerMotor.ChangeState(GetComponent<DeathState>());
        }
        // other transitions here:
        // Idle state
        // Running state
        // Fighting state
        // Death state
    }
}
