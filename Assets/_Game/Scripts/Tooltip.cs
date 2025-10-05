using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class Tooltip : MonoBehaviour
{
    private TMP_Text tmptext;
    [SerializeField] private OnPlayerInteractionRange opir;


    private void Start()
    {
        tmptext = GetComponent<TMP_Text>();
    }

    public void Show(string text)
    {
        if (opir.inRange)
        {
            tmptext.text = text;
            tmptext.enabled = true;
        }
    }

    public void Hide()
    {
        tmptext.enabled = false;
    }
}