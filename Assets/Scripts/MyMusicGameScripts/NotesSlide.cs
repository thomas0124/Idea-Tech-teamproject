using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class NotesSlide : MonoBehaviour
{
    [Header("出現場所。それぞれにOriginのタグをつける必要あり")]
    [SerializeField] private GameObject origin;
    [Header("移動先。それぞれにDestinaitonのタグをつける必要あり")]
    [SerializeField] private GameObject[] destinations = new GameObject[2];
    //ノーツの移動経路(0:出現場所, 1:中継地点, 2:目的地)
    private Vector3[] notesPath = new Vector3[3];
    //出現場所となるオブジェクト
    [SerializeField] private GameObject generatePos;

    private void Start()
    {
        generatePos = GameObject.Find("GenerationPoint");

        GameObject target = GameObject.Find("Origin");
        origin = target;

        GameObject[] targets = GameObject.FindGameObjectsWithTag("Destination");
        if(targets.Length > 2)
        {
            Debug.LogError("Destinationタグが付いたオブジェクトが" + targets.Length + "つ以上あります。2つ以下にしてください。");
            Destroy(this.gameObject);
        }
        else
        {
            destinations = targets;
        }

        //ノーツの移動経路を設定
        notesPath[0] = generatePos.transform.position;

        notesPath[1] = origin.transform.position;

        notesPath[2] = (Random.Range(0,2) == 0) ? destinations[0].transform.position : destinations[1].transform.position;

        //DOTWEENで移動させる。
        transform.DOPath(
            notesPath,
            7.0f
        );
        
    }

    private void OnTriggerEnter(Collider col)
    {
        //"resetbox"タグに衝突した場合
        if(col.gameObject.tag == "resetbox")
        {
            //オブジェクトが消える
            Destroy(this.gameObject);
        }
    }
}
