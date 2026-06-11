using System;
using UnityEngine;

public class CharacterSelectUI : MonoBehaviour
{
    public static CharacterSelectUI instance;

    private void Awake()
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
}
