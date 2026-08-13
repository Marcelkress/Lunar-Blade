using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CharacterSelectionManager : MonoBehaviour
{
    public static CharacterSelectionManager Instance { get; private set; }
    
    [Header("Prefabs")]
    [SerializeField] private GameObject[] characterPrefabs; // your fighter prefabs

    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Settings")] 
    private int playerCount;
    public int maxPlayerCount = 4;
    public float waitToSpawnTime;
    public MatchSettings matchSettings;

    [Header("UI")] 
    public TMP_Text playerCountDisplay;
    public GameObject pressToJoinText;

    public CountdownPanel countdownPanel;

    private PlayerInput[] selectors;
    private int[] selections; // = new int[] { -1, -1 };       // chosen character index per player
    private int readyCount = 0;
    private bool allPlayersReady;

    private int lifeCount;
    private PlayerInputManager playerInputManager;

    void Awake()
    {
        Instance = this;
        allPlayersReady = false;
        
        playerInputManager = GetComponent<PlayerInputManager>();
        playerInputManager.DisableJoining();
        
        playerCount = 2;
        playerCountDisplay.text = playerCount.ToString();

        // Point PlayerInputManager at the selector prefab for now
        //inputManager.playerPrefab = selectorPrefab;
    }

    public void IncreasePlayerCount()
    {
        playerCount++;
        if(playerCount > matchSettings.maxPlayerCount)
        {
            playerCount = matchSettings.maxPlayerCount;
        }
        playerCountDisplay.text = playerCount.ToString();
    }

    public void DecreasePlayerCount()
    {
        playerCount--;
        if(playerCount < 2)
        {
            playerCount = 2;
        }
        playerCountDisplay.text = playerCount.ToString();
    }

    public void SetLifeCount(int _lifeCount)
    {
        lifeCount = _lifeCount;
    }

    public void ChoosePlayerCount()
    {
        EventSystem.current.SetSelectedGameObject(null);
        //Debug.Log($"ChoosePlayerCount called, playerCount={playerCount}");
        playerInputManager.EnableJoining();
        pressToJoinText.SetActive(true);
        selections = new int[playerCount];
        Array.Fill(selections, -1);
        readyCount = 0;
        selectors = new PlayerInput[playerCount];
    }

    public void Reset()
    {
        allPlayersReady = false;
        readyCount = 0;
        
        selections = new int[playerCount];
        Array.Fill(selections, -1);
        playerInputManager = GetComponent<PlayerInputManager>();
        playerInputManager.DisableJoining();
    }

    public void OnPlayerJoined(PlayerInput selectorInput)
    {
        pressToJoinText.SetActive(false);
        int index = selectorInput.playerIndex;
        
        if (index < 0 || index >= selectors.Length)
        {
            Debug.LogError($"Unexpected player index: {index}");
            return;
        }

        // Don't overwrite if already joined (duplicate event guard)
        if (selectors[index] != null) 
            return;

        selectors[index] = selectorInput;

        var ui = selectorInput.GetComponent<PlayerSelector>();
        
        if (ui != null) 
            ui.Init(index);

        if (playerInputManager.playerCount > playerCount)
        {
            playerInputManager.DisableJoining();
        }
    }
    
    #region Character Selection Functions
    
    // Called by PlayerSelector when a player confirms their pick
    public void OnCharacterSelect(int playerIndex, int characterIndex)
    {
        if (selections[playerIndex] != -1) 
            return;

        selections[playerIndex] = characterIndex;
        readyCount++;

        // Debug.Log($"playerCount={playerCount}, selections=[{string.Join(",", selections)}]");
        
        if (AllSelected())
        {
            allPlayersReady = true;
            StartCoroutine(WaitToSpawn());
        }
    }

    private bool AllSelected()
    {
        for (int i = 0; i < playerCount; i++)
        {
            if (selections[i] == -1) return false;
        }
        return true;
    }
    
    public IEnumerator WaitToSpawn()
    {
        countdownPanel.StartCountdown();

        yield return new WaitForSeconds(waitToSpawnTime);

        if (allPlayersReady)
        {
            SpawnSelectedCharacters();
            playerInputManager.DisableJoining();
        } 
    }
    
    public void OnCharacterDeselected(int playerIndex)
    {
        selections[playerIndex] = -1;
        readyCount--;
        allPlayersReady = false;
        countdownPanel.StopCountdown();
    }

    private void SpawnSelectedCharacters()
    {
        for (int i = 0; i < playerCount; i++)
        {
            if (selections[i] < 0)
            {
                Debug.LogError($"Player {(i + 1)} has invalid selection: {selections[i]}");
                return;
            }
        }

        // Capture all devices BEFORE touching any selector
        InputDevice[] devices = new InputDevice[playerCount];
        for (int i = 0; i < playerCount; i++)
        {
            if (selectors[i] == null || selectors[i].devices.Count == 0)
            {
                Debug.LogError($"Player {i} selector has no paired device — aborting spawn.");
                return;
            }
            devices[i] = selectors[i].devices[0];
        }

        // Now destroy all selectors
        for (int i = 0; i < playerCount; i++)
        {
            Destroy(selectors[i].gameObject);
        }

        // Now instantiate all fighters using the captured devices
        for (int i = 0; i < playerCount; i++)
        {
            PlayerInput fighter = PlayerInput.Instantiate(
                characterPrefabs[selections[i]],
                playerIndex: i,
                controlScheme: null,
                splitScreenIndex: -1,
                pairWithDevice: devices[i]
            );

            fighter.transform.position = spawnPoints[i].position;
            PlayerHealth health = fighter.GetComponentInChildren<PlayerHealth>();
            health.Init(lifeCount);
            MatchManager.instance.StartingMatch(playerCount, health, i);
        }
    }
    
        #endregion
}