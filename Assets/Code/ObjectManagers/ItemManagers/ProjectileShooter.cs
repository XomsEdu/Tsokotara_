using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProjectileShooter : InteractionItem
{
    public List<ColliderProjectile> projectilePool = new(); //this list will exist somewhere outside on singleton
    private ColliderProjectile projectileToUse;

    public override void SetAction(ComboMove move)
        {
            Debug.Log("MoveScheduled");
            if (item.comboMoves == null || item.comboMoves.Count == 0) return;

            bool hasPool = projectilePool != null && projectilePool.Count > 0;

            foreach (ComboMove moveInit in item.comboMoves)
            {
                if (hasPool)
                    {
                        var foundProjectile = projectilePool.FirstOrDefault(proj => proj.prefabThis == moveInit.projectile);

                        if (foundProjectile != null)
                        {
                            foundProjectile.interactionItem = this;
                            foundProjectile.statusEffects = move.statusEffects;
                            foundProjectile.itemStats = item;
                            projectileToUse = foundProjectile;
                            continue; // Skip instantiation
                        }
                    }

                GameObject cloneProjectile = Instantiate(moveInit.projectile);
                cloneProjectile.SetActive(false);
                var projectileScript = cloneProjectile.GetComponent<ColliderProjectile>();

                if (projectileScript != null)
                    {
                        projectilePool.Add(projectileScript);
                        projectileScript.statusEffects = move.statusEffects;
                        projectileScript.itemStats = item;
                        projectileScript.prefabThis = moveInit.projectile;
                        projectileScript.interactionItem = this;
                        projectileToUse = projectileScript;
                    }
                else Destroy(cloneProjectile);
                //last if checks are probably temporal
            }
            //for each collider or for raycast to be instantiated
            //set list of status effects from combo move that was passed
            //set item info in order for StatsManager to correctly calculate DMG or whtever
        }

    public override void ExecuteAction()
        {
            if (projectileToUse != null) projectileToUse.ShootProjectile();
            else Debug.LogWarning($"[{name}] No matching projectile found in pool for: {projectileToUse.name}");
        }
}



/*
Attacks and interractions:
- Spawn a [physical] GO with collider (Bullet, fireball, other magical attack);
- Shoot a raycast hitscan (immidiate bullet, Lazer);
- Activate Colliders (on Swords or other melee)
- Invoke an event (for stuff like c4)

Let`s store and instantiate projectiles/hit colliders in pool in hierchy,
changing bullets visually before spawning
*/