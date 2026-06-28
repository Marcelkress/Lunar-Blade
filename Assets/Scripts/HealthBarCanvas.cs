using System;
using UnityEngine;

public class HealthBarCanvas : MonoBehaviour
{
    public static HealthBarCanvas instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
