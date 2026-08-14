using System;
using UnityEngine;
using FMODUnity;
using FMOD;

public class AudioAnimEvents : MonoBehaviour
{   
    [SerializeField] private EventReference playerJump;
    [SerializeField] private EventReference playerDash;
    [SerializeField] private EventReference playerRun;
    [SerializeField] private EventReference playerDeflect;
    [SerializeField] private EventReference playerDeflectHit;
    [SerializeField] private EventReference playerTakeHit;
    [SerializeField] private EventReference playerRespawn;
    [SerializeField] private EventReference swordFinalDeath;
    [SerializeField] private EventReference axemanFinalDeath;
    [SerializeField] private EventReference reaperFinalDeath;
    [SerializeField] private EventReference twinbladeFinalDeath;
    [SerializeField] private EventReference swordDeath;
    [SerializeField] private EventReference axemanDeath;
    [SerializeField] private EventReference reaperDeath;
    [SerializeField] private EventReference twinbladeDeath;
    [SerializeField] private EventReference playerAttack_01;
    [SerializeField] private EventReference playerAttack_02;
    [SerializeField] private EventReference playerAttack_03;
    [SerializeField] private EventReference playerAttack_04;
    [SerializeField] private EventReference reaperLightAttack;
    [SerializeField] private EventReference reaperDoubleAttack;
    [SerializeField] private EventReference reaperSurpriseAttackEnter;
    [SerializeField] private EventReference reaperSurpriseAttackExit;
    [SerializeField] private EventReference reaperSpecialAttack;
    [SerializeField] private EventReference twinbladeLightAttack;
    [SerializeField] private EventReference twinbladeDoubleAttack;
    [SerializeField] private EventReference twinbladeSmokeAttackIn;
    [SerializeField] private EventReference twinbladeSmokeAttackOut;
    [SerializeField] private EventReference twinbladeSpecialAttack;
    [SerializeField] private EventReference axemanLightAttack;
    [SerializeField] private EventReference axemanMediumAttack;
    [SerializeField] private EventReference axemanDoubleAttack;
    [SerializeField] private EventReference axemanSpecialAttack;
    [SerializeField] private EventReference bitmanA;
    [SerializeField] private EventReference bitmanB;
    [SerializeField] private EventReference bitmanC;
    [SerializeField] private EventReference bitmanS;
  

    


    private CharacterMovement movement;
    private void Start()
    {
        movement = GetComponentInParent<CharacterMovement>();
        movement.jumpPerformedEvent.AddListener(JumpSound);
        movement.doubleJumpPerformedEvent.AddListener(DoubleJumpSound);
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
     private void RunSound()//playerDeflectHit;
    {
        AudioManager.instance.PlayOneShot(playerRun, this.transform.position);
    }
    public void PlayerTakeHit()//;;
    {
        AudioManager.instance.PlayOneShot(playerTakeHit, this.transform.position);
    }
    public void PlayerRespawn()//;;
    {
        AudioManager.instance.PlayOneShot(playerRespawn, this.transform.position);
    }
   
    public void SwordDeath()//;;
    {
        AudioManager.instance.PlayOneShot(swordDeath, this.transform.position);
    }
    public void AxemandDeath()//;;
    {
        AudioManager.instance.PlayOneShot(axemanDeath, this.transform.position);
    }
    public void ReaperDeath()//;;
    {
        AudioManager.instance.PlayOneShot(reaperDeath, this.transform.position);
    }
    public void TwinbladeDeath()//;;
    {
        AudioManager.instance.PlayOneShot(twinbladeDeath, this.transform.position);
    }
     public void SwordFinalDeath()//;;Final
    {
        AudioManager.instance.PlayOneShot(swordFinalDeath, this.transform.position);
    }
    public void AxemandFinalDeath()//;;
    {
        AudioManager.instance.PlayOneShot(axemanFinalDeath, this.transform.position);
    }
    public void ReaperFinalDeath()//;;
    {
        AudioManager.instance.PlayOneShot(reaperFinalDeath, this.transform.position);
    }
    public void TwinbladeFinalDeath()//;;
    {
        AudioManager.instance.PlayOneShot(twinbladeFinalDeath, this.transform.position);
    }
    public void PlayerDeflectHit()//;playerTakeHit;
    {
        AudioManager.instance.PlayOneShot(playerDeflectHit, this.transform.position);
    }
    public void PlayerDeflect()//;
    {
        AudioManager.instance.PlayOneShot(playerDeflect, this.transform.position);
    }
    private void BitmanA()
    {
        AudioManager.instance.PlayOneShot(bitmanA, this.transform.position);
    }
    private void BitmanB()
    {
        AudioManager.instance.PlayOneShot(bitmanB, this.transform.position);
    }
    private void BitmanC()
    {
        AudioManager.instance.PlayOneShot(bitmanC, this.transform.position);
    }
    private void BitmanS()
    {
        AudioManager.instance.PlayOneShot(bitmanS, this.transform.position);
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

    // REAPER
    private void ReaperLightAttack()
    {
        AudioManager.instance.PlayOneShot(reaperLightAttack, this.transform.position);
    }
    private void ReaperDoubleAttack()
    {
        AudioManager.instance.PlayOneShot(reaperDoubleAttack, this.transform.position);
    }
    private void ReaperSurpriseAttackEnter()
    {
        AudioManager.instance.PlayOneShot(reaperSurpriseAttackEnter, this.transform.position);
    }
    private void ReaperSurpriseAttackExit()
    {
        AudioManager.instance.PlayOneShot(reaperSurpriseAttackExit, this.transform.position);
    }
    private void ReaperSpecialAttack()
    {
        AudioManager.instance.PlayOneShot(reaperSpecialAttack, this.transform.position);
    }

    // TWINBLADE

     private void TwinbladeLightAttack()
    {
        AudioManager.instance.PlayOneShot(twinbladeLightAttack, this.transform.position);
    }
    private void TwinbladeDoubleAttack()
    {
        AudioManager.instance.PlayOneShot(twinbladeDoubleAttack, this.transform.position);
    }
    private void TwinbladeSmokeAttackIn()
    {
        AudioManager.instance.PlayOneShot(twinbladeSmokeAttackIn, this.transform.position);
    }
    private void TwinbladeSmokeAttackOut()
    {
        AudioManager.instance.PlayOneShot(twinbladeSmokeAttackOut, this.transform.position);
    }
    private void TwinbladeSpecialAttack()
    {
        AudioManager.instance.PlayOneShot(twinbladeSpecialAttack, this.transform.position);
    }

    //AXEMAN

    private void AxemanLightAttack()
    {
        AudioManager.instance.PlayOneShot(axemanLightAttack, this.transform.position);
    }
    private void AxemanMediumAttack()
    {
        AudioManager.instance.PlayOneShot(axemanMediumAttack, this.transform.position);
    }
    private void AxemanDoubleAttack()
    {
        AudioManager.instance.PlayOneShot(axemanDoubleAttack, this.transform.position);
    }
    private void AxemanSpecialAttack()
    {
        AudioManager.instance.PlayOneShot(axemanSpecialAttack, this.transform.position);
    }
}