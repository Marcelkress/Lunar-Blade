using UnityEngine;

public class TESTSCENE : MonoBehaviour
{
    [Header("Characters")] public GameObject blade;
    public GameObject reaper, axe;

    public enum CHARACTERS
    {
        blade, reaper, axe
    }
    
    [Header("Test character")]
    public CHARACTERS character;
    
    void Start()
    {
        switch (character)
        {
            case CHARACTERS.blade:
                Instantiate(blade, transform.position, blade.transform.rotation);
                break;
            case CHARACTERS.reaper:
                Instantiate(reaper, transform.position, reaper.transform.rotation);
                break;
            case CHARACTERS.axe:
                Instantiate(axe, transform.position, axe.transform.rotation);
                break;
        }    
    }
}
