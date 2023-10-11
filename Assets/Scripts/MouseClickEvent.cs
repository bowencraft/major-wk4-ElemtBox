using UnityEngine;
using UnityEngine.Events;

public class MouseClickEvent : MonoBehaviour
{
    public UnityEvent FollowClickEvent;
    private void OnMouseDown()
    {
        FollowClickEvent.Invoke();
    }
}