using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerState/FallingState")]
public class FallingState : PlayerBaseState
{
    private float fallGravity = 50;
    public override void Enter()
    {
        Player.gravity = fallGravity;
        Debug.Log("Enter FallingState");
    }

    public override void Run()
    {
        Player.jumpforce = Player.jumpforce;

        if (Player.GroundCheck())
        {
            stateMachine.TransitionTo<PlayerGroundState>();
        }
        if (Player.isJumpPressed && jumpCount < airJumps)
        {
            Player.velocity += Vector3.up * Player.jumpforce;
            jumpCount++;
            Debug.Log(jumpCount);
            Debug.Log("ExitFallState " + Player.gravity);
            stateMachine.TransitionTo<PlayerFlyState>();
            
        }
        Player.gravity = fallGravity;
        Vector3 gravityMovement = Vector3.down * Player.gravity * Time.deltaTime;
        Player.velocity += gravityMovement;
        
    }
}
