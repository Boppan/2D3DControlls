using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float acceleration;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float deceleration;
    [SerializeField] private float skinWidth;
    [SerializeField] private LayerMask collisionMask;
    BoxCollider2D collider;
    public float gravity;
    public float jumpforce; 
    private float groundCheckDistance = 0.1f;
    private bool grounded;
    private float airResistance = 0.1f;
    Vector2 velocity; 

    // Start is called before the first frame update
    private void Awake()
    {
        
        collider = GetComponent<BoxCollider2D>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        grounded = Physics2D.BoxCast(transform.position, 
                                     collider.size, 
                                     0.0f, Vector2.down, 
                                     groundCheckDistance + skinWidth, 
                                     collisionMask);

        Vector2 gravityMovement = Vector2.down * gravity * Time.deltaTime;

        GetInput();
       
        
        velocity += gravityMovement;
        velocity *= Mathf.Pow(airResistance, Time.deltaTime);

        UpdateVelocity();

        transform.position += (Vector3)velocity * Time.deltaTime;

    }

    //Gjort om från bool till Vector 2


    private void UpdateVelocity()
    {
        int calc = 0;
        //Kolla efter en hit
        RaycastHit2D hit; 
        
        //ITTERATIV
        do
        {
            if (velocity.magnitude < float.Epsilon)
            {
                velocity = Vector2.zero;
                return;
            }
            hit =
                Physics2D.BoxCast
                (transform.position,
                collider.size,
                0.0f,
                velocity.normalized,
                Mathf.Infinity,
                collisionMask);
            if (hit)
            {

                //2:a exemplet
                float distanceToColliderNeg = skinWidth / Vector2.Dot(velocity.normalized, hit.normal);
                float allowedMovementDistance = hit.distance + distanceToColliderNeg;
                if (allowedMovementDistance > velocity.magnitude * Time.deltaTime)
                {
                    return;
                }
                if (allowedMovementDistance > 0.0f)
                {
                    transform.position += (Vector3)velocity.normalized * allowedMovementDistance;
                }

                Vector2 normalForce = NormalForceFunction(velocity, hit.normal);
                velocity += normalForce;
                Friction(normalForce);
                calc++;
                
            }
        }
        while (hit && calc < 10);
    }

    Vector2 NormalForceFunction(Vector2 velocity, Vector2 normal )
    {

        float projection = Vector2.Dot(velocity, normal);
        if(projection > 0)
        {
           projection = 0f;
        }
        return -(projection * normal);
    }

    private void GetInput()
    {
        Vector2 input = Vector2.right * Input.GetAxisRaw("Horizontal");
        velocity += input * acceleration * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            velocity += Vector2.up * jumpforce;
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
    private void Accelerate(Vector2 input)
    {
        velocity += input * acceleration * Time.deltaTime;
        velocity = Vector2.ClampMagnitude(velocity, maxSpeed);
    }

    private void Decelerate()
    {
        if(deceleration * Time.deltaTime > Mathf.Abs(velocity.x))
        {
            velocity.x = 0.0f;
        }
        else
        {
            Vector2 projection = new Vector2(velocity.x, 0.0f).normalized;
            velocity -= projection * deceleration * Time.deltaTime;
        }
        
    }
    private void Friction(Vector2 normalForce)
    {
        float staticFrictionCoefficient = 0.5f;
        float kineticFrictionCoefficient = 0.25f; 
        if (velocity.magnitude < normalForce.magnitude * staticFrictionCoefficient)
        {
            velocity = Vector2.zero;
        }
        else
        {
            velocity -= velocity.normalized * normalForce.magnitude * kineticFrictionCoefficient;
        }
    }
}
