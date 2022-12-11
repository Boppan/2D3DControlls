using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerState/MoveState")]
public class PlayerMoveState : PlayerBaseState
{

  /*  public float MaxMoveSpeed = float.Epsilon;
    public float MinMoveSpeed = float.Epsilon;
    CapsuleCollider capsuleCollider;
    Vector3 velocity;

    public override void Enter()
    {
        capsuleCollider = Player.GetComponent<CapsuleCollider>();
        Debug.Log("Enter Move State");
    }
    public override void Run()
    {
        if (Player.velocity.magnitude < MinMoveSpeed)
        {
            stateMachine.TransitionTo<IdleState>();
        }

        if (Player.velocity.magnitude > MaxMoveSpeed)
        {
            stateMachine.TransitionTo<PlayerRunState>();
        }

        *//* if (!Physics.BoxCast(Vector3.down, 10f))
         {
             stateMachine.TransitionTo<FallingState>();
         }*//*

        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.TransitionTo<PlayerFlyState>();
        }
        Vector3 gravityMovement = Vector3.down * Player.gravity * Time.deltaTime;
        velocity += gravityMovement;
        
        Vector3 input = Vector3.right * Input.GetAxisRaw("Horizontal") + Vector3.forward * Input.GetAxisRaw("Vertical");
        if (input.magnitude > float.Epsilon)
        {
            Player.Accelerate(input);
        }
        else
        {
            Player.Decelerate();
        }
        

        Player.transform.position += (Vector3)velocity * Time.deltaTime;
        
        
        //Vector3 movement = (Input.GetAxis("Horizontal") * Vector3.right + Input.GetAxis("Vertical") * Vector3.forward).normalized * Speed * Time.deltaTime;
        //Player.transform.position += movement;
    }*/

}
