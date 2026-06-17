using System;
using UnityEngine;

public class HitDummy : MonoBehaviour, IHittable
{
    private SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void TakeHit(int damage, bool staggerAttack)
    {
        Debug.Log("OUCH! - Took " + damage + " damage");
        sprite.color = Color.red;
        Invoke(nameof(ResetColor), .15f);
    }

    private void ResetColor()
    {
        sprite.color = Color.white;
    }
}
