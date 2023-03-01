// <copyright file="GeospatialController.cs" company="Google LLC">
//
// Copyright 2022 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

namespace Google.XR.ARCoreExtensions.Samples.Geospatial
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.Android;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;
    using UnityEngine.XR.ARFoundation;
    using UnityEngine.XR.ARSubsystems;


    public class GeospatialController : MonoBehaviour
    {
        [Header("AR Components")]

        public ARSessionOrigin SessionOrigin;

        public ARSession Session;

        public ARAnchorManager AnchorManager;

        public ARRaycastManager RaycastManager;

        public AREarthManager EarthManager;

        public ARCoreExtensions ARCoreExtensions;

        public GamePlayManager gamePlayManager;


        [Header("UI Elements")]

        /// <summary>
        /// Help message shows while localizing.
        /// </summary>
        private const string _localizingMessage = "Localizing your device to set anchor.";

        /// <summary>
        /// Help message shows when location fails or hits timeout.
        /// </summary>
        private const string _localizationFailureMessage =
            "Localization not possible.\n" +
            "Close and open the app to restart the session.";

        /// <summary>
        /// Help message shows when location success.
        /// </summary>
        private const string _localizationSuccessMessage = "Localization completed.";

        /// <summary>
        /// Help message shows when resolving takes too long.
        /// </summary>
        private const string _resolvingTimeoutMessage =
            "Still resolving the terrain anchor.\n" +
            "Please make sure you're in an area that has VPS coverage.";

        /// <summary>
        /// The timeout period waiting for localization to be completed.
        /// </summary>
        private const float _timeoutSeconds = 180;

        /// <summary>
        /// Accuracy threshold for heading degree that can be treated as localization completed.
        /// </summary>
        private const double headingAccuracyThreshold = 25;

        /// <summary>
        /// Accuracy threshold for altitude and longitude that can be treated as localization
        /// completed.
        /// </summary>
        private const double horizontalAccuracyThreshold = 20;

        //ロケーションサービスの起動を待っているかを表すbool値
        private bool waitingForLocationService = false;
        //AR画面にいるかいないかを表すbool値
        private bool isInARView = false;
        private bool isReturning = false;
        private bool isLocalizing = false;
        //Geospatialが利用可能になったかどうかを表すbool値
        private bool enablingGeospatial = false;

        //private bool _usingTerrainAnchor = false;
        private float localizationPassedTime = 0f;
        private float configurePrepareTime = 3f;
        private List<GameObject> _anchorObjects = new List<GameObject>();
        /// <summary>
        /// StartLocationService()を管理するIEnumerator型の変数
        /// </summary>
        private IEnumerator startLocationService = null;

        /// <summary>
        /// AvailabilityCheck()を管理するIEnumerator型の変数
        /// </summary>
        private IEnumerator asyncCheck = null;

        //ロードしたデータを保存しておくリスト(データの型はGeospatialAnchorHistory)
        //private GeospatialAnchorHistoryCollection historyCollection = new GeospatialAnchorHistoryCollection();
        //保存時に新たにFirebaseに保存される緯度経度高度方位と画像
        //private GeospatialAnchorHistory newHistory;

        [SerializeField] private Text localizingStatusText;

        /// <summary>
        /// 起動後最初に1回だけ実行する
        /// </summary>
        public void Awake()
        {
            //スクリーンの向きを固定する
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = true;
            Screen.autorotateToPortraitUpsideDown = false;
            Screen.orientation = ScreenOrientation.AutoRotation;

            // Enable geospatial sample to target 60fps camera capture frame rate
            // on supported devices.
            // Note, Application.targetFrameRate is ignored when QualitySettings.vSyncCount != 0.
            Application.targetFrameRate = 60;

            if (SessionOrigin == null)
            {
                Debug.LogError("ARSessionOriginがnullです.");
            }

            if (Session == null)
            {
                Debug.LogError("ARSessionがnullです.");
            }

            if (ARCoreExtensions == null)
            {
                Debug.LogError("ARCoreExtensionsがnullです.");
            }

            localizingStatusText.text = "位置情報取得中...";

        }

        /// <summary>
        /// アタッチされたオブジェクトが有効になるたびに実行する。初回はAwake()より後に実行する。
        /// </summary>
        public void OnEnable()
        {
            //ロケーションサービスをスタート
            startLocationService = StartLocationService();
            StartCoroutine(startLocationService);

            isReturning = false;
            enablingGeospatial = false;
            localizationPassedTime = 0f;
            isLocalizing = true;

            SwitchToARView(true);
        }

        /// <summary>
        /// アタッチされたオブジェクトが無効になるたびに実行する。
        /// </summary>
        public void OnDisable()
        {
            //各機能の利用状況の確認を停止する
            StopCoroutine(asyncCheck);
            asyncCheck = null;
            //ロケーションサービスの起動を停止する
            StopCoroutine(startLocationService);
            startLocationService = null;

            Debug.Log("ロケーションサービスをストップします");
            Input.location.Stop();
        }

        public void Update()
        {
            //AR画面に移行していなければreturn
            if (!isInARView)
            {
                return;
            }

            //ARSessionのエラー状況を確認する
            LifecycleUpdate();

            if (isReturning)
            {
                return;
            }
            if (ARSession.state != ARSessionState.SessionInitializing &&
                ARSession.state != ARSessionState.SessionTracking)
            {
                return;
            }

            // 端末がGeospatialAPIの機能をサポートしているかどうかを調べる
            var featureSupport = EarthManager.IsGeospatialModeSupported(GeospatialMode.Enabled);
            switch (featureSupport)
            {
                case FeatureSupported.Unknown:
                    return;

                case FeatureSupported.Unsupported:
                    return;

                case FeatureSupported.Supported:
                    if (ARCoreExtensions.ARCoreExtensionsConfig.GeospatialMode == GeospatialMode.Disabled)
                    {
                        Debug.Log("GeospatialMode.Enabled に切り替わりました");
                        ARCoreExtensions.ARCoreExtensionsConfig.GeospatialMode = GeospatialMode.Enabled;
                        //新たな設定にかかる準備時間
                        configurePrepareTime = 3.0f;
                        //Geospatialが利用可能になったことを表すフラグ
                        enablingGeospatial = true;
                        return;
                    }

                    break;
            }

            // 新たな設定が有効になるのを待つ
            if (enablingGeospatial)
            {
                configurePrepareTime -= Time.deltaTime;
                if (configurePrepareTime < 0)
                {
                    //configurePrepareTimeに設定された時間が経過後に準備ができていなかったらfalse
                    enablingGeospatial = false;
                }
                else
                {
                    return;
                }
            }

            //AREarthManagerの状況を確認する
            var earthState = EarthManager.EarthState;
            if (earthState == EarthState.ErrorEarthNotReady)
            {
                Debug.Log("Geospatial 機能を初期化する");
                return;
            }
            else if (earthState != EarthState.Enabled)
            {
                Debug.LogWarning("EarthState エラーが発生しました");
                return;
            }

            // ローカライゼーションを確認する
            bool isSessionReady = (ARSession.state == ARSessionState.SessionTracking) && (Input.location.status == LocationServiceStatus.Running);

            var earthTrackingState = EarthManager.EarthTrackingState; 
            var pose = earthTrackingState == TrackingState.Tracking ? EarthManager.CameraGeospatialPose : new GeospatialPose();
            
            // Threshold:閾値 事前に設定した閾値以上では精度が低いためLost localizationとする
            // isSessionReadyとearthTrackingStateがそれぞれtrue, TrackingState.Trackingでなければ同様にLost localizationとする
            if (!isSessionReady || earthTrackingState != TrackingState.Tracking ||
                pose.HeadingAccuracy > headingAccuracyThreshold ||
                pose.HorizontalAccuracy > horizontalAccuracyThreshold)
            {
                // Lost localization
                //ローカライゼーションが完了していない、もしくは失ったとき
                if (!isLocalizing)
                {
                    isLocalizing = true;
                    localizationPassedTime = 0f;
                }

                if (localizationPassedTime > _timeoutSeconds)
                {
                    Debug.LogError("ローカライゼーションがタイムアウトしました");
                }
                else
                {
                    //ローカライゼーション中
                    localizationPassedTime += Time.deltaTime;
                }
            }
            else if (isLocalizing)
            {
                //ローカライゼーションが完了したとき
                isLocalizing = false;
                localizationPassedTime = 0f;

                //より精度の高い位置情報が取得出来たら画面に完了したことを伝える
                if(pose.HeadingAccuracy < 10 && pose.HorizontalAccuracy < 10)
                {
                    localizingStatusText.text = "完了";
                }

            }//EventSystem.current.IsPointerOverGameObject : uGUI操作中であればtrueになる。引数はスマホ等では必要
            else if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) && (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)))
            {
                //スクリーンをタップしたとき
                HandleScreenTap(Input.GetTouch(0).position);
            }

        }

        /// <summary>
        /// 位置情報が正確であるときに機能する。
        /// スクリーンをタップしたときにタップしたオブジェクトについているタグによって機能を分ける。
        /// </summary>
        /// <param name="position"></param>

        private void HandleScreenTap(Vector2 position)
        {
            //mainCameraのタグがついたオブジェクト(カメラ)からレイを飛ばす
            Ray ray = Camera.main.ScreenPointToRay(position);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                if(hit.collider == null) return;

                if(hit.collider.CompareTag("StartButton"))
                {
                    gamePlayManager.GameStart();
                    hit.collider.gameObject.SetActive(false);
                }
            }
            /*
            List<ARRaycastHit> hitResults = new List<ARRaycastHit>();
            RaycastManager.Raycast(position, hitResults, TrackableType.Planes | TrackableType.FeaturePoint);

            if (hitResults.Count > 0)
            {
                ARRaycastHit hit = hitResults[0];
                GeospatialPose geospatialPose = EarthManager.Convert(hitResults[0].pose);
                
                GeospatialAnchorHistory history = new GeospatialAnchorHistory(
                    geospatialPose.Latitude, geospatialPose.Longitude, geospatialPose.Altitude,
                    myPose.Heading); 

                var anchor = PlaceGeospatialAnchor(history, _usingTerrainAnchor);
                

            }
            */
            
        }

        /*
        private void PlaceGeospatialAnchor(GeospatialAnchorHistory history)
        {
            var anchor = AnchorManager.AddAnchor(history.Latitude, history.Longitude, history.Altitude, history.Heading);
                
            if (anchor != null)
            {
                GameObject PaintQuad = Instantiate(QuadPrefab, anchor.transform);
                PaintQuad.GetComponent<Renderer>().material.mainTexture = history.Texture;
                _anchorObjects.Add(anchor.gameObject);
                anchor.gameObject.SetActive(true);
                PaintQuad.gameObject.SetActive(true);

                //SnackBarText.text = $"{_anchorObjects.Count} Anchor(s) Set!";
            }
            else
            {
                SnackBarText.text = string.Format(
                    "Failed to set {0}!", "an anchor");
            }
        }
        */
        
        /// <summary>
        /// enable=true : ARの画面に移行 / enable=false : ARの画面を停止
        /// </summary>
        /// <param name="enable"></param>
        private void SwitchToARView(bool enable)
        {
            //AR画面に移行したかのフラグ
            isInARView = enable;

            SessionOrigin.gameObject.SetActive(enable);
            Session.gameObject.SetActive(enable);
            ARCoreExtensions.gameObject.SetActive(enable);

            //各機能の利用可能状況を確認する
            if (enable && asyncCheck == null)
            {
                asyncCheck = AvailabilityCheck();
                StartCoroutine(asyncCheck);
            }
        }

        /// <summary>
        /// ARSession.state/の利用可能状況を確認する
        /// </summary>
        /// <returns></returns>
        private IEnumerator AvailabilityCheck()
        {
            if (ARSession.state == ARSessionState.None)
            {
                yield return ARSession.CheckAvailability();
            }

            //ARSessionState.CheckingAvailabilityのために1フレーム停止させ、次のフレームから開始する
            yield return null;

            if (ARSession.state == ARSessionState.NeedsInstall)
            {
                yield return ARSession.Install();
            }

            //ARSessionState.Installingのために1フレーム停止させ、次のフレームから開始する
            yield return null;

#if UNITY_ANDROID
            //カメラを利用できるかを確認する
            if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
            {
                Debug.Log("カメラの利用許可をリクエスト");
                Permission.RequestUserPermission(Permission.Camera);
                yield return new WaitForSeconds(3.0f);
            }

            //カメラの許可のリクエストから3秒待ち、もう一度カメラが利用できるかを確認する
            if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
            {
                // User has denied the request.
                Debug.LogWarning(
                    "Failed to get camera permission. VPS availability check is not available.");
                yield break;
            }
#endif

            //ロケーションサービスの起動ができるまで待機
            while (waitingForLocationService)
            {
                yield return null;
            }

            if (Input.location.status != LocationServiceStatus.Running)
            {
                Debug.LogWarning(
                    "Location service is not running. VPS availability check is not available.");
                yield break;
            }

            // Update event is executed before coroutines so it checks the latest error states.
            if (isReturning)
            {
                yield break;
            }

            var location = Input.location.lastData;
            var vpsAvailabilityPromise =
                AREarthManager.CheckVpsAvailability(location.latitude, location.longitude);
            yield return vpsAvailabilityPromise;

            Debug.LogFormat("VPS Availability at ({0}, {1}): {2}", location.latitude, location.longitude, vpsAvailabilityPromise.Result);
            //VPSCheckCanvas.SetActive(vpsAvailabilityPromise.Result != VpsAvailability.Available);
        }

        /// <summary>
        /// ロケーションサービスを起動する
        /// </summary>
        /// <returns></returns>
        private IEnumerator StartLocationService()
        {
            //準備できるまでtrueにする
            waitingForLocationService = true;
#if UNITY_ANDROID
            //位置情報を利用できるかを確認する
            if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                Debug.Log("位置情報の利用をリクエスト");
                Permission.RequestUserPermission(Permission.FineLocation);
                yield return new WaitForSeconds(3.0f);
            }
