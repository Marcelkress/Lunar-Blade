using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IHittable
{
    [Header("Character")]
    public CharacterStats stats;
    public Sprite characterIcon;

    [Header("Refs")]
    public GameObject healthBar;


    [Header("Events")] 
    public UnityEvent TakeHitStaggerEvent,
        takeHitEvent,
        RespawnEvent,
        DeathEvent;

    
    private int maxHealth;
    private int currentHealth;
    private bool invulnerable;
    private InputManager inputManager;

    private Vector3 startPos;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputManager = GetComponentInParent<InputManager>();
        startPos = transform.position;
        invulnerable = false;
        maxHealth = stats.maxHealth;
        currentHealth = maxHealth;
        GameObject _healthCanvas = Instantiate(healthBar);
        _healthCanvas.GetComponentInChildren<PlayerHealthBar>().Init(this, characterIcon);
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
        takeHitEvent?.Invoke();
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

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    private IEnumerator ResetInvulnerability()
    {
        yield return new WaitForSeconds(stats.invulnerabilityTimeAfterHit);
        invulnerable = false;
    }

    private void Die()
    {
        DeathEvent.Invoke();
        invulnerable = true;
        inputManager.canReceiveInput = false;
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(stats.respawnWaitTime);
        RespawnEvent.Invoke();
        
        inputManager.transform.position = startPos;
        inputManager.canReceiveInput = true;
        
        yield return new WaitForSeconds(stats.respawnWaitTime / 2);
        invulnerable = false;
    }
}
    