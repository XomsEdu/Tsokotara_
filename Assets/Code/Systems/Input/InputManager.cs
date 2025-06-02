using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance {get; private set;} private void Awake() => instance = this;

    InputActions inputActions;

    public Vector2 movementInput;
    public bool isJumpHeld;     public bool wasJumpHeld;    public bool isJumpPressed;
    public bool isInventoryHeld;
    public bool isMainActionHeld; public bool wasMainActionHeld; public bool isMainActionPressed;
    public bool isHeavyActionHeld; public bool wasHeavyActionHeld; public bool isHeavyActionPressed;
    public bool isCancelActionHeld; public bool wasCancelActionHeld; public bool isCancelActionPressed;
    public bool isShootingActionHeld;

    private void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new InputActions();

                inputActions.Movement.WASD.performed += i => movementInput = i.ReadValue<Vector2>(); //WASD

                inputActions.Movement.Jump.performed += i => isJumpHeld = true; //Jump
                inputActions.Movement.Jump.canceled += i => isJumpHeld = false;

                inputActions.Interaction.Inventory.performed += i => isInventoryHeld = true; //Inventory
                inputActions.Interaction.Inventory.canceled += i => isInventoryHeld = false;

                inputActions.Interaction.MainAction.performed += i => isMainActionHeld = true; //Main action
                inputActions.Interaction.MainAction.canceled += i => isMainActionHeld = false;

                inputActions.Interaction.HeavyAction.performed += i => isHeavyActionHeld = true; //Heavy action
                inputActions.Interaction.HeavyAction.canceled += i => isHeavyActionHeld = false;

                inputActions.Interaction.CancelAction.performed += i => isCancelActionHeld = true; //Cancel action
                inputActions.Interaction.CancelAction.canceled += i => isCancelActionHeld = false;
            }

            inputActions.Enable();
        }

    private void OnDisable() => inputActions.Disable();

    private void FixedUpdate()
        {
            isJumpPressed = isJumpHeld && !wasJumpHeld; 
            wasJumpHeld = isJumpHeld;

            isMainActionPressed = isMainActionHeld && !wasMainActionHeld;
            wasMainActionHeld = isMainActionHeld;

            isHeavyActionPressed = isHeavyActionHeld && !wasHeavyActionHeld;
            wasHeavyActionHeld = isHeavyActionHeld;

            isCancelActionPressed = isCancelActionHeld && !wasCancelActionHeld;
            wasCancelActionHeld = isCancelActionHeld;
        }


    public bool IsButtonHeld(ActionsInput input)
        {
            return input switch
                {
                    ActionsInput.JumpHeld => wasJumpHeld,
                    ActionsInput.MainActionHeld => wasMainActionHeld,
                    ActionsInput.HeavyActionHeld => wasHeavyActionHeld,
                    ActionsInput.CancelActionHeld => wasCancelActionHeld,
                    _ => false
                };
        }
}

public enum ActionsInput
{
    Jump, JumpHeld,
    MainAction, MainActionHeld,
    HeavyAction, HeavyActionHeld,
    CancelAction, CancelActionHeld,
    ShootAction, ShootActionHeld
}