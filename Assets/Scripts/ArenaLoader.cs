using UnityEngine;

public class ArenaLoader : MonoBehaviour
{
    public float loadMapWaitTime;
    public GameObject cavesArena, forestArena, BITDOMAINArena;
    
    public static ArenaLoader instance;

    public enum Arenas
    {
        Caves,
        Forest,
        BITDOMAIN
    }

    private bool arenaAlreadyLoaded;
    private GameObject activeArena;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        DontDestroyOnLoad(gameObject);
        
        if (cavesArena == null) cavesArena?.SetActive(false);
        if (forestArena != null) forestArena?.SetActive(false);
        
        arenaAlreadyLoaded = false;
    }

    // Called from map menu button
    public void LoadArena(Arenas arena)
    {
        if (arenaAlreadyLoaded)
            return;
        
        switch (arena)
        {
            case Arenas.Caves:
                activeArena = cavesArena;
                break;
            case Arenas.Forest:
                activeArena = forestArena;
                break;
            case Arenas.BITDOMAIN:
                if (BITDOMAINArena == null)
                {
                    activeArena = cavesArena;
                }
                else
                {
                    activeArena = BITDOMAINArena;
                }
                break;
            default:
                activeArena = cavesArena;
                break;
        }
        arenaAlreadyLoaded = true;
        Invoke(nameof(ActivateArena), loadMapWaitTime);
    }

    private void ActivateArena()
    {   
        activeArena.SetActive(true);
    }
    
    // Called from win screen button
    public void UnLoadArena()
    {
        activeArena.SetActive(false);
        activeArena = null;
        arenaAlreadyLoaded = false;
    }
}
