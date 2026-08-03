using UnityEngine;

public class TESTSCENE : MonoBehaviour
{
    [Header("Characters")] public GameObject blade;
    public GameObject reaper, axe, twinblade;

    public enum CHARACTERS
    {
        blade, reaper, axe, twinblade
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
            case CHARACTERS.twinblade:
                Instantiate(twinblade, transform.position, twinblade.transform.rotation);
                break;
        }    
    }
}
