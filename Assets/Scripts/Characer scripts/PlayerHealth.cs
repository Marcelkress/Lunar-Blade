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
    
    public UnityEvent TakeHitStaggerEvent;
    public GameObject healthCanvas;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        invulnerable = false;
        maxHealth = stats.maxHealth;
        currentHealth = maxHealth;
        GameObject _healthCanvas = Instantiate(healthCanvas);
        _healthCanvas.GetComponentInChildren<PlayerHealthBar>().Init(this.transform);
    }

    public void TakeHit(int damage, bool staggerAttack)
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

        if (staggerAttack)
        {
            TakeHitStaggerEvent.Invoke();
        }

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
