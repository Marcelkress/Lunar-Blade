using System;
using UnityEngine;

public class AnimEvents : MonoBehaviour
{
    [SerializeField] private Collider2D[] damageColliders;
    [SerializeField] private Collider2D healthCollider;
    
    private PlayerHealth playerHealth;
    private CharacterMovement characterMovement;
    private AbilityChargeManager chargeManager;
    private CameraGroupTarget camGroupTarget;
    
    public CameraSettings camSettings;
    
    private void Start()
    {
        camGroupTarget = GetComponentInParent<CameraGroupTarget>();
        characterMovement = GetComponentInParent<CharacterMovement>();
        chargeManager = GetComponentInParent<AbilityChargeManager>();
        playerHealth = healthCollider.GetComponent<PlayerHealth>();
    }

    #region Combat
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

    public void SpecialAttackUI()
    {
        SpecialAttackDisplay.instance.DisplayCharacter(playerHealth.characterIndex);
    }
    
    #endregion
    
    #region Movement
    
    public void LockMove()
    {
        characterMovement.CanMove(false);
    }
    
    public void UnlockMove()
    {
        characterMovement.CanMove(true);
    }
    
    #endregion
    
    #region Camera

    public void SpecialAttackFocus()
    {
        camGroupTarget.FocusOnTarget();
    }
    
    #endregion

}
