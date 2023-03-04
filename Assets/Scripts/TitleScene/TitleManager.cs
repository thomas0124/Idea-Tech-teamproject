using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private SoundDirector soundDirector;
    [Header("Titleシーンで流れるBGM")]
    [SerializeField] private AudioClip BGMclip;
    [Header("スタートボタンを押したときのSE")]
    [SerializeField] private AudioClip StartButtonSE;
    private void Start()
    {
        SoundDirector.instance.PlayBGM(BGMclip);
    }

    public void OnStartButton()
    {
        SoundDirector.instance.PlaySE(StartButtonSE);
    }

}
