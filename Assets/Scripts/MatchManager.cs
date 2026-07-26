using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MatchManager : MonoBehaviour
{
    [Header("Settings")] 
    public int defaultMatchLives = 5;
    public int maxAllowedLives = 10;
    
    [Header("UI Elements")] 
    public GameObject lifecountCanvas;
    public GameObject winCanvas;
    public TMP_Text lifeCountDisplay;
    public TMP_Text playerWinText;
    public GameObject mainMenuButton;
    
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
        lifecountCanvas.SetActive(true);
        winCanvas.SetActive(false);
        lifeCount = matchSettings.defaultMatchLives;
        lifeCountDisplay.text = lifeCount.ToString();
        startedMatch = false;
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
        characterSelectionManager.EnableChoosePlayers(lifeCount);
        lifecountCanvas.SetActive(false);   
    }

    public void SetPlayerReferences(int playerCount, PlayerHealth player, int i)
    {
        if(players == null)
            players = new PlayerHealth[playerCount];
        
        players[i] = player;
        player.PermadeathEvent.AddListener(WinCheck);
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
                    winCanvas.SetActive(true);
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
