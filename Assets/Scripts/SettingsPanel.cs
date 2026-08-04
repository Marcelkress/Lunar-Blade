using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    private bool enableVsync;
    private bool fullscreenToggle;

    private float musicVolume, ambientVolume, characterVolume;
    
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
        musicSlider.value = musicVolume;
        ambientSlider.value = ambientVolume;
        characterSlider.value = characterVolume;
    }

    #region AudioSettings

    public void ChangeMusicVolume(float volume)
    {
        musicVolume = volume;
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