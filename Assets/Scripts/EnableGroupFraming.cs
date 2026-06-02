using Unity.Cinemachine;
using UnityEngine;

public class EnableGroupFraming : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<CinemachineGroupFraming>().enabled = true;
    }

}
