using TMPro;
using UnityEngine;
using UnityEngine.UI;
using FMOD;
using FMOD.Studio;
using UnityEngine.Rendering;

public class SettingsPanel : MonoBehaviour
{
    private bool enableVsync;
    private bool fullscreenToggle;

    private float musicVolume = 10, ambientVolume = 10, characterVolume = 10;

    

    [Header("Display things")]
    public TMP_Text vSyncText;
    public TMP_Text fullscreenText;

    [Header("Audio things")] 
    public Slider musicSlider;
    public Slider ambientSlider, characterSlider;
    
    public void OnEnter()
    {
        vSyncText.text = QualitySettings.vSyncCount == 1 ? "On" : "Off";
        fullscreenText.text = Screen.fullScreen ? "On" : "Off";
        
        // ABSALON her skal vi hente FMOD volumer og sette slider til de rigtige values
        FMODUnity.RuntimeManager.StudioSystem.getParameterByName("Param_Volume_Music", out float VolMus);
        FMODUnity.RuntimeManager.StudioSystem.getParameterByName("Param_Volume_Char", out float VolChar);
        FMODUnity.RuntimeManager.StudioSystem.getParameterByName("Param_Volume_Ambience", out float VolAmb);
        
        musicSlider.value = VolMus + 10;
        characterSlider.value = VolChar + 10;
        ambientSlider.value = VolAmb + 10;
        UnityEngine.Debug.Log(VolMus);
    }
//rema 1000 skraber
    #region AudioSettings

    public void ChangeMusicVolume(float volume)
    {
        // SET FMOD VOL
        musicVolume = volume;
        //musicInstance.setParameterByName("Param_Volume_Music", volume);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Param_Volume_Music",volume);
        
        
    }

    public void ChangeAmbientVolume(float volume)
    {
        // SET FMOD VOL
        ambientVolume = volume;

        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Param_Volume_Ambience",volume);
    }

    public void ChangeCharacterVolume(float volume)
    {
        // SET FMOD VOL
        characterVolume = volume;

        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Param_Volume_Char",volume);
    }

    #endregion
    
    
    #region  DisplaySettings

    public void ToggleVsync()
    {
        if (enableVsync)
        {
            vSyncText.text = "On";
            QualitySettings.vSyncCount = 1;
            enableVsync = false;
        }
        else
        {
            vSyncText.text = "Off";
            QualitySettings.vSyncCount = 0;
            enableVsync = true;
        }
    }
    
    public void ToggleFullscreen()
    {
        if (fullscreenToggle)
        {
            fullscreenText.text = "On";
            Screen.fullScreen = true;
            fullscreenToggle = false;
        }
        else
        {
            fullscreenText.text = "Off";
            Screen.fullScreen = false;
            fullscreenToggle = true;
        }
    }
    
    public void SetResolution(int val)
    {
        switch (val)
        {
            case 0:
                Screen.SetResolution(1920, 1080, true);
                break;
            case 1:
                Screen.SetResolution(2560, 1440, true);
                break;
            case 2:
                Screen.SetResolution(3840, 2160, true);
                break;
        }
    }
    
    #endregion
}