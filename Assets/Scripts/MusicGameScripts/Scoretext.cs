using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class Scoretext : MonoBehaviour
{
    // Start is called before the first frame update
    public float Score;
    public TextMeshProUGUI scorecounttext;
 
    //Scoretextにアクセスして実行する
    internal static void SetScoretext()
    {
        throw new NotImplementedException();
    }
 
    // Start is called before the first frame update
    void Start()
    {
        //0からスタートする
        Score = 0;
    }
 
    // Update is called once per frame
    void Update()
    {
        //textのフォーマットを設定する
        scorecounttext.text = string.Format("{0}", Score);
    }
}
