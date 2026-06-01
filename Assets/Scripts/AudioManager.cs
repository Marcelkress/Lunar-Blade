using FMOD.Studio;
using UnityEngine;
using FMODUnity;
using FMOD;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance {get; private set; }

    private void Awake()
    {
        
        if (instance != null)
        {
            UnityEngine.Debug.LogError("DER ER MERE END ÉN AUDIOMANAGER I SCENEN!!!");
        }
        instance = this;
    }
    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

}
