using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProjectileShooter : InteractionItem
{
    public List<ColliderProjectile> projectilePool = new(); //this list will exist somewhere outside on singleton
    private ColliderProjectile projectileToUse;

    public override void SetAction(ComboMove move)
        {
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
        }

    public override void ExecuteAction()
        {
            if (projectileToUse != null) projectileToUse.ShootProjectile(owner);
        }
}