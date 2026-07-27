using TMPro;
using UnityEngine;

public class SettingsPanel : MonoBehaviour
{
    private bool enableVsync;
    public TMP_Text vSyncText;
    
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
    
    public void SetResolution()
    {
        
    }
}
