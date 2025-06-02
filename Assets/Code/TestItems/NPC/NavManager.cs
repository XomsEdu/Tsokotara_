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
    public CharachterManager navAvatar;

    [Header ("NonSerialized")]
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
        navAvatar = GetComponent<CharachterManager>();
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

    private bool AtPoint() => Vector3.Distance(currentTarget, this.transform.position) <= 0.5;
    public void WalkTo()
        {
            agent.isStopped = false;
            Debug.Log($"Current target: {currentTarget}, Agent position: {transform.position}, Distance: {Vector3.Distance(currentTarget, transform.position)}");
            bool result = agent.SetDestination(currentTarget);
            Debug.Log($"SetDestination success: {result}, Remaining distance: {agent.remainingDistance}, Has path: {agent.hasPath}, Path status: {agent.pathStatus}");  
        }

    /*private void LateUpdate()
        {
            if(navAnimator != null) navAnimator.SetFloat
                ("moveSpeed",agent.velocity.magnitude / agent.speed, 0.1f, Time.deltaTime);
        }*/

    private void LookInDir(Transform target) => StartCoroutine(TakeLook(target));
    
    private IEnumerator TakeLook(Transform target)
        {
            Debug.Log("LookingForSomeone");

            Vector3 dir = (target.position - transform.position).normalized;
            if (Physics.Raycast(transform.position, dir, out RaycastHit hit, 100f, ~LayerMask.GetMask("Ignore Raycast")))
                {
                    if (hit.collider.CompareTag("Player"))
                        {
                            Debug.Log("SeeSomeone");
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
            while(lastNavAction.targets.Count > 1)
                {
                    yield return new WaitForSeconds(5); //to replace with wait for look out anim
                    lastNavAction.SelectTarget();
                    WalkTo();
                    yield return new WaitUntil(() => AtPoint());
                }
            agent.isStopped = true;
            //lastNavAction.PerformJob();
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
            lastNavAction = null;
            inCombat = true;
            if (attackTarget == null || !attackTarget.gameObject.activeInHierarchy) yield break;

            // Sample NavMesh position near the attack target
            Vector3 samplePos = attackTarget.position + Vector3.up * 2f;

            if (NavMesh.SamplePosition(samplePos, out NavMeshHit navHit, 2f, NavMesh.AllAreas))
                {
                    currentTarget = navHit.position;
                    Debug.Log($"[Attack] Sampled NavMesh target at {navHit.position}");
                }
            else
                {
                    currentTarget = attackTarget.position;
                    Debug.LogWarning($"[Attack] Failed to sample NavMesh position near {samplePos}");
                }

            if (!AtPoint())
                {
                    Debug.Log("WalkingTo target");
                    WalkTo();
                }

            // Face the player
            Vector3 lookPos = attackTarget.position - transform.position;
            lookPos.y = 0;
            transform.rotation = Quaternion.LookRotation(lookPos);

            // Attack logic
            Debug.Log("Attacking");
            // navAnimator.SetTrigger("toAttack");
            yield return new WaitForSeconds(2f); // can be replaced by animation wait

            inCombat = false;
            StartCoroutine(TakeLook(attackTarget));
        }


}

[System.Serializable]
public class NavAction
{
    public float time;
    public Transform target;
    public List<Transform> targets;
    private int targetIndex;
    public NavManager navManager;

    public void PerformJob() => navManager.navAnimator.SetTrigger("doJob");

    public void SelectTarget()
        {
            if (targets != null && targets.Count > 0)
                {
                    targetIndex = (targetIndex + 1) % targets.Count;

                    Vector3 samplePos = targets[targetIndex].position + Vector3.up * 2f;

                    if (NavMesh.SamplePosition(samplePos, out NavMeshHit navHit, 2f, NavMesh.AllAreas))
                        {
                            navManager.currentTarget = navHit.position;
                            Debug.Log($"[NavAction] Valid NavMesh target at {navHit.position} (from targets list)");
                        }
                    else
                        {
                            Debug.LogWarning($"[NavAction] SamplePosition failed for: {samplePos}");
                            navManager.currentTarget = navManager.transform.position; // fallback
                        }
                }
            else if (target != null)
                {
                    Vector3 samplePos = target.position + Vector3.up * 2f;

                    if (NavMesh.SamplePosition(samplePos, out NavMeshHit navHit, 2f, NavMesh.AllAreas))
                        {
                            navManager.currentTarget = navHit.position;
                            Debug.Log($"[NavAction] Valid NavMesh target at {navHit.position} (single target)");
                        }
                    else
                        {
                            Debug.LogWarning($"[NavAction] SamplePosition failed for: {samplePos}");
                            navManager.currentTarget = navManager.transform.position; // fallback
                        }
                }
            else
                {
                    Debug.LogWarning("[NavAction] No valid target or targets list to select from.");
                    navManager.currentTarget = navManager.transform.position; // fallback
                }
            }
}