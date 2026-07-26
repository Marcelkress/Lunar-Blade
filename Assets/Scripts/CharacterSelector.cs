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
    public int playerCount = 2;
    public int maxPlayerCount = 4;
    public float waitToSpawnTime;
    public MatchSettings matchSettings;

    [Header("UI")] 
    public GameObject playerCountCanvas;
    public TMP_Text playerCountDisplay;
    public GameObject startingText;
    public GameObject firstSelectedUIButton;
    
    private PlayerInput[] selectors = new PlayerInput[2];
    private int[] selections; // = new int[] { -1, -1 };       // chosen character index per player
    private int readyCount = 0;
    private bool allPlayersReady;

    private bool allSpawned = false;
    private int lifeCount;
    private PlayerInputManager playerInputManager;

    void Awake()
    {
        allSpawned = false;
        Instance = this;
        allPlayersReady = false;
        
        selections = new int[playerCount];
        Array.Fill(selections, -1);
        playerInputManager = GetComponent<PlayerInputManager>();
        playerInputManager.DisableJoining();
        startingText.SetActive(false);

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

    public void EnableChoosePlayers(int _lifeCount)
    {
        lifeCount = _lifeCount;
        playerCountCanvas.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstSelectedUIButton);
    }

    public void ChoosePlayerCount()
    {
        playerInputManager.EnableJoining();
        playerCountCanvas.SetActive(false);
    }

    public void OnPlayerJoined(PlayerInput selectorInput)
    {
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
        // Ignore if this player already confirmed
        if (selections[playerIndex] != -1) 
            return;

        selections[playerIndex] = characterIndex;
        readyCount++;

        if (readyCount == playerCount)
        {
            allPlayersReady = true;
            startingText.SetActive(true);
            StartCoroutine(WaitToSpawn());
        }
    }

    private IEnumerator WaitToSpawn()
    {
        yield return new WaitForSeconds(waitToSpawnTime);

        if (allPlayersReady)
        {
            startingText.SetActive(false);
            SpawnSelectedCharacters();
            allSpawned = true;
            playerInputManager.DisableJoining();
        } 
    }

    public void OnCharacterDeselected(int playerIndex)
    {
        selections[playerIndex] = -1;
        readyCount--;
        allPlayersReady = false;
        startingText.SetActive(false);
    }

    private void SpawnSelectedCharacters()
    {
        for (int i = 0; i < playerCount; i++)
        {
            if (selections[i] < 0)
            {
                Debug.LogError($"Player {i} has invalid selection: {selections[i]}");
                return;
            }
        }

        for (int i = 0; i < playerCount; i++)
        {
            InputDevice device = selectors[i].devices[0];
            Destroy(selectors[i].gameObject);

            PlayerInput fighter = PlayerInput.Instantiate(
                characterPrefabs[selections[i]],
                playerIndex: i,
                controlScheme: null,
                splitScreenIndex: -1,
                pairWithDevice: device
            );

            fighter.transform.position = spawnPoints[i].position;
            PlayerHealth health = fighter.GetComponentInChildren<PlayerHealth>();
            health.Init(lifeCount);
            MatchManager.instance.SetPlayerReferences(playerCount, health, i);
        }
    }
    
        #endregion
}