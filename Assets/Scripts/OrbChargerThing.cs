using System;
using System.Collections;
using UnityEngine;

public class OrbChargerThing : MonoBehaviour, IHittable
{
    public float coolDownTime = 20;
    public int maxHits = 3;
    private bool canBeHit = true;
    private int hitCount = 0;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > .5f)
        {
            anim.SetBool("Hittable", canBeHit);
        }
    }

    public bool TakeHit(int damage, bool staggerAttack)
    {
        if (canBeHit)
        {
            hitCount++;
            anim.SetTrigger("Hit");

            if (hitCount >= maxHits)
            {
                canBeHit = false;
                StartCoroutine(Reset());
            }
            
            return true;
        }

        return false;
    }

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(coolDownTime);
        canBeHit = true;
        hitCount = 0;
    }
    
    
}
