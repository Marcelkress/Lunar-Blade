using System;
using UnityEngine;
using FMODUnity;
using FMOD;

public class AudioAnimEvents : MonoBehaviour
{   
    [SerializeField] private EventReference playerJump;
    [SerializeField] private EventReference playerDash;
    [SerializeField] private EventReference playerRun;
    [SerializeField] private EventReference playerAttack_01;
    [SerializeField] private EventReference playerAttack_02;
    [SerializeField] private EventReference playerAttack_03;
    [SerializeField] private EventReference playerAttack_04;
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
    private void PlayerAttack_01()
    {
        AudioManager.instance.PlayOneShot(playerAttack_01, this.transform.position);
    }
    private void PlayerAttack_02()
    {
        AudioManager.instance.PlayOneShot(playerAttack_02, this.transform.position);
    }
    private void PlayerAttack_03()
    {
        AudioManager.instance.PlayOneShot(playerAttack_03, this.transform.position);
    }
    private void PlayerAttack_04()
    {
        AudioManager.instance.PlayOneShot(playerAttack_04, this.transform.position);
    }
}

