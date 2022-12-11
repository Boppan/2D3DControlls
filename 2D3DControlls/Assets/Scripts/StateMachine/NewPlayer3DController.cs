using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayer3DController : MonoBehaviour
{
    [SerializeField] private float acceleration;
    public float maxSpeed;
    [SerializeField] private float deceleration;
    [SerializeField] private float skinWidth;
    [SerializeField] private LayerMask collisionMask;
    //[SerializeField] private float maxSlopeAngle;

    public float gravity;
    public float jumpforce;
    public float runMaxSpeed;
    public float staticFrictionCoefficient;
    public float kineticFrictionCoefficient;

    public bool isJumpPressed = false;
    public bool isRunning = false; 

    private float groundCheckDistance = 0.1f;
    private float airResistance = 0.1f;
    public bool isMoving;
    //private bool isGrounded;
    CapsuleCollider capsuleCollider;
    public GameObject cameraObject;
    public GameObject player;

    Vector3 groundNormalForce;
    public Vector3 velocity;

    private StateMachine stateMachine;
    public State[] states;

    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    void Start()
    {
        stateMachine = new StateMachine(this, states);
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();
        //Vector3 gravityMovement = Vector3.down * gravity * Time.deltaTime;
        GetInput();
        //velocity += gravityMovement;
        velocity *= Mathf.Pow(airResistance, Time.deltaTime);

        stateMachine.Run();
        UpdateVelocity();
        transform.position += (Vector3)velocity * Time.deltaTime;

    }

    private void UpdateVelocity()
    {
        int calc = 0;
        RaycastHit hit;

        do
        {
            if (velocity.magnitude < float.Epsilon)
            {
                velocity = Vector3.zero;
                return;
            }
            Vector3 point1 = transform.position + capsuleCollider.center + Vector3.up * (capsuleCollider.height / 2 - capsuleCollider.radius);
            Vector3 point2 = transform.position + capsuleCollider.center + Vector3.down * (capsuleCollider.height / 2 - capsuleCollider.radius);
            hit = RaycastCheck(point1, point2);


            if (hit.collider)
            {
                float distanceToColliderNeg = skinWidth / Vector3.Dot(velocity.normalized, hit.normal);
                float allowedMovementDistance = hit.distance + distanceToColliderNeg;
                if (allowedMovementDistance > velocity.magnitude * Time.deltaTime)
                {
                    return;
                }
                if (allowedMovementDistance > 0.0f)
                {
                    transform.position += (Vector3)velocity.normalized * allowedMovementDistance;
                }

                Vector3 normalForce = NormalForceFunction(velocity, hit.normal);
                velocity += normalForce;
                Friction(normalForce);
                calc++;


            }
        }
        while (hit.collider && calc < 10);
    }

    RaycastHit RaycastCheck(Vector3 point1, Vector3 point2)
    {
        Physics.CapsuleCast
            (point1, point2,
            capsuleCollider.radius,
            velocity.normalized,
            out var hit,
            Mathf.Infinity,
            collisionMask);
        return hit;
    }

    Vector3 NormalForceFunction(Vector3 velocity, Vector3 normal)
    {

        float projection = Vector3.Dot(velocity, normal);
        if (projection > 0)
        {
            projection = 0f;
        }
        return -(projection * normal);
    }

    private void GetInput()
    {
        Vector3 input = Vector3.right * Input.GetAxisRaw("Horizontal") + Vector3.forward * Input.GetAxisRaw("Vertical");

        //Kollar om mindre än maxlutning
        /* if (Mathf.Sin(capsuleCollider.radius) > maxSlopeAngle)

         {
             input = cameraObject.transform.rotation * input;
             input = Vector3.ProjectOnPlane(input, groundNormalForce).normalized;
         }*/

        input = cameraObject.transform.rotation * input;
        input = Vector3.ProjectOnPlane(input, groundNormalForce).normalized;

        if (Input.GetKeyDown(KeyCode.Space) && GroundCheck())
        {
            velocity += Vector3.up * jumpforce;
            isJumpPressed = true;
            //Debug.Log(isJumpPressed);
        } else 
        {
            isJumpPressed = false; 
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isRunning = true;
           
        } else
        {
            isRunning = false;
        }

        if (input.magnitude > float.Epsilon)
        {
            Accelerate(input);
        }
        else
        {
            Decelerate();
        }
    }


    private void Accelerate(Vector3 input)
    {
        if (input.magnitude > 1)
        {
            velocity += input.normalized * acceleration * Time.deltaTime;
        }
        velocity += input * acceleration * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
    }
    private void Decelerate()
    {
        if (deceleration * Time.deltaTime > Mathf.Abs(velocity.x))
        {
            velocity.x = 0.0f;
        }
        else
        {
            Vector3 projection = new Vector3(velocity.x, 0.0f).normalized;
            velocity -= projection * deceleration * Time.deltaTime;
        }
    }
    private void Friction(Vector3 normalForce)
    {

        if (velocity.magnitude < normalForce.magnitude * staticFrictionCoefficient)
        {
            velocity = Vector3.zero;
        }
        else
        {
            velocity -= velocity.normalized * normalForce.magnitude * kineticFrictionCoefficient;
        }

    }

    public bool GroundCheck()
    {
        Vector3 origin = transform.position + capsuleCollider.center + Vector3.down * (capsuleCollider.height / 2 - capsuleCollider.radius);
        
        bool isGrounded = Physics.SphereCast(origin,
                                             capsuleCollider.radius,
                                             Vector2.down,
                                             out RaycastHit hit,
                                             groundCheckDistance + skinWidth,
                                             collisionMask);
        groundNormalForce = hit.normal;

        if (isGrounded)
        {
            return true;
        }
        return false; 
    }



    private void OnDrawGizmos()
    {
        /*  Gizmos.color = Color.black; 
          Gizmos.DrawWireSphere(debug, 1); */
    }
}
