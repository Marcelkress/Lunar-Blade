using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonEvents : MonoBehaviour,  ISelectHandler
{
    public UnityEvent OnSelected;
    
    public void OnSelect(BaseEventData eventData)
    {
        OnSelected.Invoke();
    }
}
