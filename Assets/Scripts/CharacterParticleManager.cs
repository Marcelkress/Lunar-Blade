using UnityEngine;

public class CharacterParticleManager : MonoBehaviour
{
    public float jumpDisableTime = 0.2f;
    
    private ParticleSystem ps;
    private CharacterMovement movement;

    private ParticleSystem.EmissionModule mod;
        
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movement = GetComponent<CharacterMovement>();
        ps = GetComponent<ParticleSystem>();
        mod = ps.emission;
    }
    
    void Update()
    {
        if (movement.isGrounded)
        {
            mod.enabled = true;
        }
        else
        {
            Invoke(nameof(DisableEmission), jumpDisableTime);
        }
    }

    void DisableEmission()
    {
        mod.enabled = false;
    }
    
}
