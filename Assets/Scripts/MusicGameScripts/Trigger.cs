using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    // Start is called before the first frame update
    public void PointerDown()
    {
        //オブジェクトをアクティブにする
        this.gameObject.SetActive(true);
    }
    public void PointerUp()
    {
        //オブジェクトを非アクティブにする
        this.gameObject.SetActive(false);
    }
}
