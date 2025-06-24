using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

//We need a class for "collective memory" so NPCs in one camp can get their destinations 
//and follow same enemies together

public class NavManager : MonoBehaviour
{
    [Header ("Serialized")]
    public List<NavAction> dailySchedule = new();
    public ComboMove attackMove;

    [Header ("NonSerialized")]
    [NonSerialized] public CharacterManager navAvatar;
    [NonSerialized] public Animator navAnimator;
    [NonSerialized] public NavMeshAgent agent;
    [NonSerialized] public NavAction lastNavAction; 
    public Vector3 currentTarget;
    private bool inCombat;
    private EyeSight eyeSight;
    private InteractionItem interactionController;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        navAvatar = GetComponent<CharacterManager>();
        eyeSight = navAvatar.head.GetComponent<EyeSight>();
        navAnimator = GetComponent<Animator>();
        interactionController = GetComponentInChildren<InteractionItem>();
    }

    void Start()
        {
            if (eyeSight != null) eyeSight.iSenseTarget += LookInDir;
            GameState.instance.tickDayTime += ScheduleAction;
            ScheduleAction(0);
        }

    void OnDisable()
    {
        GameState.instance.tickDayTime -= ScheduleAction;
        if (eyeSight != null) eyeSight.iSenseTarget -= LookInDir;
    }

    private bool AtPoint() => Vector3.Distance(currentTarget, this.transform.position) <= 1;
    public void WalkTo()
        {
            agent.isStopped = false;
            bool result = agent.SetDestination(currentTarget);
            navAnimator.SetBool("Walking", true);
        }

    private void LookInDir(Transform target) => StartCoroutine(TakeLook(target));
    
    private IEnumerator TakeLook(Transform target)
        {
            //Debug.Log("LookingForSomeone");

            Vector3 dir = (target.position - transform.position).normalized;
            if (Physics.Raycast(transform.position, dir, out RaycastHit hit, 100f, ~LayerMask.GetMask("Ignore Raycast")))
                {
                    if (hit.collider.CompareTag("Player"))
                        {
                            //Debug.Log("SeeSomeone");
                            StopAllCoroutines();
                            StartCoroutine(Attack(target));
                            yield break;
                        }
                }

            if (inCombat)
                {
                    currentTarget = target.position;
                    WalkTo();
                    inCombat = false;
                    yield return new WaitUntil(AtPoint);
                    StartCoroutine(TakeLook(target)); // proper recursive call
                }
            else
                {
                    ScheduleAction(GameState.instance.dayTime);
                }
        }


    private IEnumerator PerformRoutine(float time)
        {
            lastNavAction = GetCurrentAction(time);
            if(lastNavAction != null)
                {
                    lastNavAction.navManager = this;
                    lastNavAction.SelectTarget();
                }
            WalkTo();
            yield return new WaitUntil(() => agent.hasPath && !agent.pathPending);
            yield return new WaitUntil(() => AtPoint());
            navAnimator.SetBool("Walking", false);
            while(lastNavAction.targets.Count > 1)
                {
                    yield return new WaitForSeconds(5);
                    lastNavAction.SelectTarget();
                    WalkTo();
                    yield return new WaitUntil(() => AtPoint());
                    lastNavAction.PerformJob();
                }
            //if(lastNavAction.targets.Count == 1) lastNavAction.PerformJob();
        }

    private void ScheduleAction(float time) 
        {
            if(GetCurrentAction(time) == lastNavAction || inCombat) return;

            StopAllCoroutines();
            StartCoroutine(PerformRoutine(time));
        }

    public NavAction GetCurrentAction(float currentTime)
        {
            if (dailySchedule == null || dailySchedule.Count == 0) return null;

            var sortedSchedule = dailySchedule.OrderBy(entry => entry.time).ToList();

            for (int i = 0; i < sortedSchedule.Count; i++)
            {
                float start = sortedSchedule[i].time;
                float end = (i + 1 < sortedSchedule.Count) ? sortedSchedule[i + 1].time : 24f;

                if (currentTime >= start && currentTime < end)
                {
                    return sortedSchedule[i];
                }
            }
            return null;
        }


    public void AnimationEventTrigger() => interactionController.ExecuteAction();
    public IEnumerator Attack(Transform attackTarget)
        {
            if (attackTarget == null || !attackTarget.gameObject.activeInHierarchy) yield break;

            inCombat = true;
            lastNavAction = null;

            // Stop movement
            agent.isStopped = true;
            navAnimator.SetBool("Walking", false);

            // Optional: reposition if too far
            Vector3 targetOffset = attackTarget.position + Vector3.up * 2f;
            if (!AtPoint())
                {
                    if (NavMesh.SamplePosition(targetOffset, out NavMeshHit navHit, 2f, NavMesh.AllAreas))
                        currentTarget = navHit.position;
                    else
                        currentTarget = attackTarget.position;

                    WalkTo();
                    yield return new WaitUntil(AtPoint);
                    agent.isStopped = true;
                    navAnimator.SetBool("Walking", false);
                }

            // Rotate towards player
            Vector3 lookPos = attackTarget.position - transform.position;
            lookPos.y = 0;
            transform.rotation = Quaternion.LookRotation(lookPos);

            // Trigger attack
            interactionController.SetAction(attackMove);
            navAnimator.SetTrigger("toAttack");

            // Wait until animation plays and finishes
            yield return new WaitUntil(() =>
                navAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"));

            yield return new WaitUntil(() =>
                navAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

            // Attack done
            inCombat = false;

            // Resume scanning
            StartCoroutine(TakeLook(attackTarget));
        }



}

[System.Serializable]
public class NavAction
{
    public float time;
    public List<Transform> targets;

    private int targetIndex;
    [NonSerialized] public NavManager navManager;

    public void PerformJob() => navManager.navAnimator.SetTrigger("doJob");

    public void SelectTarget()
        {
            if (targets != null && targets.Count > 0)
                {
                    targetIndex = (targetIndex + 1) % targets.Count;

                    Vector3 samplePos = targets[targetIndex].position + Vector3.up * 2f;

                    if (NavMesh.SamplePosition(samplePos, out NavMeshHit navHit, 5f, NavMesh.AllAreas))
                        navManager.currentTarget = navHit.position;

                    else navManager.currentTarget = navManager.transform.position; // fallback
                }
            else
                {
                    //Debug.LogWarning("[NavAction] No valid target or targets list to select from.");
                    navManager.currentTarget = navManager.transform.position; // fallback
                }
        }
}