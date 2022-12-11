using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerState/IdleState")]
public class IdleState : PlayerBaseState
{
    public override void Enter()
    {

        Debug.Log("Enter IdleState");
    }

    public override void Run()
    {
        
    }
}
