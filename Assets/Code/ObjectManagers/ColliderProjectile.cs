using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class ColliderProjectile : MonoBehaviour
{
    [SerializeField] private float liveTime;
    [SerializeField] private bool isPhysicsBased;
    [SerializeField] private bool isDestroyedOnContact;
    public bool canParry;
    public Transform transformThis;

    public List<StatusEffect> statusEffects;
    public ItemUsable itemStats;
    [NonSerialized] public ProjectileShooter interactionItem;
    [NonSerialized] public GameObject prefabThis;
    private GameObject shooterOfProjectile;
    private Rigidbody thisRigidbody;

    private void Awake() => isPhysicsBased = (thisRigidbody = GetComponent<Rigidbody>()) != null;

    public void ShootProjectile(GameObject shooter)
        {
            shooterOfProjectile = shooter;
            gameObject.SetActive(true);
            StartCoroutine(ObjectLive());
        }

    private void OnTriggerEnter(Collider other)
        {
            if(shooterOfProjectile == other.gameObject) return;

            StatsManager enemyStats = other.GetComponent<StatsManager>();
            if (enemyStats != null)
                {
                    enemyStats.SetEffect(statusEffects, itemStats);
                }
            if(isDestroyedOnContact && !other.isTrigger)
                {
                    interactionItem.projectilePool.Add(this);
                    gameObject.SetActive(false);
                }

            //ColliderProjectile opposingProjectile = other.GetComponent<ColliderProjectile>();
            //if(opposingProjectile != null && opposingProjectile.canParry)
                //thisRigidbody.AddForce(-transform.forward * itemStats.speed, ForceMode.Impulse);
        }

    private IEnumerator ObjectLive()
        {
            transform.position = interactionItem.spawnerPosition.position;
            transform.rotation = interactionItem.spawnerPosition.rotation;
            interactionItem.projectilePool.Remove(this);
            thisRigidbody.AddForce(transform.forward * itemStats.speed, ForceMode.Impulse);
            yield return new WaitForSeconds(liveTime);
            gameObject.SetActive(false);
            interactionItem.projectilePool.Add(this);
            thisRigidbody.velocity = Vector3.zero;
            thisRigidbody.angularVelocity = Vector3.zero;
        }
}

//TODO: maybe some parrying mechanic in future but it`s later
//Need tag setting to avoid friendly and self fire