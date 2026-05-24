using UnityEngine;

public class AttackManager : MonoBehaviour
{
    private InputManager inputManager;
    private CharacterMovement movement;
    
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
        if (inputManager.attackOneWasPressed || inputManager.attackTwoWasPressed)
        {
            Attack();
        }
    }

    private void Attack()
    {
//        Debug.Log("Simple attack");
        //movement.LockMove(lockTime);
    }
}
