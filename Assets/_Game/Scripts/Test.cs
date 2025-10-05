using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UIElements;

public class Test : MonoBehaviour
{
    [SerializeField] private Item lighter;

    [Button]
    public void fogTest1() 
    {
        FogController.instance.ChangeFogDensity(0.4f, 1);
    }
    
    [Button]
    public void fogTest2() 
    {
        FogController.instance.ChangeFogDensity(FogController.OriginalDensity, 1);
    }

    [Button]
    public void vignetteTest1() 
    {
        VignetteController.instance.ChangeVignetteIntensity(0.6f, 1);
    }
    
    [Button]
    public void vignetteTest2() 
    {
        VignetteController.instance.ChangeVignetteIntensity(VignetteController.OriginalVignette, 1);

    }

    [Button]
    public void AddLighter()
    {
        // Inventory.instance.AddItemToInventory(lighter.itemData);
    }
    [Button]
    public void RemoveLighter()
    {
        Inventory.instance.RemoveItemFromInventory(Inventory.instance.HandItem);
    }

    public void test()
    {
        print("TEST");
    }
}
