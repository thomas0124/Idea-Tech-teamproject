using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.XR.ARCoreExtensions;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class SoundController : MonoBehaviour
{
    
    [SerializeField] private ARAnchorManager arAnchorManager;
    [SerializeField] private AREarthManager arEarthManager;

    private AudioSource audioSource;
    private ARGeospatialAnchor anchor;
    private const double VERTICAL_THRESHOLD = 10;
    private const double HOLIZONTAL_THRESHOLD = 10;

    private void Start() 
    {
        audioSource = GetComponent<AudioSource>();
    }

    /*
    private void Update() 
    {
        //UnityEditorではAREarthManagerが動作しないのでスキップ
        if (Application.isEditor)
        {
            return;
        }
        
        //ARFoundationのトラッキング準備が完了するまで何もしない
        if (ARSession.state != ARSessionState.SessionTracking)
        {
            return;
        }
        
        if (!IsSupportedDevice())
        {
            return;
        }

        if (!IsHighAccuracyDeviceEarthPosition())
        {
            return;
        }
        
        if (IsExistGeoSpatialAnchor())
        {
            Debug.Log("Adjust position and rotation.");
            Adjust();
        }
    }

    /// <summary>
    /// 対応端末かチェック
    /// </summary>
    /// <returns>対応端末であればTrueを返す</returns>
    private bool IsSupportedDevice()
    {
        return arEarthManager.IsGeospatialModeSupported(GeospatialMode.Enabled) != FeatureSupported.Unsupported;
    }

    /// <summary>
    /// デバイスの位置精度をチェック
    /// </summary>
    /// <returns>閾値以上の位置精度であればTrueを返す</returns>
    private bool IsHighAccuracyDeviceEarthPosition()
    {
        //EarthTrackingStateが準備できていない場合
        if (arEarthManager.EarthTrackingState != TrackingState.Tracking) return false;

        //自身の端末の位置を取得し、精度が高い位置情報が取得できているか確認する
        var pose = arEarthManager.CameraGeospatialPose;
        var verticalAccuracy = pose.VerticalAccuracy;
        var horizontalAccuracy = pose.HorizontalAccuracy;

        //位置情報が安定していない場合
        if (verticalAccuracy > VERTICAL_THRESHOLD && horizontalAccuracy > HOLIZONTAL_THRESHOLD) return false;
        
        return true;
    }

    /// <summary>
    /// アンカーの存在を確認
    /// なければ追加
    /// </summary>
    private bool IsExistGeoSpatialAnchor()
    {
        //EarthTrackingStateの準備ができていない場合は処理しない
        if (arEarthManager.EarthTrackingState != TrackingState.Tracking)
        {
            return false;
        }

        if (anchor == null)
        {
            //GeoSpatialアンカーを作成
            //この瞬間に反映されるわけではなくARGeospatialAnchorのUpdate関数で毎フレーム補正がかかる
            var myPose = arEarthManager.CameraGeospatialPose;
            var offsetRotation = Quaternion.AngleAxis(180f - (float)myPose.Heading, Vector3.up);
            anchor = arAnchorManager.AddAnchor(myPose.Latitude, myPose.Longitude, myPose.Altitude, offsetRotation);
        }

        return anchor != null;
    }

    /// <summary>
    /// アンカーの位置にオブジェクトを出す
    /// </summary>
    private void Adjust()
    {
        this.transform.SetPositionAndRotation(anchor.transform.position,anchor.transform.rotation);
    }
    */

    public void MusicPlay()
    {
        audioSource.Play();
    }

    public void MusicStop()
    {
        audioSource.Stop();
    }
    
}
