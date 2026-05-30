using System;
using UnityEngine;

public class AudioAnimEvents : MonoBehaviour
{
    private CharacterAudio charAudio;

    private void Start()
    {
        charAudio = GetComponentInParent<CharacterAudio>();
    }

    public void PlayAttackAudio(int ID)
    {
        charAudio.PlayAttackClip(ID);
    }
    
}
