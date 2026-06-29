    using System;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitDetection : MonoBehaviour
{
    private List<Collider2D> alreadyHit;
    private CharacterStats stats;
    [SerializeField] private int attackID;
    public bool staggerEnemyOnHit = true;
    
    private AbilityChargeManager abilityChargeManager;

    private void Start()
    {
        var owner = GetComponentInParent<CharacterMovement>();
        stats = owner.moveStats;
        alreadyHit = new List<Collider2D>();

        var myCollider = GetComponent<Collider2D>();
        abilityChargeManager = GetComponentInParent<AbilityChargeManager>();
        
        var ownerColliders = owner.GetComponentsInChildren<Collider2D>();
        foreach (var c in ownerColliders)
        {
            if (c == myCollider) continue;
            Physics2D.IgnoreCollision(myCollider, c, true);
        }
    }

    private void OnTriggerStay2D(Collider2D otherCol)
    {
        if (otherCol.gameObject.TryGetComponent(out IHittable hit))
        {
            foreach (var alrHit in alreadyHit)
            {
                if (alrHit == otherCol)
                    return;
            }

            int dmg;

            switch (attackID)
            {
                case 1:
                    dmg = stats.attackOneDmg;
                    break;
                case 2:
                    dmg = stats.attackTwoDmg;
                    break;
                case 3:
                    dmg = stats.attackThreeDmg;
                    break;
                case 4:
                    dmg = stats.specialAttackDmg;
                    break;
                default:
                    dmg = 1;
                    break;
            }

            if (hit.TakeHit(dmg, staggerEnemyOnHit))
            {
                abilityChargeManager.SuccessfulHit();
            }
            
            alreadyHit.Add(otherCol);
        }
    }

    public void ClearHits()
    {
        alreadyHit.Clear();
    }
}

/// <summary>
/// Anything that can take hits from players
/// </summary>
public interface IHittable
{
    public bool TakeHit(int damage, bool staggerAttack);
}
