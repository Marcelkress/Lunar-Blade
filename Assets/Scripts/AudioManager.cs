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
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }
    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }
 [SerializeField] private EventReference UI_onButtonHovered;
 [SerializeField] private EventReference UI_onButtonClicked;
 [SerializeField] private EventReference UI_onButtonClickedFailure;

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
}
