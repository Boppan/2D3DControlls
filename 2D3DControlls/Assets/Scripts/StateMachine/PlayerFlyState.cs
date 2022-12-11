using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerState/FlyState")]
public class PlayerFlyState : PlayerBaseState
{
    

    public override void Enter()
    {
        Player.gravity = 25;
        Debug.Log("Enter Fly State");
    }

    public override void Run()
    {
        Player.jumpforce = Player.jumpforce;
        
        if (Player.isJumpPressed && jumpCount < airJumps)
        {
            Player.velocity += Vector3.up * Player.jumpforce;
            jumpCount++;
            Debug.Log(jumpCount);
        }
        if (Player.GroundCheck())
        {
            stateMachine.TransitionTo<PlayerGroundState>();
        }
        if(Player.velocity.y < 0)
        {
            stateMachine.TransitionTo<FallingState>();
        }

        Vector3 gravityMovement = Vector3.down * Player.gravity * Time.deltaTime;
        Player.velocity += gravityMovement;
        
    }
}