#endif
            //位置情報の利用が拒否されたとき
            if (!Input.location.isEnabledByUser)
            {
                Debug.Log("ユーザーによって位置情報の利用が許可されませんでした");
                waitingForLocationService = false;
                yield break;
            }

            Debug.Log("ロケーションサービスをスタートします");
            Input.location.Start();

            while (Input.location.status == LocationServiceStatus.Initializing)
            {
                yield return null;
            }

            waitingForLocationService = false; //ロケーションサービスが起動したのでfalseに

            //ロケーションサービスが停止した時
            if (Input.location.status != LocationServiceStatus.Running)
            {
                Debug.LogWarningFormat(
                    "Location service ends with {0} status.", Input.location.status);
                Input.location.Stop();
            }

        }

        private void LifecycleUpdate()
        {
            //バックボタンをタップしたらアプリを終了させる
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                Application.Quit();
            }

            if (isReturning)
            {
                return;
            }

            //トラッキングをしないときのみ、画面をスリープさせる
            var sleepTimeout = SleepTimeout.NeverSleep;
            if (ARSession.state != ARSessionState.SessionTracking)
            {
                sleepTimeout = SleepTimeout.SystemSetting;
            }

            Screen.sleepTimeout = sleepTimeout;

            // ARSession がエラー状態の場合、アプリを終了する
            string returningReason = string.Empty;
            if (ARSession.state != ARSessionState.CheckingAvailability &&
                ARSession.state != ARSessionState.Ready &&
                ARSession.state != ARSessionState.SessionInitializing &&
                ARSession.state != ARSessionState.SessionTracking)
            {
                Debug.LogError("ARSession のエラー状態が発生しました。アプリを再起動してください。");
            }
            else if (Input.location.status == LocationServiceStatus.Failed)
            {
                Debug.LogError("ロケーションサービスの開始に失敗しました。アプリを再起動し、正確な位置情報を許可してください。");
            }
            else if (SessionOrigin == null || Session == null || ARCoreExtensions == null)
            {
                Debug.LogError("ARコンポーネントが見つかりません。");
            }
        }

    }
}
