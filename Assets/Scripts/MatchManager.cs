using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MatchManager : MonoBehaviour
{
    [Header("UI Panels")]
    public RectTransform lifeCountPanel;
    public RectTransform playerCountPanel;
    public RectTransform winPanel;
    public float animationDuration = 0.5f;
    
    [Header("UI References")] 
    public TMP_Text lifeCountDisplay;
    public TMP_Text playerWinText;
    public GameObject mainMenuButton;
    public GameObject tintImage;

    [Header("First selected buttons")]
    public GameObject lifeCountButton;
    public GameObject playerCountButton;
    
    [Header("UI Panel Positions")] 
    public RectTransform mainPosition;
    public RectTransform rightPosition, leftPosition;
    
    [Header("References")] 
    public CharacterSelectionManager characterSelectionManager;
    
    [Header("Match settings")]public MatchSettings matchSettings;
    
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
    }
    
    private void Start()
    {
        tintImage.SetActive(true);
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
        if (lifeCount < 0)
        {
            lifeCount = 0;
        }
        lifeCountDisplay.text = lifeCount.ToString();
    }

    public void SetMatchLives()
    {
        DoPanelPosition(lifeCountPanel, leftPosition);
        DoPanelPosition(playerCountPanel, mainPosition);
        EventSystem.current.SetSelectedGameObject(playerCountButton);
        characterSelectionManager.EnableChoosePlayers(lifeCount);
    }

    public void PlayerCountSet()
    {
        DoPanelPosition(playerCountPanel, leftPosition);
    }

    public void StartingMatch(int playerCount, PlayerHealth player, int i)
    {
        if(players == null)
            players = new PlayerHealth[playerCount];
        
        players[i] = player;
        player.PermadeathEvent.AddListener(WinCheck);
        tintImage.SetActive(false);
    }
    
    private void WinCheck()
    {
        deadPlayerCount++;
        
        if (deadPlayerCount >= players.Length - 1) // If only one player is left they have won
        {
            Debug.Log("Player wins!");
            foreach (var player in players)
            {
                if (player.GetCurrentHealth() > 0)
                {
                    tintImage.SetActive(true);
                    DoPanelPosition(winPanel, mainPosition);
                    EventSystem.current.SetSelectedGameObject(mainMenuButton);
                    player.GetComponentInParent<PlayerInput>().SwitchCurrentActionMap("UI");
                    playerWinText.text = "Player " + player.GetComponentInParent<InputManager>().playerID + " wins!";
                    return;
                }
            }
        }
    }

    public void ReplayMatch()
    {
        // Auto choose everything
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
}
