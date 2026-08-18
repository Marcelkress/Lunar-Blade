using System;
using UnityEngine; 
using FMODUnity;
using FMOD;
using FMOD.Studio;
using UnityEngine.SceneManagement;
using Debug = FMOD.Debug;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    private static bool alreadyPlaying = false;
    
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
        
        // musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        alreadyPlaying = true;
    }

    public void SceneLoaded(string sceneName)
    {
        switch (sceneName)
        {
            case "MainMenu":
                UnityEngine.Debug.Log("Main menu music");
                // FMOD TING HER
                break;
            case "Caves":
                UnityEngine.Debug.Log("Caves music");
                // FMOD TING HER
                break;
            case "Forest":
                UnityEngine.Debug.Log("Forest music");
                //FMOD TING HER
                break;
        }
    }

    void OnDestroy()
    {
        musicInstance.release();
    }
}