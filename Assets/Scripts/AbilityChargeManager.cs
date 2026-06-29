using UnityEngine;

public class AbilityChargeManager : MonoBehaviour
{
    public CharacterStats stats;
    
    private bool hasCharge;
    private int requiredHits, currentHits;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        requiredHits = stats.requiredHitsToCharge;
        currentHits = 0;
        hasCharge = false;
    }

    public void SuccessfulHit()
    {
        currentHits++;

        if (currentHits >= requiredHits)
        {
            hasCharge = true;
        }
    }

    public bool ConsumeCharge()
    {
        if (hasCharge)
        {
            hasCharge = false;
            return true;
        }
        
        return false;
    }
    
}
