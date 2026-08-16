using UnityEngine;
using UnityEngine.UI;

public class InMatchSettings : MonoBehaviour
{
    [Header("Sliders")] 
    public Slider musicSlider;
    public Slider ambientSlider, characterSlider, UISlider;
    
    public void OnEnter()
    {
        if (AudioValues.instance == null)
        {
            AudioValues.instance = new();
        }
        
        musicSlider.value = AudioValues.instance.musicVol;
        characterSlider.value = AudioValues.instance.CharVol;
        ambientSlider.value = AudioValues.instance.ambienceVol;
        UISlider.value = AudioValues.instance.uiVol;
    }
    
    public void ChangeMusicVolume(float volume)
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Param_Volume_Music", volume);
        AudioValues.instance.musicVol = volume;
    }

    public void ChangeAmbientVolume(float volume)
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Param_Volume_Ambience", volume);
        AudioValues.instance.ambienceVol = volume;
    }

    public void ChangeCharacterVolume(float volume)
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Param_Volume_Char", volume);
        AudioValues.instance.CharVol = volume;
    }
    
    public void ChangeUIVolume(float volume)
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Param_Volume_UI", volume);
        AudioValues.instance.uiVol = volume;
    }
}
