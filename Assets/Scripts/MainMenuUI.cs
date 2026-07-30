using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public string matchSceneName;
    
    [Header("Panels")] public RectTransform mainPanel;
    public RectTransform mapPanel;
    public RectTransform settingsPanel;

    [Header("Panel Positions")] public RectTransform mainPosition;
    public RectTransform rightPosition;
    public RectTransform leftPosition;

    [Header("Panel first selected buttons")]
    public GameObject mainFirstSelectedButton;
    public GameObject mapFirstSelectedButton;
    public GameObject settingsFirstSelectedButton;
    
    [Header("Animation")] 
    public float animationDuration;

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
        DoPanelPosition(settingsPanel, rightPosition);
        EventSystem.current.SetSelectedGameObject(mainFirstSelectedButton);
    }

    public void GoToSettingsPanel()
    {
        DoPanelPosition(settingsPanel, mainPosition);
        DoPanelPosition(mainPanel, leftPosition);
        EventSystem.current.SetSelectedGameObject(settingsFirstSelectedButton);
    }

    public void StartMatch()
    {
        SceneManager.LoadScene(matchSceneName);
    }

    private void DoPanelPosition(RectTransform panel, RectTransform target)
    {
        panel.DOAnchorPos(target.anchoredPosition, animationDuration);
    }

    public void LoadCavesArena()
    {
        ArenaLoader.instance.LoadArena(ArenaLoader.Arenas.Caves);
    }
    
    public void LoadForestArena()
    {
        ArenaLoader.instance.LoadArena(ArenaLoader.Arenas.Forest);
    }
}