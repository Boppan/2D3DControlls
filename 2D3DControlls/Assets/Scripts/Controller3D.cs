using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Controller3D : MonoBehaviour
{
    [SerializeField] private float acceleration;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float deceleration;
    [SerializeField] private float skinWidth;
    [SerializeField] private LayerMask collisionMask;
    //[SerializeField] private float maxSlopeAngle;
    
    public float gravity;
    public float jumpforce;
    public bool isJumpPressed = false;

    private float groundCheckDistance = 0.1f;
    private float airResistance = 0.1f;
    private bool grounded;
    CapsuleCollider collider;
    public GameObject camera;
    public GameObject player; 

    Vector3 velocity;
    Vector3 groundNormalForce;



    // Start is called before the first frame update
    private void Awake()
    {

        collider = GetComponent<CapsuleCollider>();
        
    }
    void Start()
    {

    }
    Vector3 debug;

    // Update is called once per frame
    void Update()
    {

        Vector3 origin = transform.position + collider.center + Vector3.down * (collider.height / 2 - collider.radius);

        grounded = Physics.SphereCast(origin,
                                      collider.radius,
                                      Vector2.down,
                                      out RaycastHit hit,
                                      groundCheckDistance + skinWidth,
                                      collisionMask);

        groundNormalForce = hit.normal;

        Vector3 gravityMovement = Vector3.down * gravity * Time.deltaTime;

        GetInput();
        

        velocity += gravityMovement;
        velocity *= Mathf.Pow(airResistance, Time.deltaTime);


        UpdateVelocity();

        transform.position += (Vector3)velocity * Time.deltaTime;

    }

    private void UpdateVelocity()
    {

        int calc = 0;
        //Kolla efter en hit
        RaycastHit hit;


        //ITTERATIV
        do
        {
            if (velocity.magnitude < float.Epsilon)
            {
                velocity = Vector3.zero;
                return;
            }
            Vector3 point1 = transform.position + collider.center + Vector3.up * (collider.height / 2 - collider.radius);
            Vector3 point2 = transform.position + collider.center + Vector3.down * (collider.height / 2 - collider.radius);
            hit = RaycastCheck(point1, point2);


            if (hit.collider)
            {

                //2:a exemplet
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
            collider.radius,
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

            input = camera.transform.rotation * input;
            input = Vector3.ProjectOnPlane(input, groundNormalForce).normalized;



        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            velocity += Vector3.up * jumpforce;
            isJumpPressed = true;
            Debug.Log(isJumpPressed);
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
        if(input.magnitude > 1)
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
        float staticFrictionCoefficient = 0.3f;
        float kineticFrictionCoefficient = 0.15f;
        if (velocity.magnitude < normalForce.magnitude * staticFrictionCoefficient)
        {
            velocity = Vector3.zero;
        }
        else
        {
            velocity -= velocity.normalized * normalForce.magnitude * kineticFrictionCoefficient;
        }


    }
    private void OnDrawGizmos()
    {
      /*  Gizmos.color = Color.black; 
        Gizmos.DrawWireSphere(debug, 1); */
    }
}