using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSelectionManager : MonoBehaviour
{
    public static CharacterSelectionManager Instance { get; private set; }
    
    [Header("Prefabs")]
    [SerializeField] private GameObject[] characterPrefabs; // your fighter prefabs
    //[SerializeField] private GameObject selectorPrefab;     // lightweight join/select prefab

    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;

    private PlayerInput[] selectors = new PlayerInput[2];
    private int[] selections = new int[] { -1, -1 };       // chosen character index per player
    private int readyCount = 0;

    private PlayerInputManager inputManager;

    void Awake()
    {
        Instance = this;
        inputManager = GetComponent<PlayerInputManager>();
        

        // Point PlayerInputManager at the selector prefab for now
        //inputManager.playerPrefab = selectorPrefab;
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
    
    // Called by CharacterSelectorUI when a player confirms their pick
    public void OnCharacterSelect(int playerIndex, int characterIndex)
    {
        // Ignore if this player already confirmed
        if (selections[playerIndex] != -1) 
            return;

        selections[playerIndex] = characterIndex;
        readyCount++;

        if (readyCount == 2)
            SpawnSelectedCharacters();
    }

    private void SpawnSelectedCharacters()
    {
        for (int i = 0; i < 2; i++)
        {
            if (selections[i] < 0 || selections[i] >= characterPrefabs.Length)
            {
                Debug.LogError($"Player {i} has invalid selection: {selections[i]}");
                return;
            }
        }

        for (int i = 0; i < 2; i++)
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
}