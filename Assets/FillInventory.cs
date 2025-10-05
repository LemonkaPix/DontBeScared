using Core.Web;
using System.Collections;
using UnityEngine;

public class FillInventory : MonoBehaviour
{
    [SerializeField] ItemObject flashlightItem;
    [SerializeField] ItemObject shotgun;
    public IEnumerator Start()
    {

        yield return new WaitForFixedUpdate();
        flashlightItem.PickUpIgnoreHand();
        yield return new WaitForFixedUpdate();
        shotgun.PickUp();

        yield return new WaitForSeconds(0.1f);
        MessageDisplayer.Instance.DisplayNextMessage();
    }

}
