using System;
using System.Collections.Generic;
using UnityEngine;

public class InputCache : MonoBehaviour
{
    public static InputCache instance {get; private set;}
    InputManager inputManager;

    [SerializeField] private float chacheResetTimer = 0.1f;
    public event Action<List<ActionsInput>> OnCacheReset;

    private List<ActionsInput> inputActions = new();
    private float timeStart;

    private void Awake() => instance = this;

    private void Start() => inputManager = InputManager.instance;

    private void FixedUpdate()
        {
            if (Time.time >= (timeStart + chacheResetTimer) && inputActions.Count > 0)
                {
                    OnCacheReset?.Invoke(new List<ActionsInput>(inputActions));
                    inputActions.Clear();
                }

            CacheButton(ActionsInput.Jump, inputManager.isJumpPressed);
            CacheButton(ActionsInput.MainAction, inputManager.isMainActionPressed);
            CacheButton(ActionsInput.HeavyAction, inputManager.isHeavyActionPressed);
            CacheButton(ActionsInput.CancelAction, inputManager.isCancelActionPressed);
        }

    private void CacheButton(ActionsInput action, bool condition)
        {
            if (condition)
                {
                    inputActions.Add(action);
                    timeStart = Time.time;  if (inputActions.Count > 3) inputActions.RemoveAt(0);
                }
        }
}