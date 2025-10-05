using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item", order = 1)]
public class ItemScriptable : ScriptableObject
{
    public string Name;
    public Sprite Icon;
    public Sprite HandIcon;
    public Material ItemMaterial;
    public GameObject Prefab;
}
