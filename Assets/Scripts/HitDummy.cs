using System;
using UnityEngine;

public class HitDummy : MonoBehaviour, IHittable
{
    private SpriteRenderer sprite;
    public bool debug;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public bool TakeHit(int damage, bool staggerAttack, bool specialAttack)
    {
        sprite.color = Color.red;
        Invoke(nameof(ResetColor), .15f);

        if (debug)
        {
            Debug.Log("Dummy hit - "  + damage + " damage");
        }
        
        return true;
    }

    private void ResetColor()
    {
        sprite.color = Color.white;
    }
}
