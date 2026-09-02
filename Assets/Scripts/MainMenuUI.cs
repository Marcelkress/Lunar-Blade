using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public string matchSceneName;
    public InputSystemUIInputModule inputModule;
    
    [Header("Panels")] public RectTransform mainPanel;
    public RectTransform mapPanel;
    public RectTransform settingsPanel;
    public RectTransform firstPanel;

    [Header("Panel Positions")] public RectTransform mainPosition;
    public RectTransform rightPosition;
    public RectTransform leftPosition;
    public RectTransform bottomPosition;

    [Header("Panel first selected buttons")]
    public GameObject mainFirstSelectedButton;
    public GameObject mapFirstSelectedButton;
    public GameObject settingsFirstSelectedButton;

    [Header("Animation")] 
    public float animationDuration;

    [Header("Menu Music Layers")] 
    public int mainMusicLayer = 1;
    public int settingsMusicLayer = 2;
    public int mapMusicLayer = 3;
    
    private int firstInputIgnored;
    
    void Start()
    {
        firstInputIgnored = 0;
        inputModule.cancel.action.performed += Back;
        
        MusicManager.instance.SetMenuThemeLayer(mapMusicLayer);

        InputSystem.onAnyButtonPress
            .CallOnce(ctrl => Debug.Log($"Button {ctrl} was pressed"));
        
        InputSystem.onAnyButtonPress.CallOnce(StartGame);
    }

    private void StartGame(InputControl ctrl)
    {
        DoPanelPosition(firstPanel, bottomPosition, mainFirstSelectedButton);
        MusicManager.instance.SetMenuThemeLayer(mainMusicLayer);
        Invoke(nameof(GoToMainPanel),  animationDuration);
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
        firstInputIgnored++;
        if (firstInputIgnored <= 2)
            return;
        
        MusicManager.instance.SetMenuThemeLayer(mapMusicLayer);
        DoPanelPosition(mainPanel, leftPosition, mapFirstSelectedButton);
        DoPanelPosition(mapPanel, mainPosition, mapFirstSelectedButton);
    }

    public void GoToMainPanel()
    {
        firstInputIgnored++;
        
        MusicManager.instance.SetMenuThemeLayer(mainMusicLayer);
        DoPanelPosition(mainPanel, mainPosition, mainFirstSelectedButton);
        DoPanelPosition(mapPanel, rightPosition, mainFirstSelectedButton);
        DoPanelPosition(settingsPanel, rightPosition, mainFirstSelectedButton);
    }

    public void GoToSettingsPanel()
    {
        firstInputIgnored++;
        
        MusicManager.instance.SetMenuThemeLayer(settingsMusicLayer);
        DoPanelPosition(settingsPanel, mainPosition, settingsFirstSelectedButton);
        DoPanelPosition(mainPanel, leftPosition, settingsFirstSelectedButton);
    }

    public void StartMatch()
    {
        SceneManager.LoadScene(matchSceneName);
    }

    private void DoPanelPosition(RectTransform panel, RectTransform target, GameObject selectButton)
    {
        panel.DOAnchorPos(target.anchoredPosition, animationDuration)
            .OnComplete(() => SetSelectedButton(selectButton));
    }

    private void SetSelectedButton(GameObject button)
    {
        EventSystem.current.SetSelectedGameObject(button);
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
        ArenaLoader.instance.LoadArena(ArenaLoader.Arenas.BITDOMAIN);
        MusicManager.instance.SceneLoaded("BITMAN");
    }

    public void Quit()
    {
        Application.Quit();
    }
}