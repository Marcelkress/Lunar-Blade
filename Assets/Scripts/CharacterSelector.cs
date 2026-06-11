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

    // Hooked up to PlayerInputManager.onPlayerJoined in the Inspector
    public void OnPlayerJoined(PlayerInput selectorInput)
    {
        int index = selectorInput.playerIndex;
        selectors[index] = selectorInput;
        
        
        // Give the selector UI a reference back to this manager
        var ui = selectorInput.GetComponent<PlayerSelector>();
        if (ui != null) ui.Init(index);
    }
    
    // Called by CharacterSelectorUI when a player confirms their pick
    public void OnCharacterSelect(int playerIndex,int characterIndex)
    {
        selections[playerIndex] = characterIndex;
        readyCount++;
        
        if (readyCount == 2)
            SpawnSelectedCharacters();
    }

    private void SpawnSelectedCharacters()
    {
        for (int i = 0; i < 2; i++)
        {
            InputDevice device = selectors[i].devices[0]; // capture the device
            Destroy(selectors[i].gameObject);              // remove the selector

            // Instantiate the chosen fighter and route the same device to it
            PlayerInput fighter = PlayerInput.Instantiate(
                characterPrefabs[selections[i]],
                playerIndex:    i,
                controlScheme:  null,           // auto-detect from device
                splitScreenIndex: -1,
                pairWithDevice: device          // <-- this is the key call
            );

            fighter.transform.position = spawnPoints[i].position;
        }
    }
}