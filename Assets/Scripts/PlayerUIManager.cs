using System;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    public GameObject playerUIPrefab;
    
    [HideInInspector] public GameObject playerUI;
    [HideInInspector] public PlayerHealthBar playerHealthBar;
    [HideInInspector] public AbilityChargeUI abilityChargeUI;

    private void Awake()
    {
        playerUI = Instantiate(playerUIPrefab);
        
        playerHealthBar = playerUI.GetComponent<PlayerHealthBar>();
        abilityChargeUI = playerUI.GetComponent<AbilityChargeUI>();
    }
}
