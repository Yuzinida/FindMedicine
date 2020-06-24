/*==============================================================================
Copyright 2017 Maxst, Inc. All Rights Reserved.
==============================================================================*/

using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine.Networking;

using maxstAR;

public class ImageTrackerSample : ARBehaviour
{
    private Dictionary<string, ImageTrackableBehaviour> imageTrackablesMap =
        new Dictionary<string, ImageTrackableBehaviour>();

    private CameraBackgroundBehaviour cameraBackgroundBehaviour = null;
    //public GameObject UItext;
    public GameObject canvasObj;
    public GameObject canvasObj1;
    public GameObject canvasObj2;
    public GameObject canvasObj3;
    public GameObject canvasObj4;
    public GameObject canvasObj5;
    public GameObject canvasObj6;
    public GameObject canvasObj7;
    public GameObject canvasObj8;
    public GameObject canvasObj9;

    public GameObject pannel;

    public GameObject pannel1;

    public GameObject pannel2;

    public GameObject pannel3;

    public GameObject pannel4;

    public GameObject pannel5;

    public GameObject pannel6;

    public GameObject pannel7;

    public GameObject pannel8;

    public GameObject pannel9;


    //public float time = 0;
    //public float offTime = 3;

    //public GameObject UItext2;

    void Awake()
    {
        Init();

        AndroidRuntimePermissions.Permission[] result = AndroidRuntimePermissions.RequestPermissions("android.permission.WRITE_EXTERNAL_STORAGE", "android.permission.CAMERA");
        if (result[0] == AndroidRuntimePermissions.Permission.Granted && result[1] == AndroidRuntimePermissions.Permission.Granted)
            Debug.Log("We have all the permissions!");
        else
            Debug.Log("Some permission(s) are not granted...");

        cameraBackgroundBehaviour = FindObjectOfType<CameraBackgroundBehaviour>();
        if (cameraBackgroundBehaviour == null)
        {
            Debug.LogError("Can't find CameraBackgroundBehaviour.");
            return;
        }
    }

    void Start()
    {

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        imageTrackablesMap.Clear();
        ImageTrackableBehaviour[] imageTrackables = FindObjectsOfType<ImageTrackableBehaviour>();
        foreach (var trackable in imageTrackables)
        {
            imageTrackablesMap.Add(trackable.TrackableName, trackable);
            Debug.Log("Trackable add: " + trackable.TrackableName);
        }

        TrackerManager.GetInstance().StartTracker(TrackerManager.TRACKER_TYPE_IMAGE);
        AddTrackerData();

        StartCamera();

        // For see through smart glass setting
        if (ConfigurationScriptableObject.GetInstance().WearableType == WearableCalibration.WearableType.OpticalSeeThrough)
        {
            WearableManager.GetInstance().GetDeviceController().SetStereoMode(true);

            CameraBackgroundBehaviour cameraBackground = FindObjectOfType<CameraBackgroundBehaviour>();
            cameraBackground.gameObject.SetActive(false);

            WearableManager.GetInstance().GetCalibration().CreateWearableEye(Camera.main.transform);

            // BT-300 screen is splited in half size, but R-7 screen is doubled.
            if (WearableManager.GetInstance().GetDeviceController().IsSideBySideType() == true)
            {
                // Do something here. For example resize gui to fit ratio
            }
        }
    }

