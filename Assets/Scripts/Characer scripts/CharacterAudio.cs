using UnityEngine;

public class CharacterAudio : MonoBehaviour
{
    public AudioSource source;
    
    [Header("Attack audio")] public AudioClip attackOne;
    public AudioClip attackTwo, attackThree;
    
    void Start()
    {
        source = GetComponent<AudioSource>();
    }
    
    public void PlayAttackClip(int ID)
    {
        switch (ID)
        {
            case 1:
                source.PlayOneShot(attackOne);
                break;
            case 2:
                source.PlayOneShot(attackTwo);
                break;
            case 3:
                source.PlayOneShot(attackThree);
                break;
            default:
                source.PlayOneShot(attackOne);
                break;
        }
        
    }
    
}
