using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using Unity.Mathematics;
using UnityEngine.Rendering.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public GameObject HandItem;
    [SerializeField] public int maxItemCount = 3;
    [SerializeField] public List<GameObject> items;
    [SerializeField] private int currentItemId = 0;
    [SerializeField] private Transform inventoryParent;
    [SerializeField] private GameObject itemPrefab;
    //public Image handImage;
    public bool BlockChangingItem = false;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {

        // if (items.Count == 0)
        // {
        //handImage.enabled = false;
        // UIController.Instance.RightHandVisible(false);
        // }
        // else
        // {
        //     var obj = items[currentItemId];

        //     var item = obj.GetComponent<Item>();
        //     UIController.Instance.RightHandVisible(true);
        //     UIController.Instance.ChangeRightHandSprite(item.itemData.HandIcon);
        //     ChangeHandItem(0);
        // }
    }

    public void AddItemToInventory(GameObject item, [CanBeNull] Vector3 pos, bool no_switch = false)
    {
        if (items.Count < maxItemCount)
        {
            var obj = Instantiate(item, inventoryParent);
            items.Add(obj);
            currentItemId = items.Count - 1;
            if (!no_switch)
            {
                ChangeHandItem(currentItemId);
                //UIController.Instance.RightHandVisible(true);
            }
        }
        else
        {
            var nItem = Instantiate(itemPrefab, pos, quaternion.identity);
            // print("created new object");
            ItemObject io = nItem.GetComponent<ItemObject>();
            io.itemData = HandItem.GetComponent<Item>().itemData;
            io.RefreshSprite();

            RemoveItemFromInventory(HandItem);

            var obj = Instantiate(item, inventoryParent);
            items.Add(obj);
            currentItemId = items.Count - 1;
            ChangeHandItem(currentItemId);
            UIController.Instance.RightHandVisible(true);

        }
    }

    public void RemoveItemFromInventory(GameObject item)
    {
        items.Remove(item.gameObject);
        ChangeHandItem(math.clamp(currentItemId - 1, 0, items.Count - 1));
        Destroy(item.gameObject);
        if (items.Count == 0)
        {
            UIController.Instance.RightHandVisible(false);
        }
    }

    [Button]
    public void NextItem()
    {
        if (BlockChangingItem) return;
        if (currentItemId != items.Count - 1)
        {
            currentItemId++;
        }
        else
        {
            currentItemId = 0;
        }
        ChangeHandItem(currentItemId);
    }
    [Button]
    public void PreviusItem()
    {
        if (BlockChangingItem) return;
        if (currentItemId != 0)
        {
            currentItemId--;
        }
        else
        {
            currentItemId = items.Count - 1;
        }
        ChangeHandItem(currentItemId);
    }

    public void ChangeHandItem(int id)
    {
        if (HandItem)
            HandItem.gameObject.SetActive(false);
        HandItem = items[id];
        HandItem.gameObject.SetActive(true);
        currentItemId = id;
        Item item = HandItem.GetComponent<Item>();

        if (item is not Shotgun)
        {
            //flashlight
            UIController.Instance.MiddleHoldVisible(false);
            UIController.Instance.RightHandVisible(true);
            UIController.Instance.ChangeRightHandSprite(HandItem.GetComponent<Item>().itemData.HandIcon);
            UIController.Instance.RightHandShowAnimation();
        }
        else
        {
            //shotgun

            UIController.Instance.MiddleHoldVisible(true);
            UIController.Instance.RightHandVisible(false);
        }
    }

}
