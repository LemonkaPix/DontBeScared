using System;
using UnityEngine;

//[RequireComponent(typeof(OnPlayerInteractionRange))]
public class Highlight : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] renderers;
    [SerializeField] private GameObject outline;
    [SerializeField] private float outlineWidth = 0.015625f;
    bool highlighted = false;

    //private OnPlayerInteractionRange opir;

    private void Start()
    {
        foreach (var renderer in renderers)
        {
            renderer.material.EnableKeyword("_EMISSION");
        }

        renderers[0].transform.position += Vector3.up * outlineWidth;
        renderers[1].transform.position += Vector3.down * outlineWidth;
        renderers[2].transform.position += Vector3.right * outlineWidth;
        renderers[3].transform.position += Vector3.left * outlineWidth;

        //opir = GetComponent<OnPlayerInteractionRange>();
    }

    public void HighlightObject(bool state)
    {
        //if (opir.inRange)
        if (PlayerMovement.Instance.InPlayerRange(transform.position))
        {
            outline.SetActive(state);
            highlighted = state;
        }

        if (!state)
        {
            outline.SetActive(state);
            highlighted = state;
        }
    }

    private void Update()
    {
        if (highlighted)
        {
            if (!PlayerMovement.Instance.InPlayerRange(transform.position))
            {
                print("off");
                HighlightObject(false);
            }
        }
    }
}