using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }

    [SerializeField] Image rightHand;
    [SerializeField] Image middleHold;

    [SerializeField] private float shotgunRecoil;

    Vector3 rightHandPos;
    Vector3 middleHoldPos;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        rightHandPos = rightHand.transform.localPosition;
        middleHoldPos = middleHold.transform.localPosition;
    }

    public void ChangeRightHandSprite(Sprite sprite)
    {
        if (rightHand.sprite != sprite)
        {
            rightHand.sprite = sprite;
        }
    }
    public void ChangeMiddleHoldSprite(Sprite sprite)
    {
        if (middleHold.sprite != sprite)
        {
            middleHold.sprite = sprite;
        }
    }
    public void RightHandVisible(bool state)
    {
        Debug.LogError($"Right Hand visible changed to: {state}");
        rightHand.enabled = state;
    }
    public void MiddleHoldVisible(bool state)
    {
        middleHold.enabled = state;
    }

    [Button]
    public void RightHandShowAnimation()
    {
        rightHand.rectTransform.localPosition -= Vector3.up * Screen.currentResolution.height / 2;
        LeanTween.moveLocal(rightHand.gameObject, rightHandPos, .25f).setEase(LeanTweenType.easeOutSine);
    }
    [Button]
    public void MiddleHoldShowAnimation()
    {
        middleHold.rectTransform.localPosition -= Vector3.up * Screen.currentResolution.height / 2;
        LeanTween.moveLocal(middleHold.gameObject, middleHoldPos, .25f).setEase(LeanTweenType.easeOutSine);
    }
    [Button]
    public void SwitchFromRightToMiddleHold()
    {
        LeanTween.moveLocal(rightHand.gameObject, rightHandPos - Vector3.up * Screen.currentResolution.height / 2, .25f).setEase(LeanTweenType.easeOutSine);
        middleHold.rectTransform.localPosition -= Vector3.up * Screen.currentResolution.height / 2;
        LeanTween.moveLocal(middleHold.gameObject, middleHoldPos, .25f).setEase(LeanTweenType.easeOutSine).setDelay(.25f);

    }
    [Button]
    public void SwitchFromMiddleToRightHand()
    {
        LeanTween.moveLocal(middleHold.gameObject, middleHoldPos - Vector3.up * Screen.currentResolution.height / 2, .25f).setEase(LeanTweenType.easeOutSine);
        rightHand.rectTransform.localPosition -= Vector3.up * Screen.currentResolution.height / 2;
        LeanTween.moveLocal(rightHand.gameObject, rightHandPos, .25f).setEase(LeanTweenType.easeOutSine).setDelay(.25f);
    }

    public void ShotgunRecoilHand()
    {
        var currentResolutionHeight = Screen.currentResolution.height / 100;
        LeanTween.moveLocal(rightHand.gameObject, rightHand.transform.localPosition + new Vector3(1, -1) * (currentResolutionHeight * shotgunRecoil), .1f).setEase(LeanTweenType.easeOutSine);
        LeanTween.moveLocal(rightHand.gameObject, rightHandPos, .8f).setEase(LeanTweenType.easeInOutSine).setDelay(.1f);

    }
    public void ShotgunRecoilMiddle()
    {
        var currentResolutionHeight = Screen.currentResolution.height / 100;
        LeanTween.moveLocal(middleHold.gameObject, middleHold.transform.localPosition + Vector3.down * (currentResolutionHeight * shotgunRecoil), .1f).setEase(LeanTweenType.easeOutSine);
        LeanTween.moveLocal(middleHold.gameObject, middleHoldPos, .8f).setEase(LeanTweenType.easeInOutSine).setDelay(.1f);

    }
}
