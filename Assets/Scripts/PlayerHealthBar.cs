using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerHealthBar : MonoBehaviour
{
    private Slider healthSlider;
    private PlayerHealth playerHealth;

    [Header("Settings")] public float slideTime;
    public Image characterIcon;
    
    public void Init(PlayerHealth _playerHealth, Sprite _characterSprite)
    {
        transform.SetParent(HealthBarCanvas.instance.transform);
        
        // Set references
        healthSlider = GetComponentInChildren<Slider>();
        playerHealth = _playerHealth;
        characterIcon.sprite = _characterSprite;

        // Assign listeners
        playerHealth.takeHitEvent.AddListener(UpdateBar);
        playerHealth.DeathEvent.AddListener(ResetHealthBar);
        
        // Set values
        healthSlider.maxValue = playerHealth.stats.maxHealth;
        healthSlider.value = 0;
        healthSlider.DOValue(playerHealth.stats.maxHealth, slideTime);
    }

    private void UpdateBar()
    {
        healthSlider.DOValue(playerHealth.GetCurrentHealth(), slideTime);
    }

    private void ResetHealthBar()
    {
        healthSlider.DOValue(healthSlider.maxValue, slideTime);
    }
}
