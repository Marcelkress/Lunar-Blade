using FMOD.Studio;
using UnityEngine;
using FMODUnity;
using FMOD;
using FMODUnityResonance;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance {get; private set; }
    
    [SerializeField] private EventReference UI_onButtonHovered;
    [SerializeField] private EventReference UI_onButtonClicked;
    [SerializeField] private EventReference UI_onButtonClickedFailure;
    [SerializeField] private EventReference UI_onButtonClickedSlider;
    [SerializeField] private FMOD.Studio.EventInstance musicEvent;

    private void Awake()
    {
        if (instance != null)
        {
            UnityEngine.Debug.LogError("DER ER MERE END ÉN AUDIOMANAGER I SCENEN!!!");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }
    
    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }
    
    public void PlayButtonOnHovered()
    {
        AudioManager.instance.PlayOneShot(UI_onButtonHovered, this.transform.position);
    }
    public void PlayButtonOnClicked()
    {
        AudioManager.instance.PlayOneShot(UI_onButtonClicked, this.transform.position);
    }
    public void playButtonOnClickedFailure()
    {
        AudioManager.instance.PlayOneShot(UI_onButtonClickedFailure, this.transform.position);
    }
    public void PlayButtonOnClickedSlider()
    {
        AudioManager.instance.PlayOneShot(UI_onButtonClickedSlider, this.transform.position);
    }

    public void MusicVolume()
    {
        
    }
}
