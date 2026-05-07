using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    private Animator anim;
    private CharacterMovement movement;
    private InputManager inputManager;

    private bool isGrounded, moving;
    private float velocityY;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        movement = GetComponent<CharacterMovement>();
        inputManager = GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovementValues();
        SetMovementParams();
        AttackAnimations();
    }

    private void AttackAnimations()
    {
        if (inputManager.simpleAttackWasPressed)
        {
            anim.SetTrigger("Simple Attack");
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
