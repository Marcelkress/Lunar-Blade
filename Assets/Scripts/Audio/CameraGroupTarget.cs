using Unity.Cinemachine;
using UnityEngine;

public class CameraGroupTarget : MonoBehaviour
{
    public CinemachineTargetGroup targetGroup;
    public CameraSettings cameraSettings;
    private CinemachineGroupFraming groupFrame;
    
    private int thisIndex;
    private float ogSize;
    
    void Awake()
    {
        groupFrame = FindAnyObjectByType<CinemachineGroupFraming>();
        targetGroup = FindAnyObjectByType<CinemachineTargetGroup>();

        targetGroup.AddMember(transform, cameraSettings.TargetWeight, cameraSettings.TargetRadius);

        thisIndex = -1;
        for (int i = 0; i < targetGroup.Targets.Count; i++)
        {
            if (targetGroup.Targets[i].Object == transform)
            {
                thisIndex = i;
                break;
            }
        }

        ogSize = groupFrame.OrthoSizeRange.x;
    }

    public void FocusOnTarget()
    {
        targetGroup.Targets[thisIndex].Weight = cameraSettings.focusTargetWeight;
        groupFrame.OrthoSizeRange = new Vector2(cameraSettings.focusCameraSize, groupFrame.OrthoSizeRange.y);
        
        CancelInvoke(nameof(UnFocusOnTarget));
        Invoke(nameof(UnFocusOnTarget), cameraSettings.focusDuration);
    }
    
    private void UnFocusOnTarget()
    {
        targetGroup.Targets[thisIndex].Weight = cameraSettings.TargetWeight;
        groupFrame.OrthoSizeRange = new Vector2(ogSize, groupFrame.OrthoSizeRange.y);
    }
    
}
