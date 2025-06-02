using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class ActionManager : MonoBehaviour
{
    CharachterManager charachterManager;
    [SerializeField] private AnimatorOverrideController overrideController;

    private AnimatorOverrideController runtimeOverride;
    private List<ActionsInput> actionInputs = new();
    private Animator animator;
    private ComboMove lastPerformedMove;
    private Coroutine scheduleCoroutine;
    private void Awake()
        {
            charachterManager = GetComponent<CharachterManager>();
            animator = GetComponent<Animator>();
        }

    private void Start()
        {
            if (InputCache.instance != null)
            InputCache.instance.OnCacheReset += OnCacheButtonReset;
            //charachterManager.itemSwapped += SwapTrigger;
            runtimeOverride = new AnimatorOverrideController(overrideController);
            animator.runtimeAnimatorController = runtimeOverride;
        }

    private void OnDisable()
        {
            if (InputCache.instance != null)
            InputCache.instance.OnCacheReset -= OnCacheButtonReset;
            //charachterManager.itemSwapped += SwapTrigger;
        }

    //private void SwapTrigger() => animator.SetTrigger("onSwapItem");
        
    private bool ActionPerform()
        {
            if (charachterManager.interactionController.item.comboMoves == null) return false;

            foreach (ComboMove move in charachterManager.interactionController.item.comboMoves)
                {
                    if (actionInputs.Count != 0 && move.moveType == MoveType.Tap 
                        && lastPerformedMove == move.requiredMove
                        && actionInputs.SequenceEqual(move.requieredInputActions))
                        {
                            charachterManager.interactionController.SetAction(move);
                            var runtimeOverride = new AnimatorOverrideController(overrideController);
                            AnimationClip lastClip = lastPerformedMove != null
                                ? lastPerformedMove.animationClip : runtimeOverride["DefaultAction"];

                            runtimeOverride[lastClip] = move.animationClip;
                            animator.runtimeAnimatorController = runtimeOverride;
                            scheduleCoroutine = StartCoroutine(ScheduleAnimation(move));

                            return true;
                        }
                    else if (move.moveType == MoveType.Hold && lastPerformedMove == move.requiredMove 
                        && IsHeldCheck(move))
                        {
                            charachterManager.interactionController.SetAction(move);
                            var runtimeOverride = new AnimatorOverrideController(overrideController);
                            AnimationClip lastClip = lastPerformedMove != null
                                ? lastPerformedMove.animationClip : runtimeOverride["DefaultAction"];

                            runtimeOverride[lastClip] = move.animationClip;
                            animator.runtimeAnimatorController = runtimeOverride;
                            //if (scheduleCoroutine != null) StopCoroutine(scheduleCoroutine);
                            scheduleCoroutine = StartCoroutine(HoldLoopAnimation(move));
                            return true;
                        }
                }
            return false;
        }

    public void AnimationEventTrigger() => charachterManager.interactionController.ExecuteAction();
    private IEnumerator ScheduleAnimation(ComboMove move)
        {
            actionInputs.Clear();
            lastPerformedMove = move;
            animator.SetTrigger("toAction");
            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
            
            ActionPerform();
            if(ActionPerform() == false)
                yield return new WaitForSeconds(move.resetComboTimer);
            if(ActionPerform() == false) lastPerformedMove = null;
        }

    private bool IsHeldCheck(ComboMove move)
        {
            return move.requieredInputActions.All(input =>
                InputManager.instance.IsButtonHeld(input));
        }

    private IEnumerator HoldLoopAnimation(ComboMove move)
        {
            actionInputs.Clear();
            lastPerformedMove = move;
            charachterManager.interactionController.SetAction(move);

            //animator.SetBool("toActionLoop",true);
            while (IsHeldCheck(move))
                {
                    yield return new WaitUntil(() 
                        => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
                }
            //animator.SetBool("toActionLoop",false);

            ActionPerform();
            if(ActionPerform() == false)
                yield return new WaitForSeconds(move.resetComboTimer);
            if(ActionPerform() == false) lastPerformedMove = null;
        }


    //we will need some button hold actions with looped clips here (like for machine guns or other looped combo move)

    private void OnCacheButtonReset(List<ActionsInput> cachedInputs)
        {
            actionInputs = cachedInputs;    actionInputs.Sort();
            if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.1f) ActionPerform();
        }
}

//Few fixes needed but generaly we`re balling
//Need to add logic for interruption

/* animator.GetCurrentAnimatorStateInfo(layerIndex);
For example:

0 is typically for the Base Layer (full-body animations).

1 would be for Layer 1 (e.g., hands or another body part).

2 for Layer 2, etc. */

//to add: I need 3 or more different attack states 
//to set reusable animations for different charachter types
//also action manager will probably need some recognition, onEnable or smth we send data 
//to reference holder to say "this is current player`s action manager". 
//To reuse it for enemies i think we simply set them up manually since those don`t use more than one anim set and item