using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FadeDirector : MonoBehaviour
{
    public float fadetime;

    private CanvasGroup canvasGroup;
    public static FadeDirector instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start() 
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void FadeOut()
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.DOFade(1, fadetime).OnComplete(() => canvasGroup.blocksRaycasts = false);
    }

    private void FadeIn(float fadetime)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.DOFade(0, fadetime).OnComplete(() => canvasGroup.blocksRaycasts = false);
    }

    public void FadeOutToIn(TweenCallback action)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.DOFade(1, fadetime).OnComplete(() => {
            action();
            FadeIn(5.0f);
        });
    }

}
