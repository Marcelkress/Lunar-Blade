using System;
using UnityEngine;

public class HitDummy : MonoBehaviour, IHittable
{
    private SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void TakeHit()
    {
        Debug.Log("OUCH!");
        sprite.color = Color.red;
        Invoke(nameof(ResetColor), .3f);
    }

    private void ResetColor()
    {
        sprite.color = Color.white;
    }
}
