using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerState/JumpState")]
public class JumpState : PlayerBaseState


{
    public override void Enter()
    {
        Debug.Log("Enter FallingState");
    }

    public override void Run()
    {

    }
}
