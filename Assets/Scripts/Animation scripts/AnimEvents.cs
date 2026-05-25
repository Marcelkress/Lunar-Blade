using System;
using UnityEngine;

public class AnimEvents : MonoBehaviour
{
    [SerializeField] private Collider2D[] damageColliders;
    
    public void LockMove()
    {
        GetComponentInParent<CharacterMovement>().CanMove(false);
    }
    
    public void UnlockMove()
    {
        GetComponentInParent<CharacterMovement>().CanMove(true);
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
    
}
