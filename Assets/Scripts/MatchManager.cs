using System;
using TMPro;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    [Header("Settings")] 
    public float defaultMatchSeconds;


    [Header("UI Elements")] 
    public GameObject canvas;
    public TMP_Text matchTimeText;
    public GameObject timeButtons;

    [Header("References")] 
    public CharacterSelectionManager characterSelectionManager;
    
    
    private float matchTimeRemaining;
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
        matchTimeRemaining = defaultMatchSeconds;
        matchTimeText.enabled = false;
        startedMatch = false;
    }

    public void SetMatchTime(int _matchTime)
    {
        matchTimeRemaining = _matchTime;
        UpdateUI();
        matchTimeText.enabled = true;
        
        timeButtons.SetActive(false);
        characterSelectionManager.EnableChoosePlayers();
    }

    public void StartMatchCount()
    {
        startedMatch = true;
    }

    private float UITimer;

    void Update()
    {
        if(startedMatch)
            CountDown();
        
        UITimer += Time.deltaTime;
        if (UITimer >= 1)
        {
            UITimer = 0;
            UpdateUI();
        }
    }
    
    private void CountDown()
    {
        matchTimeRemaining -= Time.deltaTime;
        
        if (matchTimeRemaining <= 0)
        {
            matchTimeRemaining = 0;
            Debug.Log("Game Over");
        }
    }
    
    public int SecondsRemaining()
    {
        return Mathf.FloorToInt(matchTimeRemaining % 60);
    }

    public int MinutesRemaining()
    {
        return Mathf.FloorToInt(matchTimeRemaining / 60);
    }

    void UpdateUI()
    {
        matchTimeText.text = MinutesRemaining() + ":" + SecondsRemaining();
    }
}
