using NaughtyAttributes;
using UnityEngine;

public class ItemPaper : ItemObject
{

    private void Start()
    {
        PaperCollectionController.Instance.RegisterNewPaper(this);
    }

    [Button]
    public override void PickUp()
    {
        PaperCollectionController.Instance.CollectPaper(this);
    }
}