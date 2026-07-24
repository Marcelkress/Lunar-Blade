using System;
using TMPro;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    [Header("Settings")] public float defaultMatchLives = 5;

    [Header("UI Elements")] 
    public GameObject canvas;

    [Header("References")] 
    public CharacterSelectionManager characterSelectionManager;
    
    private bool startedMatch;
    
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
        canvas.SetActive(true);

        startedMatch = false;
    }

    public void SetMatchLives(int maxLives)
    {
        characterSelectionManager.EnableChoosePlayers(maxLives);
        canvas.SetActive(false);   
    }

}
