using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class CountdownPanel : MonoBehaviour
{
    [Header("References")] 
    public TMP_Text countdownText;
    public GameObject fightText;
    public GameObject tintImage;

    private float timer;
    private bool counting;
    
    private Coroutine countdownRoutine;

    private void Start()
    {
        countdownText.gameObject.SetActive(false);
        fightText.SetActive(false);
        tintImage.SetActive(false);
    }

    public void StartCountdown()
    {
        countdownText.gameObject.SetActive(true);
        tintImage.SetActive(true);

        countdownRoutine = StartCoroutine(CountDown());
    }

    public void StopCountdown()
    {
        countdownText.gameObject.SetActive(false);
        fightText.SetActive(false);
        tintImage.SetActive(false);
        
        if (countdownRoutine != null)
        {
            StopCoroutine(countdownRoutine);
            countdownRoutine = null;
        }
    }

    private IEnumerator CountDown()
    {
        countdownText.gameObject.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();

            switch (i)
            {
                case 0:
                    // 1
                    break;
                case 1:
                    // 2
                    break;
                case 3:
                    // 3
                    break;
            }
            
            yield return new WaitForSeconds(1f);
        }

        fightText.SetActive(true);
        countdownText.gameObject.SetActive(false);
        
        yield return new WaitForSeconds(1f);
        
        fightText.SetActive(false);
        tintImage.SetActive(false);
    }
}
