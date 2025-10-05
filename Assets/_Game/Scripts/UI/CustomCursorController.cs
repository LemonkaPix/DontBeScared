using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomCursorController : MonoBehaviour
{
    public TMP_InputField inputField;
    public RectTransform cursorTransform;
    public float blinkRate = 0.5f;

    private bool isBlinking = true;

    private void Start()
    {
        if (inputField == null) inputField = GetComponent<TMP_InputField>();
        if (cursorTransform == null) Debug.LogError("Brak przypisanego kursora!");

        inputField.customCaretColor = true;
        inputField.caretColor = new Color(0, 0, 0, 0);

        InvokeRepeating(nameof(BlinkCursor), blinkRate, blinkRate);
    }

    private void Update()
    {
        UpdateCursorPosition();
    }

    private void UpdateCursorPosition()
    {
        TMP_Text textComponent = inputField.textComponent;
        textComponent.ForceMeshUpdate();

        TMP_TextInfo textInfo = textComponent.textInfo;
        int lastIndex = inputField.text.Length - 1;

        if (lastIndex >= textInfo.characterCount)
            return;

        TMP_CharacterInfo charInfo = textInfo.characterInfo[Mathf.Max(lastIndex,0)];

        Vector3 charPosition = (charInfo.topRight + charInfo.bottomRight) / 2;

        charPosition.y = 5f;
        charPosition.x += 25;

        cursorTransform.position = textComponent.transform.TransformPoint(charPosition);
    }

    private void BlinkCursor()
    {
        isBlinking = !isBlinking;
        if (inputField.isFocused)
        {
            cursorTransform.gameObject.SetActive(isBlinking);
        } else
        {
            cursorTransform.gameObject.SetActive(false);
        }
    }
}