using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float forwardForce = 500f;
    public float sidewaysAcceleration = 50f;  // How fast to speed up left/right
    public float maxSidewaysSpeed = 10f;      // Max left/right velocity
    public float sidewaysFriction = 0.15f;    // Friction coefficient (0-1, higher = more friction)
    public float jumpHeight = 2f;
    public LayerMask groundLayer;

    float currentSidewaysVelocity = 0f;
    bool moveRight = false;
    bool moveLeft = false;

    bool jump = false;
    bool isGrounded = false;
    bool hasAirJump = true;
    bool hasJumped = false;
    public float jumpCooldown = 0.1f; // seconds
    float lastJumpTime = -Mathf.Infinity;


    void Start()
    {
        rb.constraints = RigidbodyConstraints.FreezePositionZ;
    }



    // Changing Physics 
    void Update()
    {
        if (Input.GetKey("d"))
        {
            moveRight = true;
        }

        if (Input.GetKey("a"))
            moveLeft = true;
        
        // Use GetKeyDown so a single press registers once; enforce cooldown
        if (Input.GetKeyDown("space") && Time.time >= lastJumpTime + jumpCooldown)
        {
            jump = true;
            lastJumpTime = Time.time;
        }

    }


    void FixedUpdate()
    {
        // Check if grounded using raycasts
        CheckGrounded();

        // Handle sideways movement with acceleration and friction
        if (moveRight)
        {
            // Accelerate right, clamped to max speed
            currentSidewaysVelocity = Mathf.Min(currentSidewaysVelocity + sidewaysAcceleration * Time.deltaTime, maxSidewaysSpeed);
            moveRight = false;
        }
        else if (moveLeft)
        {
            // Accelerate left, clamped to max speed
            currentSidewaysVelocity = Mathf.Max(currentSidewaysVelocity - sidewaysAcceleration * Time.deltaTime, -maxSidewaysSpeed);
            moveLeft = false;
        }
        else
        {
            // Apply friction when no input
            currentSidewaysVelocity *= (1f - sidewaysFriction);
        }
        
        // Apply the sideways velocity
        rb.linearVelocity = new Vector3(currentSidewaysVelocity, rb.linearVelocity.y, rb.linearVelocity.z); 
    
        if (jump)
        {
            if (isGrounded && !hasJumped)
            {
                // First jump: set exact initial vertical velocity for target jumpHeight
                float jumpVelocity = Mathf.Sqrt(2f * Mathf.Abs(Physics.gravity.y) * jumpHeight);
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpVelocity, rb.linearVelocity.z);
                // Freeze rotation and Z position during jump
                rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
                hasJumped = true;
                hasAirJump = true;
            }
            else if (!isGrounded && hasAirJump && hasJumped)
            {
                // Mid-air jump
                float initialVelocity = Mathf.Sqrt(2f * Mathf.Abs(Physics.gravity.y) * jumpHeight);
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, initialVelocity, rb.linearVelocity.z);
                hasAirJump = false;
            }
            
            jump = false;
        }
    }

    void CheckGrounded()
    {
        // Raycast down to check if player is on ground
        float rayDistance = 0.51f;
        
        isGrounded = Physics.Raycast(transform.position, UnityEngine.Vector3.down, rayDistance, groundLayer);

        // If grounded, reset jump counters and unfreeze rotation
        if (isGrounded)
        {
            hasJumped = false;
            hasAirJump = true;
            // Unfreeze rotation when back on ground
            rb.constraints = RigidbodyConstraints.FreezePositionZ;
        }
    }
}
