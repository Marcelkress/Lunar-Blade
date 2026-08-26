using System;
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

    [Header("Menu Music Layers")] 
    public int mainLayer = 1;
    public int settingsLayer = 2;
    public int mapLayer = 3;
    
    void Start()
    {
        inputModule.cancel.action.performed += Back;
        MusicManager.instance.SetMenuThemeLayer(mainLayer);
    }

    private void OnDestroy()
    {
        inputModule.cancel.action.performed -= Back;
    }

    private void Back(InputAction.CallbackContext ctx)
    {
        GoToMainPanel();
    }
    
    public void GoToMapPanel()
    {
        MusicManager.instance.SetMenuThemeLayer(mapLayer);
        DoPanelPosition(mainPanel, leftPosition);
        DoPanelPosition(mapPanel, mainPosition);
        EventSystem.current.SetSelectedGameObject(mapFirstSelectedButton);
    }

    public void GoToMainPanel()
    {
        MusicManager.instance.SetMenuThemeLayer(mainLayer);
        DoPanelPosition(mainPanel, mainPosition);
        DoPanelPosition(mapPanel, rightPosition);
        DoPanelPosition(settingsPanel, rightPosition);
        EventSystem.current.SetSelectedGameObject(mainFirstSelectedButton);
    }

    public void GoToSettingsPanel()
    {
        MusicManager.instance.SetMenuThemeLayer(settingsLayer);
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
        MusicManager.instance.SceneLoaded("Caves");
        ArenaLoader.instance.LoadArena(ArenaLoader.Arenas.Caves);
    }
    
    public void LoadForestArena()
    {
        MusicManager.instance.SceneLoaded("Forest");
        ArenaLoader.instance.LoadArena(ArenaLoader.Arenas.Forest);
    }

    public void LoadBITDOMAINArena()
    {
        MusicManager.instance.SceneLoaded("Caves");
        ArenaLoader.instance.LoadArena(ArenaLoader.Arenas.Forest);
    }

    public void Quit()
    {
        Application.Quit();
    }
}