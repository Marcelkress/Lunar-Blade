using System;
using UnityEngine;

public class HitDummy : MonoBehaviour, IHittable
{
    private SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void TakeHit(int damage)
    {
        Debug.Log("OUCH! - Took " + damage + " damage");
        sprite.color = Color.red;
        Invoke(nameof(ResetColor), .3f);
    }

    private void ResetColor()
    {
        sprite.color = Color.white;
    }
}
