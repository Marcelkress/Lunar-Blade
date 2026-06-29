using System;
using UnityEngine;

public class HitDummy : MonoBehaviour, IHittable
{
    private SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public bool TakeHit(int damage, bool staggerAttack)
    {
        Debug.Log("OUCH! - Took " + damage + " damage");
        sprite.color = Color.red;
        Invoke(nameof(ResetColor), .15f);
        return true;
    }

    private void ResetColor()
    {
        sprite.color = Color.white;
    }
}
