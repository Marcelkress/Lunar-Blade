using System;
using UnityEngine; 
using FMODUnity;
using FMOD;
using FMOD.Studio;
using UnityEngine.SceneManagement;
using Debug = FMOD.Debug;
using FMODUnityResonance;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    private static bool alreadyPlaying = false;
    
    [SerializeField] private EventReference mainMenuMusic;
    [SerializeField] private EventReference cavesMusic;
    [SerializeField] private EventReference forestMusic;
    [SerializeField] private EventReference BITMANMusic;
    
    private EventInstance musicInstance;
    private float MenuLayer;
    
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
        
        //musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        alreadyPlaying = true;
    }

    public void SceneLoaded(string sceneName)
    {
        switch (sceneName)
        {
            case "MainMenu":
                musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                UnityEngine.Debug.Log("Main menu music");
                musicInstance = RuntimeManager.CreateInstance(mainMenuMusic);
                musicInstance.start();
                break;
            case "Caves":
                musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                UnityEngine.Debug.Log("Caves music");
                musicInstance = RuntimeManager.CreateInstance(cavesMusic);
                musicInstance.start();
                break;
            case "Forest":
                musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                UnityEngine.Debug.Log("Forest music");
                musicInstance = RuntimeManager.CreateInstance(forestMusic);
                musicInstance.start();
                break;
            case "BITMAN":
                musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                UnityEngine.Debug.Log("BITMAN music");
                musicInstance = RuntimeManager.CreateInstance(BITMANMusic);
                musicInstance.start();
                break;
        }
    }
    
    public void SetMenuThemeLayer(int layer)
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Param_MenuMusicLayer", layer);
    }

    void OnDestroy()
    {
        musicInstance.release();
    }
}