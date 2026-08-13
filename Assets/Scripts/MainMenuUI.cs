using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public string matchSceneName;
    public InputSystemUIInputModule inputModule;
    
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
    
    void Start()
    {
        inputModule.cancel.action.performed += Back;
    }

    private void Back(InputAction.CallbackContext ctx)
    {
        GoToMainPanel();
    }
    
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

    public void Quit()
    {
        Application.Quit();
    }
}