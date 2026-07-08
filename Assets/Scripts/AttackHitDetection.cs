    using System;
using System.Collections.Generic;
using UnityEngine;

public enum AttackID
{
    AttackOne = 1,
    AttackTwo = 2,
    AttackThree = 3,
    SpecialAttack = 4
}
    
public class AttackHitDetection : MonoBehaviour
{
    private List<Collider2D> alreadyHit;
    private CharacterStats stats;
    [SerializeField] private AttackID attackID;
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
            // If we have already hit the collider we ignore it
            foreach (var alrHit in alreadyHit)
            {
                if (alrHit == otherCol)
                    return;
            }
            
            // Assign damage based on attack type
            int dmg;
            switch (attackID)
            {
                case AttackID.AttackOne:
                    dmg = stats.attackOneDmg;
                    break;
                case AttackID.AttackTwo:
                    dmg = stats.attackTwoDmg;
                    break;
                case AttackID.AttackThree:
                    dmg = stats.attackThreeDmg;
                    break;
                case AttackID.SpecialAttack:
                    dmg = stats.specialAttackDmg;
                    break;
                default:
                    dmg = 1;
                    break;
            }

            // Applying damage
            if (hit.TakeHit(dmg, staggerEnemyOnHit, attackID == AttackID.SpecialAttack)) 
            { 
                // Slowdown time effect if a special attack is hit 
                if (attackID == AttackID.SpecialAttack)
                {
                    TimeManager.instance.SlowDown();   
                }
                // Charge special attack if we hit a normal attack
                else 
                {
                    abilityChargeManager.SuccessfulHit();
                }

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
    public bool TakeHit(int damage, bool staggerAttack, bool specialAttack);
}
