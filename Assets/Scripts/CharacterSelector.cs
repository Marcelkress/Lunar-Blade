using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    public GameObject[] characters;
    private GameObject currentCharacter;

    private GameObject spawnPoint;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnPoint = GameObject.Find("SpawnPoint");
    }

    public void ChooseCharacter(int characterIndex)
    {
        SpawnCharacter(characters[characterIndex]);
    }

    private void SpawnCharacter(GameObject character)
    {
        currentCharacter = character;
        Instantiate(character, spawnPoint.transform.position, spawnPoint.transform.rotation);
    }
}
