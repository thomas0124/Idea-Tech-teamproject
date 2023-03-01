using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesGenerator : MonoBehaviour
{

    [Header("ノーツの出現ポイント")]
    [SerializeField] private GameObject generatePoint;

    [Header("ノーツ")]
    [SerializeField] private GameObject notesPink;
    [SerializeField] private GameObject notesGreen;
    [SerializeField] private GameObject notesBlue;
    [SerializeField] private GameObject notesOrange;

    //ノーツを出現させるかを決めるbool値。trueで出現する。
    private bool isActive = false;
    //ノーツの生成間隔
    private float generateTime = 2.0f;

    private void Start()
    {
        //コルーチンスタート
        StartCoroutine(GetRondomNum());
    }

    /// <summary>
    /// ゲームがスタートしたときに行う処理
    /// </summary>
    public void OnStartButton()
    {
        isActive = true;
    }

    /// <summary>
    /// ゲームが終了したときに行う処理
    /// </summary>
    public void OnEndButton()
    {
        isActive = false;
    }

    /// <summary>
    /// 受け取った値0~4に応じて出現させるノーツを決める。4の場合は出現させない。
    /// </summary>
    /// <param name="num"></param>
    private void GenerateNotes(int num)
    {
        if(!(0 <= num && num <= 4))
        {
            Debug.LogError("0~4の値で指定してください");
            return;
        }

        switch(num)
        {
            case 0:
                Instantiate(notesOrange, generatePoint.transform.position, Quaternion.identity);
                break;
            case 1:
                Instantiate(notesBlue, generatePoint.transform.position, Quaternion.identity);
                break;
            case 2:
                Instantiate(notesGreen, generatePoint.transform.position, Quaternion.identity);
                break;
            case 3:
                Instantiate(notesPink, generatePoint.transform.position, Quaternion.identity);
                break;
            case 4:
                break; //StartButtonを押すまで何も生成しない。
        }

        return;
        //notesPink, roadPink.transform.position, Quaternion.identity, gameQuad.transform
    }

    /// <summary>
    /// generateTimeごとにノーツを生成するための値を決める。
    /// isActive変数がfalseの場合は生成しないことを示す値である4をセットする。
    /// </summary>
    /// <returns></returns>
    IEnumerator GetRondomNum()
    {
        if(generateTime <= 0)
        {
            Debug.LogError("ノーツの生成間隔が0秒以下です。正の値を指定してください。");
            yield break;
        }

        while(true)
        {
            yield return new WaitForSeconds(generateTime);
            int num;
            if(isActive)
            {
                num = Random.Range(0, 4);
            }
            else
            {
                num = 4;
            }
            
            GenerateNotes(num); //値に応じたノーツを生成
        }
    }
}
