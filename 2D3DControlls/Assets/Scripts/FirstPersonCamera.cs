using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{


    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float cameraSkinwidth;
    [SerializeField] private LayerMask collisionMask;
    public GameObject camera;
    public Transform player;
    public bool thirdPersonCamera;

    public float smoothSpeed = 0.125f;
    private float x;
    private float y;

    public float cameraDistance;

    SphereCollider collider; 


    public Vector3 cameraOffset;
    Vector3 firstPersonCameraOffset; 
   
    // Start is called before the first frame update
    void Start()
    {
        
        collider = GetComponent<SphereCollider>();
        firstPersonCameraOffset = transform.localPosition;
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    private void LateUpdate()
    {
        //input från musen
        y += Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        x -= Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

        transform.rotation = Quaternion.Euler(x, y, 0);

        if (thirdPersonCamera == true)
        {

            Vector3 offset = camera.transform.rotation * cameraOffset;
            Vector3 desiredPosition = player.transform.position + offset;
            Vector3 direction = desiredPosition - player.transform.position;
            RaycastHit hit = RaycastCheck(direction);
            cameraDistance = Mathf.Lerp(direction.magnitude, cameraDistance, 1- Time.deltaTime);
            if (hit.collider)
            {
                cameraDistance = hit.distance; 
            }
            offset = offset.normalized * cameraDistance;

            desiredPosition = player.transform.position + offset;
            transform.position = desiredPosition;
        } else
        {
            transform.position = player.transform.position + firstPersonCameraOffset; 
        }
        
    }
    
    RaycastHit RaycastCheck(Vector3 direction)
    {

        Physics.SphereCast(player.transform.position,
                           collider.radius,
                           direction,
                           out RaycastHit hit,
                           direction.magnitude,
                           collisionMask);
        return hit;
    }
    Vector3 NormalForceFunction(Vector3 cameraVelocity, Vector3 normal)
    {

        float projection = Vector3.Dot(cameraVelocity, normal);
        if (projection > 0)
        {
            projection = 0f;
        }
        return -(projection * normal);
    }


}
