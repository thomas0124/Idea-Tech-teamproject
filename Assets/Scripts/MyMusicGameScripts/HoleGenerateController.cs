using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HoleGenerateController : MonoBehaviour
{
    [SerializeField] private GameObject notesGenerationHole;
    private AudioSource audioSourceSE;

    private void Start() 
    {
        audioSourceSE = notesGenerationHole.GetComponent<AudioSource>();    
    }
    
    public void GenerateHole()
    {
        Sequence sequence = DOTween.Sequence()
        .OnStart(() =>
        {
            notesGenerationHole.SetActive(true);
        })
        .Append(notesGenerationHole.transform.DOScale(new Vector3(0.1f, 1.0f, 0.5f), 1.0f)
        .SetEase(Ease.OutCubic))
        .InsertCallback(0.2f, () => audioSourceSE.PlayOneShot(audioSourceSE.clip));

        sequence.Play();
    }
}
