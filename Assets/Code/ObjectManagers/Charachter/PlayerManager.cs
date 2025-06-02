using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    Rigidbody pRigidbody;
    StatsManager statsManager;
    Transform cameraObject;

    [Header ("Internal values")]
    private Vector3 moveDirection;
    private int jumpCount;
    private float coyoteTimeCount;


    [Header ("Movement modifiers values")]
    public float rotationSpeed;
    public float fallMultiplier = 2.5f;
    public float jumpResistMultiplier = 2f;
    public float gravityScaler;
    public float gRayDistance = 0.5f; //avatarInfo
    public float coyoteTime = 0.1f;


    [Header ("Layer modifiers")]
    public LayerMask groundMask;
    //public LayerMask aimMask;


    [Header ("Behaviour booleans")]
    public bool isGrounded;

    public CharachterManager avatarInfo;

    private void Awake()
        {
            pRigidbody = GetComponent<Rigidbody>();
            statsManager = GetComponent<StatsManager>();
            
        }

    private void Start()
        {
            cameraObject = Camera.main.transform;
            inputManager = InputManager.instance;
        }

    private void FixedUpdate()
        {
            PlayerMovement();
            PlayerRotation();
            GroundCheck();
            GravityUpdate();
            PlayerJump();
        }

    private void PlayerMovement()
        {
            moveDirection = cameraObject.forward * inputManager.movementInput.y;
            moveDirection = moveDirection + cameraObject.right * inputManager.movementInput.x;
            moveDirection.Normalize();
            moveDirection.y = 0;

            pRigidbody.velocity = new Vector3(moveDirection.x * statsManager.moveSpeed, 
                pRigidbody.velocity.y, moveDirection.z * statsManager.moveSpeed);
        }

    private void PlayerRotation()
        {
            Vector3 targetDirection = Vector3.zero;

            targetDirection = cameraObject.forward * inputManager.movementInput.y;
            targetDirection = targetDirection + cameraObject.right * inputManager.movementInput.x;
            targetDirection.Normalize();
            targetDirection.y = 0;

            if (targetDirection == Vector3.zero)
                targetDirection = transform.forward;

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

           pRigidbody.MoveRotation(playerRotation);
        }

    
    private void GroundCheck()
        {
            Vector3 gRayOrigin = transform.position;
            
            RaycastHit hit;

            isGrounded = Physics.Raycast(gRayOrigin, -Vector3.up, out hit, gRayDistance, groundMask);
            
            if (isGrounded)
                {
                    jumpCount = statsManager.jumpAmount;
                    coyoteTimeCount = coyoteTime;
                }
            else coyoteTimeCount -= Time.deltaTime;
        }


    private void GravityUpdate()
        {
            pRigidbody.velocity += Vector3.up * Physics.gravity.y * (gravityScaler) * Time.deltaTime;

            if (pRigidbody.velocity.y < 0) 
                pRigidbody.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier) * Time.deltaTime;  //falling
            
            else if (pRigidbody.velocity.y > 0 && !inputManager.isJumpHeld)
                pRigidbody.velocity += Vector3.up * Physics.gravity.y * (jumpResistMultiplier) * Time.deltaTime;  //low jump resistance force
        }


    private void PlayerJump()
        {
            if (coyoteTimeCount > 0f && inputManager.isJumpHeld)
                pRigidbody.velocity = new Vector3(pRigidbody.velocity.x, statsManager.jumpForce, pRigidbody.velocity.z);

            if (coyoteTimeCount <= 0f && jumpCount > 0 && inputManager.isJumpPressed)
                {
                    jumpCount -= 1;
                    pRigidbody.velocity = new Vector3(pRigidbody.velocity.x, statsManager.jumpForce, pRigidbody.velocity.z);
                }
        }
}