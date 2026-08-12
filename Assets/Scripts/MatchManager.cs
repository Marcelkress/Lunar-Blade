using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MatchManager : MonoBehaviour
{
    [Header("UI Panels")]
    public RectTransform lifeCountPanel;
    public RectTransform playerCountPanel;
    public RectTransform winPanel;
    public RectTransform characterSelectionPanel;
    public float animationDuration = 0.5f;

    [Header("UI References")] public GameObject masterCanvas;
    public TMP_Text lifeCountDisplay;
    public TMP_Text playerWinText;
    public GameObject mainMenuButton;
    public Image tintImage;

    [Header("First selected buttons")]
    public GameObject lifeCountButton;
    public GameObject playerCountButton;
    
    [Header("UI Panel Positions")] 
    public RectTransform mainPosition;
    public RectTransform rightPosition, leftPosition;
    
    [Header("References")] 
    public CharacterSelectionManager characterSelectionManager;
    
    [Header("Match settings")]public MatchSettings matchSettings;
    
    [Header("UI Input")]
    [SerializeField] private InputActionAsset uiActionsTemplate; // assign the SAME asset you use elsewhere, in the Inspector

    [Header("Audio Events")] 
    public UnityEvent matchStartEvent;
    public UnityEvent playerWinEvent;
    
    private InputActionAsset uiActionsInstance;
    private InputSystemUIInputModule uiModule;
    
    private bool startedMatch;
    private int lifeCount;
    
    private PlayerHealth[] players;
    private int deadPlayerCount;
    
    public static MatchManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        masterCanvas.SetActive(true);
        ArenaLoader.instance.LoadArena(ArenaLoader.Arenas.Caves);
        
        uiModule = EventSystem.current.GetComponent<InputSystemUIInputModule>();
        uiActionsInstance = Instantiate(uiActionsTemplate); 
        uiModule.actionsAsset = uiActionsInstance;
        uiActionsInstance.Enable();
    }
    
    private void Start()
    {
        tintImage.enabled = true;
        DoPanelPosition(lifeCountPanel, mainPosition);
        EventSystem.current.SetSelectedGameObject(lifeCountButton);
        lifeCount = matchSettings.defaultMatchLives;
        lifeCountDisplay.text = lifeCount.ToString();
        startedMatch = false;
    }
    
    private void DoPanelPosition(RectTransform panel, RectTransform target)
    {
        panel.DOAnchorPos(target.anchoredPosition, animationDuration);
    }

    public void IncreaseLifeCount()
    {
        lifeCount++;
        if (lifeCount > matchSettings.maxAllowedLives)
        {
            lifeCount = matchSettings.maxAllowedLives;
        }
        lifeCountDisplay.text = lifeCount.ToString();
    }
    
    public void DecreaseLifeCount()
    {
        lifeCount--;
        if (lifeCount < 1)
        {
            lifeCount = 1;
        }
        lifeCountDisplay.text = lifeCount.ToString();
    }

    public void SetMatchLives()
    {
        DoPanelPosition(lifeCountPanel, leftPosition);
        DoPanelPosition(playerCountPanel, mainPosition);
        EventSystem.current.SetSelectedGameObject(playerCountButton);
        characterSelectionManager.SetLifeCount(lifeCount);
    }

    public void PlayerCountSet()
    {
        DoPanelPosition(playerCountPanel, leftPosition);
        DoPanelPosition(characterSelectionPanel, mainPosition);
    }

    public void StartingMatch(int playerCount, PlayerHealth player, int i)
    {
        if(players == null)
            players = new PlayerHealth[playerCount];
        
        players[i] = player;
        player.PermadeathEvent.AddListener(WinCheck);
        DoPanelPosition(characterSelectionPanel, leftPosition);
        tintImage.enabled = false;
    }
    
    private void WinCheck()
    {
        deadPlayerCount++;
        
        if (deadPlayerCount >= players.Length - 1) // If only one player is left they have won
        {
            foreach (var player in players)
            {
                if (player.GetCurrentHealth() > 0)
                {
                    Invoke(nameof(MatchEnd), matchSettings.winPanelWaitTime);
                    return;
                }
            }
        }
    }

    private void MatchEnd()
    {
        tintImage.enabled = true;
        DoPanelPosition(winPanel, mainPosition);
        EventSystem.current.SetSelectedGameObject(mainMenuButton);
        
        foreach (var player in players)
        {
            player.GetComponentInParent<PlayerInput>().SwitchCurrentActionMap("UI");
            playerWinText.text = "Player " + player.GetComponentInParent<InputManager>().playerID + " wins!";
        }
    }

    public void ReplayMatch()
    {
        DoPanelPosition(winPanel, rightPosition);
        
        foreach (var player in players)
        {
            Destroy(player.GetComponentInParent<PlayerUIManager>().playerUI.gameObject);
            Destroy(player.GetComponentInParent<PlayerInput>().gameObject);
        }

        players = null;

        InputManager.playerCount = 0;
        uiActionsInstance.Enable();
        deadPlayerCount = 0;
        characterSelectionManager.Reset();
        DoPanelPosition(lifeCountPanel, mainPosition);
        EventSystem.current.SetSelectedGameObject(lifeCountButton);
        lifeCount = matchSettings.defaultMatchLives;
        lifeCountDisplay.text = lifeCount.ToString();
        startedMatch = false;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        ArenaLoader.instance.UnLoadArena();
    }
    
}
