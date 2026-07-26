using UnityEngine;

[CreateAssetMenu(fileName = "MatchSettings", menuName = "Scriptable Objects/MatchSettings")]
public class MatchSettings : ScriptableObject
{
    public int maxPlayerCount = 4;
    public int maxAllowedLives = 8;
    public int defaultMatchLives = 4;
}
