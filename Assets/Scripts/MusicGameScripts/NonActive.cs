using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonActive : MonoBehaviour
{
    // Start is called before the first frame update
      //衝突したとき
    private void OnTriggerEnter(Collider col)
    {
        //オブジェクトを非アクティブにする
        this.gameObject.SetActive(false);
    }
}
