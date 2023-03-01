using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collsionmusic : MonoBehaviour
{
     //音データの再生装置を格納する
    private AudioSource audioSource;
    //音データを格納する
    [SerializeField]
    private AudioClip sound;
    private void Start()
    {
        //コンポーネントから再生装置を検出する
        audioSource = gameObject.AddComponent<AudioSource>();
    }
    //衝突したとき
    private void OnTriggerEnter(Collider col)
    {
        //"Player"タグがついているオブジェクトに衝突した場合
        if (col.gameObject.tag == "Player")
        {
            //音を鳴らす
            audioSource.PlayOneShot(sound);
            //0.2秒後にオブジェクトが消える
            Destroy(gameObject, 0.2f);
        }
    }
}
