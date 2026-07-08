using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class OrbChargerThing : MonoBehaviour, IHittable
{
    public float coolDownTime = 20;
    public int maxHits = 3;
    public Color hittableColor, coolDownColor;
    
    private bool canBeHit = true;
    private int hitCount = 0;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > .5f)
        {
            anim.SetBool("Hittable", canBeHit);
            
            if(canBeHit)
                spriteRenderer.DOColor(hittableColor, .3f);
        }
    }

    public bool TakeHit(int damage, bool staggerAttack, bool specialAttack)
    {
        if (canBeHit)
        {
            hitCount++;
            anim.SetTrigger("Hit");

            if (hitCount >= maxHits)
            {
                canBeHit = false;
                spriteRenderer.DOColor(coolDownColor, .3f);
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
