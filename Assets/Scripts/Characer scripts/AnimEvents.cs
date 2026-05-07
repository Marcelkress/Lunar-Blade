using UnityEngine;

public class AnimEvents : MonoBehaviour
{
    public void LockMove()
    {
        GetComponentInParent<CharacterMovement>().LockMove(false);
    }
    
    public void UnlockMove()
    {
        GetComponentInParent<CharacterMovement>().LockMove(true);
    }
}
