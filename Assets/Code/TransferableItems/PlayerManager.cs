using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header ("External components")]
    InputManager inputManager;
    Rigidbody pRigidbody;
    Transform cameraObject;
    

    [Header ("Internal values")]
    private Vector3 moveDirection;
    private int jumpCount;


    [Header ("Movement modifiers values")]
    public float moveSpeed;
    public float rotationSpeed;
    public float jumpForce;
    public int jumpAmount = 1;
    public float fallMultiplier = 2.5f;
    public float jumpResistMultiplier = 2f;
    public float gRayDistance = 0.5f;


    [Header ("Layer modifiers")]
    public LayerMask groundMask;
    //public LayerMask aimMask;


    [Header ("Behaviour booleans")]
    public bool isFalling;

    private void Awake()
        {
            inputManager = GetComponent<InputManager>();
            pRigidbody = GetComponent<Rigidbody>();
            cameraObject = Camera.main.transform;
        }

    private void Update()
        {
            //PlayerAim();
            //PlayerInteraction();
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

            pRigidbody.velocity = new Vector3(moveDirection.x * moveSpeed, pRigidbody.velocity.y, moveDirection.z * moveSpeed);

          /*  if (moveDirection.x != 0f && !isFalling || moveDirection.z != 0f && !isFalling)
                {
                    isWalking = true;
                }
            else
                {
                    isWalking = false;
                } */
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
            
            if (Physics.Raycast(gRayOrigin, -Vector3.up, out hit, gRayDistance, groundMask))
                {
                    isFalling = false;
                    jumpCount = jumpAmount;
                }
            else
                {
                    isFalling = true;
                }
        }

    private void GravityUpdate()
        {
            if (pRigidbody.velocity.y < 0) 
                {
                    pRigidbody.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier) * Time.deltaTime;  //falling
                }
            else if (pRigidbody.velocity.y > 0 && !inputManager.isJump) 
                {
                    pRigidbody.velocity += Vector3.up * Physics.gravity.y * (jumpResistMultiplier) * Time.deltaTime;  //low jump resistance force
                }
        }

    private void PlayerJump()
        {
            if (inputManager.isJump)
                {
                    if (jumpCount > 0)
                        {
                            pRigidbody.velocity = Vector3.up * jumpForce;
                            jumpCount -= 1;
                            //isJumping = true;
                        }
                }
        }
}
