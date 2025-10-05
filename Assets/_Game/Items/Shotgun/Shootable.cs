using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Shootable : MonoBehaviour
{
    public UnityEvent onShoot;

    public void Shot()
    {
        if(RaycastChecker.IsPathClear(Camera.main.gameObject, gameObject))
            onShoot.Invoke();
    }
}
