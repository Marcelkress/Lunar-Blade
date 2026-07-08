using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    
    public float defaultSlowDownTimeScale, defaultSlowDownDuration;
    
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

    public void SlowDown()
    {
        StartCoroutine(SlowDown(defaultSlowDownTimeScale, defaultSlowDownDuration));
    }

    public void SlowDown(float duration)
    {
        StartCoroutine(SlowDown(defaultSlowDownTimeScale, duration));
    }

    private IEnumerator SlowDown(float targetTimeScale, float duration)
    {
        Time.timeScale = targetTimeScale;
        yield return new WaitForSeconds(duration);
        Time.timeScale = 1;
    }
    
}
