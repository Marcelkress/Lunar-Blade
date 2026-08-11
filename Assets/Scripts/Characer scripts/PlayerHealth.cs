using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour, IHittable
{
    [Header("STUFF")]
    public CharacterStats stats;
    public Sprite characterIcon;
    public float fadeBounceTime = 0.2f, fadeAlphaVal = 0.5f;

    [Header("Refs")]
    public GameObject healthBar;
    public SpriteRenderer spriteRenderer;

    [Header("Audio Events")] public UnityEvent TakeHitStaggerEvent;
    public UnityEvent takeHitNoStaggerEvent,
        RespawnEvent,
        DeathEvent,
        PermadeathEvent,
        SuccesfulDeflectEvent;
    
    private int maxHealth;
    private int currentHealth;
    private bool invulnerable;
    private int maxLives;
    private int currentLives;
    
    private DeflectAbility deflectAbility;
    private InputManager input;
    private CharacterMovement movement;
    private Vector3 startPos;

    private PlayerHealthBar UI;
    
    public void Init(int _maxLives)
    {
        maxLives = _maxLives;
        currentLives = maxLives;
        startPos = transform.position;
        maxHealth = stats.maxHealth;
        currentHealth = maxHealth;
        invulnerable = false;
        
        input = GetComponentInParent<InputManager>();
        movement = GetComponentInParent<CharacterMovement>();
        deflectAbility = GetComponent<DeflectAbility>();
        
        UI = GetComponentInParent<PlayerUIManager>().playerHealthBar;
        UI.Init(this, characterIcon, maxLives);
    }
    

    public bool TakeHit(int damage, bool staggerAttack, out bool deflected, bool specialAttack)
    {
        //Debug.Log("hit detected on Player " + GetComponentInParent<InputManager>().playerID);
        deflected = false;
        
        if (invulnerable)
        {
            // Debug.Log("Invulnerable");
            return false;
        }

        if (deflectAbility.IsDeflecting() && !specialAttack)
        {
            // Do something else
            deflected = true;
            SuccesfulDeflectEvent.Invoke();
            return false;
        }

        currentHealth -= damage;
        
        takeHitNoStaggerEvent?.Invoke();
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

        if (currentLives < 0)
        {
            PermadeathEvent.Invoke();
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
    