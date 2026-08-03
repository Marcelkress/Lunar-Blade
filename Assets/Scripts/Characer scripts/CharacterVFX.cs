using UnityEngine;

public class CharacterVFX : MonoBehaviour
{
    public GameObject dustLandPrefab;
    public GameObject dustRunPrefab;
    public Transform dustSpawnPos;

    private CharacterMovement movement;

    void Start()
    {
        movement = GetComponent<CharacterMovement>();
        movement.beganRunEvent.AddListener(OnRun);
    }
    
    private void OnLand()
    {
        Instantiate(dustLandPrefab, dustSpawnPos.position, dustSpawnPos.rotation);
    }

    private void OnRun()
    {
        Instantiate(dustRunPrefab, dustSpawnPos.position, dustSpawnPos.rotation);
    }
}
