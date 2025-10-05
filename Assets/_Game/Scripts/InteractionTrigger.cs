using Unity.VisualScripting;
using UnityEngine;

public class InteractionTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // print(other.gameObject.name + " in my range");
        OnPlayerInteractionRange opir = other.gameObject.GetComponent<OnPlayerInteractionRange>();
        if (opir)
        {
            opir.OnRangeEnter();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        OnPlayerInteractionRange opir = other.gameObject.GetComponent<OnPlayerInteractionRange>();
        if (opir)
        {
            opir.OnRangeExit();
        }

    }
}
