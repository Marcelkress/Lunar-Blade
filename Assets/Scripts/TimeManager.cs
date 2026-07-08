using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;

    public float timeScaleBrief, timeScaleLonger,  
        briefTimeScaleDuration, longerTimeScaleDuration;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

    public void SlowDownBrief()
    {
        StartCoroutine(SlowDown(timeScaleBrief, briefTimeScaleDuration));
    }

    public void SlowDownSpecialAttack()
    {
        StartCoroutine(SlowDown(timeScaleLonger, longerTimeScaleDuration));
    }

    private float current;

    private IEnumerator SlowDown(float targetTimeScale, float duration)
    {
        Time.timeScale = targetTimeScale;
        yield return new WaitForSeconds(duration);
        Time.timeScale = 1;
    }
    
}
