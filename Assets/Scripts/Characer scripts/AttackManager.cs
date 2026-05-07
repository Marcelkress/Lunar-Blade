using UnityEngine;

public class AttackManager : MonoBehaviour
{
    private InputManager inputManager;
    private CharacterMovement movement;
    public float lockTime;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputManager = GetComponent<InputManager>();
        movement = GetComponent<CharacterMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        AttackCheck();
    }

    private void AttackCheck()
    {
        if (inputManager.simpleAttackWasPressed)
        {
            Attack();
        }
    }

    private void Attack()
    {
        //movement.LockMove(lockTime);
    }
}
