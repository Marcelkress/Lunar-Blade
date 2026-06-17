using UnityEngine;

public class PlayerHealthBar : MonoBehaviour
{
    private Transform followTransform;
    private RectTransform rectTransform, canvasRectTransform;
    
    public void Init(Transform followObj)
    {
        followTransform = followObj;
        canvasRectTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        rectTransform = GetComponent<RectTransform>();
    }
    
    
    void LateUpdate()
    {
        Vector2 viewportPosition = Camera.main.WorldToViewportPoint(followTransform.transform.position);
        
        Vector2 worldObjectScreenPosition = new Vector2(
            ((viewportPosition.x * canvasRectTransform.sizeDelta.x) - (canvasRectTransform.sizeDelta.x * 0.5f)),
            ((viewportPosition.y * canvasRectTransform.sizeDelta.y) - (canvasRectTransform.sizeDelta.y * 0.5f)));

        
        rectTransform.anchoredPosition = worldObjectScreenPosition;
    }
}
