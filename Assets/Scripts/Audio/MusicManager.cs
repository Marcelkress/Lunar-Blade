using System;
using UnityEngine; 
using FMODUnity;
using FMOD;
using FMOD.Studio;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    private static bool alreadyPlaying = false;

    //[EventRef]
    //[SerializeField] EventReference Track_01;
    //[SerializeField] EventInstance Track_01;
    
    [SerializeField] private EventReference mainMenuMusic;
    [SerializeField] private EventReference cavesMusic;
    [SerializeField] private EventReference forestMusic;
    
    private EventInstance musicInstance;

    private void Awake()
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
    }

    void Start()
    {
        if (alreadyPlaying)
            return;

        musicInstance = RuntimeManager.CreateInstance(mainMenuMusic);

        musicInstance.start();
        //AudioManager.instance.PlayOneShot(Track_01);

        alreadyPlaying = true;
    }

    void OnDestroy()
    {
        //Track_01.release();
        musicInstance.release();
    }
}