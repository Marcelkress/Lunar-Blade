using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IHittable
{
    [Header("STUFF")]
    public CharacterStats stats;
    public Sprite characterIcon;
    public float fadeBounceTime = 0.2f, fadeAlphaVal = 0.5f;

    [Header("Refs")]
    public GameObject healthBar;
    public SpriteRenderer spriteRenderer;

    [Header("Events")] public UnityEvent TakeHitStaggerEvent;
    public UnityEvent takeHitEvent,
        RespawnEvent,
        DeathEvent;

    
    private int maxHealth;
    private int currentHealth;
    private bool invulnerable;
    private int maxLives;
    private int currentLives;
    
    
    private InputManager input;
    private CharacterMovement movement;
    private Vector3 startPos;

    private PlayerHealthBar UI;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxLives = stats.maxLives;
        currentLives = maxLives;
        startPos = transform.position;
        maxHealth = stats.maxHealth;
        currentHealth = maxHealth;
        invulnerable = false;
        
        input = GetComponentInParent<InputManager>();
        movement = GetComponentInParent<CharacterMovement>();

        UI = GetComponentInParent<PlayerUIManager>().playerHealthBar;
        UI.Init(this, characterIcon, maxLives);
    }

    public bool TakeHit(int damage, bool staggerAttack)
    {
        Debug.Log("hit detected on Player " + GetComponentInParent<InputManager>().playerID);
        
        if (invulnerable)
        {
            Debug.Log("Within i-frames");
            return false;
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
            return true;
        }

        StartCoroutine(ResetInvulnerability());
        return true;
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
        currentLives--;
        input.LockInput(true);

        if (currentLives <= 0)
        {
            Debug.Log("Dead, all lives used");
            return;
        }
        
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(stats.respawnWaitTime);
        currentHealth = maxHealth;
        input.transform.position = startPos;
        input.LockInput(false);
        RespawnEvent.Invoke();
        StartCoroutine(FadeBounce());
        
        movement.EndStagger();
        
        yield return new WaitForSeconds(stats.respawnWaitTime / 2);
        invulnerable = false;
    }

    private IEnumerator FadeBounce()
    {
        while (invulnerable)
        {
            spriteRenderer.DOFade(fadeAlphaVal, fadeBounceTime);
            yield return new WaitForSeconds(fadeBounceTime);
            spriteRenderer.DOFade(1, fadeBounceTime);
            yield return new WaitForSeconds(fadeBounceTime);
        }
        
        spriteRenderer.DOFade(1, fadeBounceTime);
    }
}
    