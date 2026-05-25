using System;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitDetection : MonoBehaviour
{
    private List<Collider2D> alreadyHit;

    private void Start()
    {
        alreadyHit = new List<Collider2D>();
    }
    

    private void OnTriggerStay2D(Collider2D otherCol)
    {
        if (otherCol.gameObject.TryGetComponent(out IHittable hit))
        {
            foreach (var alrHit in alreadyHit)
            {
                if (alrHit == otherCol)
                    return;
            }
                
            hit.TakeHit();
            alreadyHit.Add(otherCol);
        }
    }

    public void ClearHits()
    {
        alreadyHit.Clear();
    }
}

/// <summary>
/// Anything that can take hits from players
/// </summary>
public interface IHittable
{
    public void TakeHit();
}
