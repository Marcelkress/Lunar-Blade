using UnityEngine; 
using FMODUnity;
using FMOD;
using FMOD.Studio;

public class MusicManager : MonoBehaviour
{
    //[EventRef]
    //[SerializeField] EventReference Track_01;
    //[SerializeField] EventInstance Track_01;
    public string musicEventPath = "event:/Music/Track_01";
    private EventInstance musicInstance;
    void Start()
    {
        musicInstance = RuntimeManager.CreateInstance(musicEventPath);

        musicInstance.start();
        //AudioManager.instance.PlayOneShot(Track_01);
    }

    void OnDestroy()
    {
        //Track_01.release();
        musicInstance.release();
    }
}