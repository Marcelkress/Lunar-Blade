using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class MainMenuUI : MonoBehaviour
{
    [Header("Panels")] public RectTransform mainPanel;
    public RectTransform mapPanel;

    [Header("Panel Positions")] public RectTransform mainPosition;
    public RectTransform rightPosition;
    public RectTransform leftPosition;

    [Header("Panel first selected buttons")]
    public GameObject mainFirstSelectedButton;
    public GameObject mapFirstSelectedButton;
    
    [Header("Animation")] public float animationDuration;

    public void GoToMapPanel()
    {
        DoPanelPosition(mainPanel, leftPosition);
        DoPanelPosition(mapPanel, mainPosition);
        EventSystem.current.SetSelectedGameObject(mapFirstSelectedButton);
    }

    public void GoToMainPanel()
    {
        DoPanelPosition(mainPanel, mainPosition);
        DoPanelPosition(mapPanel, rightPosition);
        EventSystem.current.SetSelectedGameObject(mainFirstSelectedButton);
    }

    public void StartMatch()
    {
        
    }

    private void DoPanelPosition(RectTransform panel, RectTransform target)
    {
        panel.DOAnchorPos(target.anchoredPosition, animationDuration);
    }
}