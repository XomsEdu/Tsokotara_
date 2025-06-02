using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public int HPMax; public int HPCurrent; //Health points
    [field: SerializeField] public float moveSpeedMultiplier {get; private set;}
    [field: SerializeField] public float jumpForceMultiplier {get; private set;}   public int jumpAmount;
    public float protectionMultiplier;

    public Dictionary<PassiveEffect, float> passiveEffectStack = new();
    //This list is updated from equipment slots

    /* public float experiencePerLevel; private float experienceCurrent;
    public int level {get; private set;} */


    private void Start() => HPCurrent = HPMax;

    public float moveSpeed;
    private void moveSpeedUpdate(float amount) => moveSpeed = moveSpeedMultiplier * amount;

    public float jumpForce;
    public void jumpForceUpdate(float amount) => jumpForce = jumpForceMultiplier + amount;

    public float protectionUpdate(float amount) => 
        protectionMultiplier + amount;

    private bool toDieCheck() => HPCurrent <= 0;

    public float HPUpdate(int amount)
        {
            HPCurrent += amount;
            if(toDieCheck())
                {
                    StopAllCoroutines();
                    gameObject.SetActive(false);
                }
            return HPCurrent;
        }

    private Coroutine damageRoutine;
    private Coroutine speedRoutine;
    private Coroutine jumpRoutine;

    public void SetEffect(List<StatusEffect> effectList, ItemUsable itemStats)
        {
            foreach (var effect in effectList)
                {
                    if(!this.gameObject.activeInHierarchy) break;
                    switch (effect.statusEffectType)
                        {
                            case StatusEffectType.damage:   // Apply immediate damage to the target
                                HPUpdate(-(int)((itemStats.strength * effect.force)/protectionMultiplier));
                                break;

                            case StatusEffectType.heal:     // Apply healing to the target
                                HPUpdate(Mathf.Min(HPMax - HPCurrent,(int)(itemStats.strength * effect.force)));
                                break;

                            case StatusEffectType.stan:
                                moveSpeedUpdate(0);         //Apply stan
                                if(speedRoutine != null) StopCoroutine(speedRoutine); 
                                speedRoutine = StartCoroutine
                                    (EffectTimer(effect.ticks, () => moveSpeedUpdate(moveSpeedMultiplier)));
                                break;

                            case StatusEffectType.damageOT:
                                if(damageRoutine != null) StopCoroutine(damageRoutine);
                                damageRoutine = StartCoroutine      //Apply DOT
                                    (RepeatingEffect((int)effect.ticks,() => 
                                        HPUpdate (-(int)effect.force)
                                    ));
                                break;

                            default:
                                Debug.LogWarning("Unknown effect type");
                                break;
                        }
                }

            
        }

    public IEnumerator RepeatingEffect(int ticks, Action applyEffect)
        {
            while (ticks-- > 0)
            {
                applyEffect?.Invoke();
                yield return new WaitForSeconds(1f);
            }
        }

    public IEnumerator EffectTimer(int ticks, Action onTimerComplete)
        {
            yield return new WaitForSeconds(ticks);
            onTimerComplete?.Invoke();
        }
}

//TODO: Manage dying, probably some die list for  bodies to disapear after some time or simply unload them
//TODO: Add effects like knoback
