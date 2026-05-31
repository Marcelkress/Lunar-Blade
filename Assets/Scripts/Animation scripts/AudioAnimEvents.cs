using System;
using UnityEngine;
using FMODUnity;
using FMOD;

public class AudioAnimEvents : MonoBehaviour
{   
    [SerializeField] private EventReference playerJump;
    [SerializeField] private EventReference playerDash;
    [SerializeField] private EventReference playerRun;
    private CharacterMovement movement;
    private void Start()
    {
        movement = GetComponentInParent<CharacterMovement>();
        movement.jumpPerformed.AddListener(JumpSound);
        movement.doubleJumpPerformed.AddListener(DoubleJumpSound);
    }
    private void JumpSound()
    {
        AudioManager.instance.PlayOneShot(playerJump, this.transform.position);
    }
    private void DoubleJumpSound()
    {
        AudioManager.instance.PlayOneShot(playerJump, this.transform.position);
    }

    private void DashSound()
    {
        AudioManager.instance.PlayOneShot(playerDash, this.transform.position);
    }
     private void RunSound()
    {
        AudioManager.instance.PlayOneShot(playerRun, this.transform.position);
    }
}

