using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ActionManager : MonoBehaviour
{
    private bool actionInProgress;
    public List<ComboMove> comboMoves;  private ComboMove lastPerformedMove;
    private List<ActionsInput> actionInputs = new List<ActionsInput>();
    
    Animator animator;
    private void Awake() => animator = GetComponent<Animator>();

    private void Start()
        {   if (InputCache.instance != null)    InputCache.instance.OnCacheReset += OnCacheButtonReset; }

        private void OnDisable()
        {   if (InputCache.instance != null)    InputCache.instance.OnCacheReset -= OnCacheButtonReset; }


    private void ActionPerform()
        {   
            if (comboMoves == null || actionInputs.Count <= 0) return;

            foreach (ComboMove move in comboMoves)
                {
                    if(lastPerformedMove == move.requieredMove
                    &&  actionInputs.SequenceEqual(move.requieredInputActions))
                        {  scheduleCoroutine =  StartCoroutine(ScheduleAnimation(move));    break;  }
                }

            actionInputs.Clear();
        }

    private IEnumerator ActionInteruption(int duration)
        {
            if (scheduleCoroutine != null) StopCoroutine(scheduleCoroutine);
            actionInProgress = true;
            actionInputs.Clear();
            yield return new WaitForSeconds(duration);
            actionInProgress = false;
            yield return new WaitForSeconds(lastPerformedMove.comboResetTime);
            if(actionInputs.Count == 0) lastPerformedMove = null;
        }
    //to be used in from animation clip (for moves like dodge or block that cancels animation)

    private Coroutine scheduleCoroutine;
        
    private IEnumerator ScheduleAnimation(ComboMove move)
        {
            Debug.Log("MovePerformed");
            lastPerformedMove = move;
            actionInProgress = true;
            //float duration = move.animationClip.length;
            yield return new WaitForSeconds(2);
            actionInProgress = false;
            ActionPerform();
            yield return new WaitForSeconds(move.comboResetTime);
            if(actionInputs.Count == 0) lastPerformedMove = null;
        }

    private void OnCacheButtonReset(List<ActionsInput> cachedInputs)
        {
            actionInputs = cachedInputs;    actionInputs.Sort();
            if(!actionInProgress) ActionPerform();
        }
}

//Few fixes needed but generaly we`re balling
//Need to add logic for interruption