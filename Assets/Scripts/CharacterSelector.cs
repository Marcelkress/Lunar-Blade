using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSelectionManager : MonoBehaviour
{
    public static CharacterSelectionManager Instance { get; private set; }
    
    [Header("Prefabs")]
    [SerializeField] private GameObject[] characterPrefabs; // your fighter prefabs

    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Player")] 
    public int playerCount;

    [Header("UI")] 
    public GameObject playerCountCanvas;
    
    private PlayerInput[] selectors = new PlayerInput[2];
    private int[] selections; // = new int[] { -1, -1 };       // chosen character index per player
    private int readyCount = 0;

    private bool allSpawned = false;
    private PlayerInputManager playerInputManager;

    void Awake()
    {
        allSpawned = false;
        Instance = this;
        
        selections = new int[playerCount];
        Array.Fill(selections, -1);
        playerInputManager = GetComponent<PlayerInputManager>();
        playerInputManager.DisableJoining();
        playerCountCanvas.SetActive(true);

        // Point PlayerInputManager at the selector prefab for now
        //inputManager.playerPrefab = selectorPrefab;
    }

    public void ChoosePlayerCount(int count)
    {
        playerCount = count;
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
            SpawnSelectedCharacters();
            allSpawned = true;
            playerInputManager.DisableJoining();
        }
    }

    public void OnCharacterDeselected(int playerIndex)
    {
        selections[playerIndex] = -1;
        readyCount--;
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
        }
    }
    
        #endregion
}