using TMPro;
using UnityEngine;

public class SettingsPanel : MonoBehaviour
{
    private bool enableVsync;
    private bool fullscreenToggle;
    
    public TMP_Text vSyncText;
    public TMP_Text fullscreenText;
    
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
}
