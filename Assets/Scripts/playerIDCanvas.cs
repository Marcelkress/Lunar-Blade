using DG.Tweening;
using TMPro;
using UnityEngine;

public class playerIDCanvas : MonoBehaviour
{
    public float yOffset, followSpeed;
    
    private TMP_Text idText;
    private Transform followTransform;
    
    public void Init(Transform _followTransform, int id)
    {
        followTransform = _followTransform;
        
        idText = GetComponentInChildren<TMP_Text>();
        idText.text = id.ToString();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.DOMove(followTransform.position + new Vector3(0, yOffset, 0), followSpeed);
    }
}
