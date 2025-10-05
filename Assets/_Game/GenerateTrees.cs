using NaughtyAttributes;
using Unity.Mathematics;
using UnityEngine;
using UnityEditor;
using Random = UnityEngine.Random;

public class GenerateTrees : MonoBehaviour
{
    [SerializeField] private GameObject tree;
    [SerializeField] private Transform parent;
    [SerializeField] private int count = 100;
    [SerializeField][MinMaxSlider(0f, 2f)] private Vector2 scaleRange;

    [Button]
    public void generate()
    {
#if UNITY_EDITOR
        for (int i = 0; i < count; i++)
        {
            var pos = new Vector3(Random.Range(-50, 50), 0, Random.Range(-50, 50));
            GameObject instance = PrefabUtility.InstantiatePrefab(tree, parent) as GameObject;
            instance.transform.position = pos;
            // float scale = Random.Range(scaleRange.x, scaleRange.y);
            // instance.transform.localScale = new Vector3(scale, scale + ((1 - scale) / 2), 1);
        }
#endif
    }
}
