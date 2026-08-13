using System;
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

    [Header("Display things")]
    public TMP_Text vSyncText;
    public TMP_Text fullscreenText;

    [Header("Audio things")] 
    public Slider musicSlider;
    public Slider ambientSlider, characterSlider, UISlider;

    private void Start()
    {
        AudioValues.CreateInstance();
        ChangeMusicVolume(AudioValues.instance.musicVol);
        ChangeAmbientVolume(AudioValues.instance.ambienceVol);
        ChangeCharacterVolume(AudioValues.instance.CharVol);
    }

    public void OnEnter()
    {
        vSyncText.text = QualitySettings.vSyncCount == 1 ? "On" : "Off";
        fullscreenText.text = Screen.fullScreen ? "On" : "Off";

        musicSlider.value = AudioValues.instance.musicVol;
        characterSlider.value = AudioValues.instance.CharVol;
        ambientSlider.value = AudioValues.instance.ambienceVol;
        UISlider.value = AudioValues.instance.uiVol;
    }
    
    #region AudioSettings

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