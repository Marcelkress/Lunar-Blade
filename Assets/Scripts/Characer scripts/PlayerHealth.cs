using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour, IHittable
{
    public CharacterStats stats;
    private int maxHealth;
    private int currentHealth;
    private bool invulnerable;
    
    public UnityEvent TakeHitEvent;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        invulnerable = false;
        maxHealth = stats.maxHealth;
        currentHealth = maxHealth;
    }

    public void TakeHit(int damage)
    {
        Debug.Log("hit detected on Player " + GetComponentInParent<InputManager>().playerID);
        
        if (invulnerable)
        {
            Debug.Log("Within i-frames");
            return;
        }
        
        currentHealth -= damage;
        UpdateHealthUI();
        invulnerable = true;
        TakeHitEvent.Invoke();

        if (currentHealth <= 0)
        {
            Die();
        }

        StartCoroutine(ResetInvulnerability());
    }

    private IEnumerator ResetInvulnerability()
    {
        yield return new WaitForSeconds(stats.invulnerabilityTimeAfterHit);
        invulnerable = false;
    }

    private void Die()
    {
        Debug.Log("Player dead");
    }

    private void UpdateHealthUI()
    {
        // TODO implement
    }
}
