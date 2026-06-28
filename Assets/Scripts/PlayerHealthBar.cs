using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerHealthBar : MonoBehaviour
{
    private Slider healthSlider;
    private PlayerHealth playerHealth;
    private Image[] lives;
    private int lifeIndex;

    public float slideTime;
    public Image characterIcon;
    public GameObject lifeUnit_Prefab;
    public Transform lifeUnit_Parent;
    
    public void Init(PlayerHealth _playerHealth, Sprite _characterSprite, int _maxLives)
    {
        transform.SetParent(HealthBarCanvas.instance.transform);
        
        // Set references
        healthSlider = GetComponentInChildren<Slider>();
        playerHealth = _playerHealth;
        characterIcon.sprite = _characterSprite;

        // Assign listeners
        playerHealth.takeHitEvent.AddListener(UpdateBar);
        playerHealth.DeathEvent.AddListener(UpdateBar);
        playerHealth.RespawnEvent.AddListener(ResetHealthBar);
        playerHealth.DeathEvent.AddListener(UpdateLives);
        
        // Set values
        healthSlider.maxValue = playerHealth.stats.maxHealth;
        healthSlider.value = 0;
        healthSlider.DOValue(playerHealth.stats.maxHealth, slideTime);

        lives = new Image[_maxLives];
        lifeIndex = _maxLives - 1;
        for (int i = 0; i < _maxLives; i++)
        {
            GameObject lifeObj = Instantiate(lifeUnit_Prefab, lifeUnit_Parent);
            lives[i] = lifeObj.GetComponent<Image>();
        }
    }

    private void UpdateBar()
    {
        healthSlider.DOValue(playerHealth.GetCurrentHealth(), slideTime);
    }

    private void ResetHealthBar()
    {
        healthSlider.DOValue(healthSlider.maxValue, slideTime);
    }

    private void UpdateLives()
    {
        if (lifeIndex <= -1)
            return;
        
        lives[lifeIndex].DOFade(0, slideTime);
        lifeIndex--;
    }
}
