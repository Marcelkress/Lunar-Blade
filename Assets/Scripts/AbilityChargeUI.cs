using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AbilityChargeUI : MonoBehaviour
{
    public Image chargeIndicator;
    public Slider chargeSlider;

    public Color minValueColor, maxValueColor; 
    
    private int requiredHits;
        
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Init(int _requiredHits, int currentHits)
    {
        requiredHits = _requiredHits;
        chargeIndicator.color = minValueColor;
        
        chargeSlider.maxValue = requiredHits;
        chargeSlider.value = currentHits;
        
        UpdateUI(currentHits);
    }

    public void UpdateUI(int currentHits)
    {
        chargeSlider.DOValue(currentHits, 0.2f);
        chargeIndicator.DOColor(GetColor(currentHits),  0.2f);
    }
    
    private Color GetColor(float val)
    { 
        float lerpVal = val /= requiredHits;
        lerpVal = Mathf.Clamp(val, 0, 1);
        
        // Debug.Log(lerpVal);
        
        Color col = Color.Lerp(minValueColor, maxValueColor, lerpVal);
        col.a = 1;
        return col;
    }
    
}
