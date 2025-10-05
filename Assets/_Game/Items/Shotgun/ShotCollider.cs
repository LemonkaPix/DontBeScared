using System.Collections;
using UnityEngine;

public class ShotCollider : MonoBehaviour
{
    BoxCollider collider;
    private void Start()
    {
        collider = GetComponent<BoxCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        //print(other.name);

        Shootable target = other.GetComponent<Shootable>();

        if (target)
        {
            target.Shot();
        }
    }

    public void Shot()
    {
        StartCoroutine(waiter());
    }

    IEnumerator waiter()
    {
        collider.enabled = true;
        yield return new WaitForSeconds(0.1f);
        collider.enabled = false;
    }
}
