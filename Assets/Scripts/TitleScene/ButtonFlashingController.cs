using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ButtonFlashingController : MonoBehaviour
{
    public float durationSeconds;

    private CanvasGroup canvasGroup;
    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.DOFade(0f, durationSeconds).SetEase(Ease.InQuart).SetLoops(-1, LoopType.Yoyo);
    }
}
