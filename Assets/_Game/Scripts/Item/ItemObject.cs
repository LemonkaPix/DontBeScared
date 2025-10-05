using NaughtyAttributes;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] MeshRenderer[] highlightMeshRenderer;
    [SerializeField] public ItemScriptable itemData;

    private void Start()
    {
        RefreshSprite();
    }

    public void RefreshSprite()
    {
        if (meshRenderer)
            meshRenderer.material = itemData.ItemMaterial;
        // if(highlightMeshRenderer)
        foreach (var renderer in highlightMeshRenderer)
        {
            renderer.material = itemData.ItemMaterial;
            renderer.material.EnableKeyword("_EMISSION");

        }
    }

    [Button]
    public virtual void PickUp()
    {
        Inventory.instance.AddItemToInventory(itemData.Prefab, transform.position);
        Destroy(gameObject);
    }

    public virtual void PickUpIgnoreHand()
    {
        Inventory.instance.AddItemToInventory(itemData.Prefab, transform.position, true);
        Destroy(gameObject);
    }

    public void ReplaceItem(ItemScriptable item)
    {
        itemData = item;
        RefreshSprite();
    }
}