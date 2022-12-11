using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerState/PlayerRunState")]
public class PlayerRunState : PlayerBaseState
{

    public override void Enter()
    {
        Player.gravity = 25;
        Player.staticFrictionCoefficient = 0.1f;
        Player.kineticFrictionCoefficient = 0f;
        Debug.Log("Enter PlayerRunState");

    }

    public override void Run()
    {
        if (!Input.GetKey(KeyCode.LeftControl))
        {
            Player.isRunning = false; 
            Player.maxSpeed = 20f;
            //Debug.Log("Här" + Player.maxSpeed);
            stateMachine.TransitionTo<PlayerGroundState>();
            
        }
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < airJumps)
        {
            stateMachine.TransitionTo<PlayerFlyState>();
        }
        if (Player.isRunning)
        {
            Player.maxSpeed = Player.runMaxSpeed;
        }
        //Debug.Log(Player.maxSpeed);


    }
}
