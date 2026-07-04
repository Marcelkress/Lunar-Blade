using System;
using UnityEngine;

public class AnimEvents : MonoBehaviour
{
    [SerializeField] private Collider2D[] damageColliders;
    [SerializeField] private Collider2D healthCollider;
    
    private CharacterMovement characterMovement;
    private AbilityChargeManager chargeManager;

    private void Start()
    {
        characterMovement = GetComponentInParent<CharacterMovement>();
        chargeManager = GetComponentInParent<AbilityChargeManager>();
    }

    public void LockMove()
    {
        characterMovement.CanMove(false);
    }
    
    public void UnlockMove()
    {
        characterMovement.CanMove(true);
    }

    public void StartStagger()
    {
        characterMovement.StartStagger();
    }

    public void EndStagger()
    {
        characterMovement.EndStagger();
    }
    public void DisableHealthCollider()
    {
        healthCollider.enabled = false;
    }
    public void EnableHealthCollider()
    {
        healthCollider.enabled = true;
    }
    
    public void EnableDamageCollider(int colliderID)
    {
        //Debug.Log("Enable damage col");
        damageColliders[colliderID - 1].enabled = true;
    }
    
    public void DisableDamageCollider(int colliderID)
    {
        damageColliders[colliderID - 1].enabled = false;
    }

    public void ClearHits(int colliderID)
    {
        damageColliders[colliderID - 1].gameObject.GetComponent<AttackHitDetection>().ClearHits();
    }

    private void ConsumeCharge()
    {
        chargeManager.ConsumeCharge();
    }
    
}
