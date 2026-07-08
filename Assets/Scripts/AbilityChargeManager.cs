using UnityEngine;

public class AbilityChargeManager : MonoBehaviour
{
    public CharacterStats stats;
    
    public bool hasCharge;
    public int requiredHits, currentHits;
    
    private AbilityChargeUI UI;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        requiredHits = stats.requiredHitsToCharge;
        currentHits = 0;
        hasCharge = false;

        UI = GetComponentInParent<PlayerUIManager>().abilityChargeUI;
        UI.Init(requiredHits, currentHits);
    }

    public void SuccessfulHit()
    {
        currentHits++;
        
        // Debug.Log($"[{gameObject.name}] SuccessfulHit called. currentHits={currentHits}/{requiredHits}, instanceID={GetInstanceID()}");
        
        UI.UpdateUI(currentHits);
    
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
            currentHits = 0;
            UI.UpdateUI(currentHits);
            return true;
        }
        
        return false;
    }
    
}
