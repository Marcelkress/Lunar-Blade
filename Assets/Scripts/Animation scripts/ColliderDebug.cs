using UnityEngine;

public class ColliderDebug : MonoBehaviour
{
    private Collider2D col;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(col.enabled);
    }
}
