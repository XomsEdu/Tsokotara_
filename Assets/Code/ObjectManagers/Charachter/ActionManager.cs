using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ActionManager : MonoBehaviour
{
    CharacterManager charachterManager;
    [SerializeField] private AnimatorOverrideController overrideController;

    private AnimatorOverrideController runtimeOverride;
    private List<ActionsInput> actionInputs = new();
    private Animator animator;
    private ComboMove lastPerformedMove;
    private Coroutine scheduleCoroutine;
    private bool canChainNextMove = true;
    private Coroutine waitCheck;

    private void Awake()
        {
            charachterManager = GetComponent<CharacterManager>();
            animator = GetComponentInChildren<Animator>();
        }

    private void OnEnable() => StartCoroutine(Initialize());
    private IEnumerator Initialize()
        {
            runtimeOverride = new AnimatorOverrideController(overrideController);
            animator.runtimeAnimatorController = runtimeOverride;
                    
            yield return new WaitUntil(() => InputCache.instance != null);
            InputCache.instance.OnCacheReset += OnCacheButtonReset;
        }

    private void OnDisable()
        {
            if (InputCache.instance != null)
                InputCache.instance.OnCacheReset -= OnCacheButtonReset;
        }

    private void OnCacheButtonReset(List<ActionsInput> cachedInputs)
        {
            actionInputs = new List<ActionsInput>(cachedInputs);
            actionInputs.Sort();
            if (waitCheck != null) StopCoroutine(waitCheck);
            waitCheck = StartCoroutine(WaitForPerform());
        }

    private IEnumerator WaitForPerform()
        {
            yield return new WaitUntil(() => canChainNextMove);
            bool performed = ActionPerform();

            
            if (lastPerformedMove != null)
                yield return new WaitForSeconds(lastPerformedMove.resetComboTimer);
            if (!performed) lastPerformedMove = null;
        }

    private bool ActionPerform()
        {
            if (charachterManager.interactionController.item.comboMoves == null)
                {
                    Debug.Log("Item moves lost");
                    return false;
                }

            foreach (ComboMove move in charachterManager.interactionController.item.comboMoves)
                {
                    if (actionInputs.Count != 0 && move.moveType == MoveType.Tap
                        && lastPerformedMove == move.requiredMove
                        && actionInputs.SequenceEqual(move.requieredInputActions))
                            {
                                charachterManager.interactionController.SetAction(move);

                                runtimeOverride["DefaultAction"] = move.animationClip;
                                animator.runtimeAnimatorController = runtimeOverride;

                                scheduleCoroutine = StartCoroutine(ScheduleAnimation(move));
                                return true;
                            }
                    else if (move.moveType == MoveType.Hold && lastPerformedMove == move.requiredMove
                        && IsHeldCheck(move))
                            {
                                charachterManager.interactionController.SetAction(move);

                                runtimeOverride["DefaultAction"] = move.animationClip;
                                animator.runtimeAnimatorController = runtimeOverride;

                                scheduleCoroutine = StartCoroutine(HoldLoopAnimation(move));
                                return true;
                            }
                }

            return false;
        }

    private IEnumerator ScheduleAnimation(ComboMove move)
        {
            //Debug.Log($"[Combo] Performing {move.name}, requires: {(move.requiredMove ? move.requiredMove.name : "none")}, got: {(lastPerformedMove ? lastPerformedMove.name : "none")}");

            canChainNextMove = false;
            actionInputs.Clear();
            lastPerformedMove = move;

            animator.SetBool("toAction", true);

            yield return new WaitUntil(() =>
                {
                    var state = animator.GetCurrentAnimatorStateInfo(0);
                    return state.IsName("Action") && state.normalizedTime >= 1f;
                });

            animator.SetBool("toAction", false);
            canChainNextMove = true;
        }

    private IEnumerator HoldLoopAnimation(ComboMove move)
        {
            canChainNextMove = false;
            actionInputs.Clear();
            lastPerformedMove = move;

            while (IsHeldCheck(move))
                {
                    yield return new WaitUntil(() =>
                        animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
                }

            canChainNextMove = true;
        }

    private bool IsHeldCheck(ComboMove move)
        {
            return move.requieredInputActions.All(input =>
                InputManager.instance.IsButtonHeld(input));
        }

    public void AnimationEventTrigger() => charachterManager.interactionController.ExecuteAction();
}
