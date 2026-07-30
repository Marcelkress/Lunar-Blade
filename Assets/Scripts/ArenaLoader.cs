using UnityEngine;

public class ArenaLoader : MonoBehaviour
{
    public float loadMapWaitTime;
    public GameObject cavesArena, forestArena, toBeMadeArena;
    
    public static ArenaLoader instance;

    public enum Arenas
    {
        Caves,
        Forest,
        ToBeMade
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
        
        cavesArena?.SetActive(false);
        forestArena?.SetActive(false);
        toBeMadeArena?.SetActive(false);
        
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
            case Arenas.ToBeMade:
                activeArena = toBeMadeArena;
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
        Destroy(activeArena);
        activeArena = null;
        arenaAlreadyLoaded = false;
    }
}