    private void AddTrackerData()
    {
        foreach (var trackable in imageTrackablesMap)
        {
            if (trackable.Value.TrackerDataFileName.Length == 0)
            {
                continue;
            }

            if (trackable.Value.StorageType == StorageType.AbsolutePath)
            {
                TrackerManager.GetInstance().AddTrackerData(trackable.Value.TrackerDataFileName);
                TrackerManager.GetInstance().LoadTrackerData();
            }
            else if (trackable.Value.StorageType == StorageType.StreamingAssets)
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    StartCoroutine(MaxstARUtil.ExtractAssets(trackable.Value.TrackerDataFileName, (filePah) =>
                    {
                        TrackerManager.GetInstance().AddTrackerData(filePah, false);
                        TrackerManager.GetInstance().LoadTrackerData();
                    }));
                }
                else
                {
                    TrackerManager.GetInstance().AddTrackerData(Application.streamingAssetsPath + "/" + trackable.Value.TrackerDataFileName);
                    TrackerManager.GetInstance().LoadTrackerData();
                }
            }
        }
    }

    private void DisableAllTrackables()
    {
        foreach (var trackable in imageTrackablesMap)
        {
            trackable.Value.OnTrackFail();
        }
    }

    void Update()
    {

        DisableAllTrackables();

        TrackingState state = TrackerManager.GetInstance().UpdateTrackingState();

        if (state == null)
        {
            return;
        }

        cameraBackgroundBehaviour.UpdateCameraBackgroundImage(state);

        TrackingResult trackingResult = state.GetTrackingResult();

        for (int i = 0; i < trackingResult.GetCount(); i++)
        {
            Trackable trackable = trackingResult.GetTrackable(i);
            imageTrackablesMap[trackable.GetName()].OnTrackSuccess(
                trackable.GetId(), trackable.GetName(), trackable.GetPose());
            SetCanvas(trackable.GetName());
        }


        if (trackingResult.GetCount() == 0)

        {
            canvasObj.SetActive(false);
            canvasObj1.SetActive(false);
            canvasObj2.SetActive(false);
            canvasObj3.SetActive(false);
            canvasObj4.SetActive(false);
            canvasObj5.SetActive(false);
            canvasObj6.SetActive(false);
            canvasObj7.SetActive(false);
            canvasObj8.SetActive(false);
            canvasObj9.SetActive(false);
            pannel.SetActive(false);
            pannel1.SetActive(false);
            pannel2.SetActive(false);
            pannel3.SetActive(false);
            pannel4.SetActive(false);
            pannel5.SetActive(false);
            pannel6.SetActive(false);
            pannel7.SetActive(false);
            pannel8.SetActive(false);
            pannel9.SetActive(false);

            //UITextaaa.SetActive(false);
            //OffDisplay();
        }
    }

    private void SetCanvas(string trackName)
    {
        //UITextaaa.SetActive(true);
        //OnDisplay();
        switch (trackName)
        {
            case "BASIC0000": //당뇨병
                canvasObj.SetActive(true);
                canvasObj1.SetActive(false);
                canvasObj2.SetActive(false);
                canvasObj3.SetActive(false);
                canvasObj4.SetActive(false);
                canvasObj5.SetActive(false);
                canvasObj6.SetActive(false);
                canvasObj7.SetActive(false);
                canvasObj8.SetActive(false);
                canvasObj9.SetActive(false);

                if (input.gettouchdown){
                    pannel.SetActive(true);
                }
                break;
            case "BASIC020114": //타이레놀 80
                canvasObj.SetActive(false);
                canvasObj1.SetActive(true);
                canvasObj2.SetActive(false);
                canvasObj3.SetActive(false);
                canvasObj4.SetActive(false);
                canvasObj5.SetActive(false);
                canvasObj6.SetActive(false);
                canvasObj7.SetActive(false);
                canvasObj8.SetActive(false);
                canvasObj9.SetActive(false);
                if (input.gettouchdown){
                    pannel1.SetActive(true);
                }
                break;
            case "BASIC020145": //펜잘큐
                canvasObj.SetActive(false);
                canvasObj1.SetActive(false);
                canvasObj2.SetActive(true);
                canvasObj3.SetActive(false);
                canvasObj4.SetActive(false);
                canvasObj5.SetActive(false);
                canvasObj6.SetActive(false);
                canvasObj7.SetActive(false);
                canvasObj8.SetActive(false);
                canvasObj9.SetActive(false);
                if (input.gettouchdown){
                    pannel2.SetActive(true);
                }
                break;
            case "BASIC020300": //게보린
                canvasObj.SetActive(false);
                canvasObj1.SetActive(false);
                canvasObj2.SetActive(false);
                canvasObj3.SetActive(true);
                canvasObj4.SetActive(false);
                canvasObj5.SetActive(false);
                canvasObj6.SetActive(false);
                canvasObj7.SetActive(false);
                canvasObj8.SetActive(false);
                canvasObj9.SetActive(false);
                if (input.gettouchdown){
                    pannel3.SetActive(true);
                }
                break;
            case "BASIC020387": //메이킨큐
                canvasObj.SetActive(false);
                canvasObj1.SetActive(false);
                canvasObj2.SetActive(false);
                canvasObj3.SetActive(false);
                canvasObj4.SetActive(true);
                canvasObj5.SetActive(false);
                canvasObj6.SetActive(false);
                canvasObj7.SetActive(false);
                canvasObj8.SetActive(false);
                canvasObj9.SetActive(false);
                if (input.gettouchdown){
                    pannel4.SetActive(true);
                }
                break;
            case "BASIC0201142": //이지앤6 애니
                canvasObj.SetActive(false);
                canvasObj1.SetActive(false);
                canvasObj2.SetActive(false);
                canvasObj3.SetActive(false);
                canvasObj4.SetActive(false);
                canvasObj5.SetActive(true);
                canvasObj6.SetActive(false);
                canvasObj7.SetActive(false);
                canvasObj8.SetActive(false);
                canvasObj9.SetActive(false);
                if (input.gettouchdown){
                    pannel5.SetActive(true);
                }
                break;
            case "BASIC0201495": // 게보린 소프트
                canvasObj.SetActive(false);
                canvasObj1.SetActive(false);
                canvasObj2.SetActive(false);
                canvasObj3.SetActive(false);
                canvasObj4.SetActive(false);
                canvasObj5.SetActive(false);
                canvasObj6.SetActive(true);
                canvasObj7.SetActive(false);
                canvasObj8.SetActive(false);
                canvasObj9.SetActive(false);
                if (input.gettouchdown){
                    pannel6.SetActive(true);
                }
                break;
                canvasObj.SetActive(false);
                canvasObj1.SetActive(false);
                canvasObj2.SetActive(false);
                canvasObj3.SetActive(false);
                canvasObj4.SetActive(false);
                canvasObj5.SetActive(false);
                canvasObj6.SetActive(false);
                canvasObj7.SetActive(true); ;
                canvasObj8.SetActive(false);
                canvasObj9.SetActive(false);
                if (input.gettouchdown){
                    pannel7.SetActive(true);
                }
                break;
            case "BASIC02011234": // 인사돌
                canvasObj.SetActive(false);
                canvasObj1.SetActive(false);
                canvasObj2.SetActive(false);
                canvasObj3.SetActive(false);
                canvasObj4.SetActive(false);
                canvasObj5.SetActive(false);
                canvasObj6.SetActive(false);
                canvasObj7.SetActive(false);
                canvasObj8.SetActive(true);
                canvasObj9.SetActive(false);
                if (input.gettouchdown){
                    pannel8.SetActive(true);
                }
                break;
            case "BASIC02017841": //
                canvasObj.SetActive(false);
                canvasObj1.SetActive(false);
                canvasObj2.SetActive(false);
                canvasObj3.SetActive(false);
                canvasObj4.SetActive(false);
                canvasObj5.SetActive(false);
                canvasObj6.SetActive(false);
                canvasObj7.SetActive(false);
                canvasObj8.SetActive(false);
                canvasObj9.SetActive(true);
                if (input.gettouchdown){
                    pannel9.SetActive(true);
                }
                break;
        }
    }







    public void SetNormalMode()
    {
        TrackerManager.GetInstance().SetTrackingOption(TrackerManager.TrackingOption.NORMAL_TRACKING);
    }

    public void SetExtendedMode()
    {
        TrackerManager.GetInstance().SetTrackingOption(TrackerManager.TrackingOption.EXTEND_TRACKING);
    }

    public void SetMultiMode()
    {
        TrackerManager.GetInstance().SetTrackingOption(TrackerManager.TrackingOption.MULTI_TRACKING);
    }

    void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            TrackerManager.GetInstance().StopTracker();
            StopCamera();
        }
        else
        {
            StartCamera();
            TrackerManager.GetInstance().StartTracker(TrackerManager.TRACKER_TYPE_IMAGE);
        }
    }

    void OnDestroy()
    {
        imageTrackablesMap.Clear();
        TrackerManager.GetInstance().SetTrackingOption(TrackerManager.TrackingOption.NORMAL_TRACKING);
        TrackerManager.GetInstance().StopTracker();
        TrackerManager.GetInstance().DestroyTracker();
        StopCamera();
    }

    void OnDisplay()
    {
        UItext.SetActive(true);
    }
    void OffDisplay()
    {
        UItext.SetActive(false);
    }

}