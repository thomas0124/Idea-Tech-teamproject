using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ObjectChangeScale : MonoBehaviour
{
    private float scaleValue = 1.0f;
    private void Start()
    {
        string objName = this.gameObject.name;
        switch(objName)
        {
            case "OnpuText":
                scaleValue = 1.5f;
                break;
            case "Onpu2Text":
                scaleValue = 0.5f;
                break;
            default:
                Debug.LogError("想定されている名前でないオブジェクトがあります");
                break;
        }

        transform.DOScale(new Vector3(this.transform.localScale.x * scaleValue,
                                      this.transform.localScale.y * scaleValue,
                                      this.transform.localScale.z * scaleValue), 2.0f)
                                      .SetLoops(-1, LoopType.Yoyo)
                                      .SetLink(gameObject);
        
    }
    
}
