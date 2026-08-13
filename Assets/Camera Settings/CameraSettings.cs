using UnityEngine;

[CreateAssetMenu(fileName = "CameraSettings", menuName = "Scriptable Objects/CameraSettings")]
public class CameraSettings : ScriptableObject
{
    [Header("General")] 
    public float TargetRadius = 3;
    public float TargetWeight = 1;

    [Header("Special attack focus")] 
    public float focusDuration = 0.8f;
    public float focusTargetWeight = 2;
    public float focusCameraSize = 4;
}