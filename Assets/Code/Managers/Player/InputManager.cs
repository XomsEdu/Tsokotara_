using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    InputActions inputActions;
    public Vector2 movementInput;
    public bool isJump;

     private void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new InputActions();

            inputActions.Movement.WASD.performed += i => movementInput = i.ReadValue<Vector2>(); //WASD

            inputActions.Movement.Jump.performed += i => isJump = true; //Jump
            inputActions.Movement.Jump.canceled += i => isJump = false;
        }

        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
}
