using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class MenuInputListener : MonoBehaviour
{
    public GameObject menuUI, inventoryUI;
    public GameObject cameraRuler;
    public GameObject player;
    public Transform checkpointTransform;

    private InputActions inputActions;
    private InputManager inputManager;
    private InputCache inputCache;

    private bool playerWasDead = false;

    void Start()
        {
            if (player != null && !player.activeSelf) OpenMenuOnDeath();
        }

    void OnEnable()
        {
            inputManager = GetComponent<InputManager>();
            inputCache = GetComponent<InputCache>();

            if (inputActions == null)
                {
                    inputActions = new InputActions();
                    inputActions.Interaction.OpenMenu.performed += OnOpenMenu;
                    inputActions.Interaction.Inventory.performed += OnOpenInventory;
                }

            inputActions.Interaction.Enable();
        }

    void OnDisable()
        {
            inputActions.Interaction.Disable();
        }

    void Update()
        {
            if (player != null && !player.activeSelf && !menuUI.activeSelf) OpenMenuOnDeath();
        }

    private void OpenMenuOnDeath()
        {
            playerWasDead = true;
            menuUI.SetActive(true);
            ToggleGameplayInput(isGameplay: false);
        }

    public void MenuButton() => StartCoroutine(DisableMenuNextFrame());

    private IEnumerator DisableMenuNextFrame()
        {
            yield return null;

            menuUI.SetActive(false);
            ToggleGameplayInput(isGameplay: true);

            if (playerWasDead)
                {
                    RevivePlayer();
                    playerWasDead = false;
                }
        }

    private void RevivePlayer()
        {
            if (player != null)
                {
                    if (checkpointTransform != null)
                        player.transform.position = checkpointTransform.position;

                    player.SetActive(true);
                }
        }

    private void OnOpenMenu(InputAction.CallbackContext context)
        {
            bool isActive = !menuUI.activeSelf;
            menuUI.SetActive(isActive);

            ToggleGameplayInput(isGameplay: !isActive);
        }

    private void OnOpenInventory(InputAction.CallbackContext context)
        {
            bool isActive = !inventoryUI.activeSelf;
            inventoryUI.SetActive(isActive);

            ToggleGameplayInput(isGameplay: !isActive);
        }

    private void ToggleGameplayInput(bool isGameplay)
        {
            inputManager.movementInput = Vector2.zero;
            if (inputManager != null)
                inputManager.enabled = isGameplay;

            if (cameraRuler != null)
                cameraRuler.SetActive(isGameplay);

            Cursor.visible = !isGameplay;
            Cursor.lockState = isGameplay ? CursorLockMode.Locked : CursorLockMode.None;
        }

    public void ExitGame()
        {
            #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
            #else
                    Application.Quit();
            #endif
        }
}
