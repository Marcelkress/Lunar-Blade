using Mono.Cecil.Cil;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraSettings", menuName = "Scriptable Objects/CameraSettings")]
public class CameraSettings : ScriptableObject
{
    public float CharacterRadius, CharacterWeight;
    
}
