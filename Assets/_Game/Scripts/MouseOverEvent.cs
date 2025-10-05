using System;
using UnityEngine;
using UnityEngine.Events;

public class MouseOverEvent : MonoBehaviour
{
    public bool useRange = false;
    //[SerializeField] float range = 3;
    public UnityEvent onMouseEnter;
    public UnityEvent onMouseExit;
    bool onMouseOver = false;
    public void OnMouseEnter()
    {
        onMouseOver = true;
        if (useRange)
            if (PlayerMovement.Instance.InPlayerRange(transform.position)) onMouseEnter.Invoke();
        else
            onMouseEnter.Invoke();
    }

    public void OnMouseExit()
    {
        bool onMouseOver = false;
        onMouseExit.Invoke();
    }

    private void Update()
    {
        if (onMouseOver)
        {
            if (!PlayerMovement.Instance.InPlayerRange(transform.position))
            {
                OnMouseExit();
            }
        }
    }
}
