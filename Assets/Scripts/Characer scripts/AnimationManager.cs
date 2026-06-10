using UnityEngine;
using UnityEngine.Events;

public class AnimationManager : MonoBehaviour
{
    private Animator anim;
    private CharacterMovement movement;
    private InputManager inputManager;
    private PlayerHealth health;
    
    private bool isGrounded, moving;
    private float velocityY;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        movement = GetComponent<CharacterMovement>();
        inputManager = GetComponent<InputManager>();
        health = GetComponentInChildren<PlayerHealth>();
        health.TakeHitEvent.AddListener(TakeHitAnim);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovementValues();
        SetMovementParams();
        AttackAnimations();
        AttackBuffer();
        
        if (inputManager.dashWasPressed)
        {
            anim.SetTrigger("Dash");
        }
    }

    private void TakeHitAnim()
    {
        anim.SetTrigger("Take Hit");
    }

    private void AttackAnimations()
    {
        if (inputManager.attackOneWasPressed)
        {
            //Debug.Log("Attack 1");
            anim.SetTrigger("Enter Attack Tree");
            anim.SetTrigger("Attack One");
            count = true;
            timer = 0;
        }
        else if (inputManager.attackTwoWasPressed)
        {
            //Debug.Log("Attack 2");
            anim.SetTrigger("Enter Attack Tree"); 
            anim.SetTrigger("Attack Two");
            count = true;
            timer = 0;
        }
        else if (inputManager.attackThreeWasPressed)
        { 
            //Debug.Log("Attack 3");
            anim.SetTrigger("Enter Attack Tree"); 
            anim.SetTrigger("Attack Three");
            count = true;
            timer = 0;
        }
        else if (inputManager.specialAttackPressed && movement.isGrounded)
        { 
            //Debug.Log("Special attack");
            anim.SetTrigger("Enter Attack Tree"); 
            anim.SetTrigger("Attack Special");
            count = true;
            timer = 0;
        }
        else if (inputManager.attackOneWasReleased)
        {
            //anim.SetTrigger("Attack One Released");
        }
    }

    private bool count;
    private float timer;

    private void AttackBuffer()
    {
        if (count)
        {
            timer += Time.deltaTime;
            if (timer > movement.moveStats.attackInputBuffer)
            {
                anim.ResetTrigger("Attack One");
                anim.ResetTrigger("Attack Two");
                anim.ResetTrigger("Attack Three");
                anim.ResetTrigger("Enter Attack Tree");
                count = false;
            }
        }
    }

    private void SetMovementParams()
    {
        anim.SetBool("Moving", moving);
        anim.SetBool("Grounded", isGrounded);
        anim.SetFloat("VelocityY", velocityY);
    }

    void UpdateMovementValues()
    {
        isGrounded = movement.isGrounded;
        moving = Mathf.Abs(inputManager.moveVector.x) != 0;
        velocityY = movement.VerticalVelocity;
    }
    
}
