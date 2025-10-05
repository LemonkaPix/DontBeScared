using UnityEngine;
using UnityEngine.Events;

public class OnPlayerInteractionRange : MonoBehaviour
{
    public bool inRange = false;
    public UnityEvent onEnter;
    public UnityEvent onExit;

    public void OnRangeEnter()
    {
        inRange = true;
        onEnter.Invoke();
    }
    
    public void OnRangeExit()
    {
        inRange = false;
        onExit.Invoke();
    }
}
