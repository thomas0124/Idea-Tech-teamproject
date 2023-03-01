using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    [SerializeField] private AudioClip SEClip;
    public void OnOKButton()
    {
        this.gameObject.SetActive(false);
        SoundDirector.instance.PlaySE(SEClip);
    }
}
