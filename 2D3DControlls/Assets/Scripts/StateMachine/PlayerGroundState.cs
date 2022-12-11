using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerState/GroundState")]
public class PlayerGroundState : PlayerBaseState
{
    public override void Enter()
    {
        jumpCount = 0; 
        Player.gravity = 25;
        Player.staticFrictionCoefficient = 0.4f;
        Player.kineticFrictionCoefficient = 0.2f;
        Debug.Log("Enter GroundState" + jumpCount);
        
    }

    public override void Run()
    {
        //Debug.Log(Player.maxSpeed);
        if (Player.isJumpPressed && Player.GroundCheck())
        {
            Debug.Log(Player.isJumpPressed);
            stateMachine.TransitionTo<PlayerFlyState>();
        }
        if(!Player.GroundCheck() && Player.velocity.y < 0)
        {
            stateMachine.TransitionTo<FallingState>();
        }


        if (Player.isRunning)
        {
            stateMachine.TransitionTo<PlayerRunState>();
        }

        Vector3 gravityMovement = Vector3.down * Player.gravity * Time.deltaTime;
        Player.velocity += gravityMovement;



    }
   
}
