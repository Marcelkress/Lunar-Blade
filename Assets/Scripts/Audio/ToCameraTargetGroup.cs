using Unity.Cinemachine;
using UnityEngine;

public class ToCameraTargetGroup : MonoBehaviour


{
   [SerializeField] private CinemachineTargetGroup targetGroup;
    public CameraSettings cameraSettings;
    
    void Awake()
    {
        targetGroup = FindObjectOfType<CinemachineTargetGroup>();
        //Debug.Log(targetGroup);
        targetGroup.AddMember(this.gameObject.transform, cameraSettings.CharacterWeight, cameraSettings.CharacterRadius);
    }
}
