using UnityEngine;
using System;

public class BaseState : MonoBehaviour
{
    protected PlayerMotor playerMotor;
    public string stateName;
    
    // entering the state
    public virtual void Construct()
    {
        
    }

    // leaving the state
    public virtual void Destruct()
    {
        
    }

    // switching 
    public virtual void Transition()
    {
        
    }

    private void Awake()
    {
        playerMotor = GetComponent<PlayerMotor>();
    }

    public virtual void MoveToPoint(Vector3 point)
    {
        Debug.LogError("Process motion isn't implemented in " + this.ToString());
    }
}
