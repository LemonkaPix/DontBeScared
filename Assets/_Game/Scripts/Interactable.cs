using System;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public bool CanInteract = true;
    public UnityEvent onInteract;

    public void Interact()
    {
        if (CanInteract && PlayerMovement.Instance.InPlayerRange(transform.position))
            onInteract?.Invoke();
    }

    public void allowInteraction(bool state)
    {
        CanInteract = state;
    }
}
