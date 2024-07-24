using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class CamerasManager : MonoBehaviour{
    public static CamerasManager instance;
    public static event Action<float> action_changeToBinocularView;

    public enum EyeDominance{ //probably should be also used in the StimulusSequnceManager
        left = 0,
        right = 1
    }
    
    public enum CameraView{
        ansRoom = 0,
        stimuli = 1
    }

    [SerializeField]
    private GameObject[] camsRight, camsLeft; // CameraView enum: ansRoom: [0],  stim1: [1]
    [SerializeField]
    private GameObject camFakeR, camFakeL;
    public CameraView currCam = CameraView.ansRoom;
    private int eyeNum = 2;
    private EyeDominance dominantEye = EyeDominance.right;
    
    private void OnEnable(){
        ControllersInput.action_buttonLeftValueChange += changeCamButton;
        StimulusSequenceManager.action_sceneChangeEye += changeEyesNum;
    }
    
    private void OnDisable(){
        ControllersInput.action_buttonLeftValueChange -= changeCamButton;
        StimulusSequenceManager.action_sceneChangeEye -= changeEyesNum;
    }

    void Awake(){
        if(instance!=null){
            Debug.LogWarning("Multiple instances of CamerasManager");
        }
        instance = this;
        // Debug.LogWarning("Cams Length" + (camsRight.Length + camsLeft.Length));
    }
    
    void Start(){
        setCamera(CameraView.ansRoom);
    }

    void changeCamButton(float buttonValue){
        if(buttonValue > 0.9f){
            currCam = (CameraView)(((int)currCam + 1) % (camsRight.Length));
            setCamera(currCam);
        }
    }

    void changeEyesNum(int newEyeNum, EyeDominance newDominantEye){
        eyeNum = newEyeNum;
        dominantEye = newDominantEye;
        setCamera(CameraView.ansRoom);
    }

    void setCamera(CameraView camera){
        currCam = camera;
        disableAllCams();
        if(currCam == CameraView.stimuli){
            action_changeToBinocularView?.Invoke(0.0f);
        }
        if(eyeNum == 2 || currCam == CameraView.ansRoom){
            camsLeft[(int)currCam].SetActive(true);
            camsRight[(int)currCam].SetActive(true);
        }
        else if (eyeNum == 1 && currCam == CameraView.stimuli){
            if(dominantEye == EyeDominance.right){
                camsRight[(int)currCam].SetActive(true);
                camFakeL.SetActive(true);
            }
            else if(dominantEye == EyeDominance.left){
                camsLeft[(int)currCam].SetActive(true);
                camFakeR.SetActive(true);
            }
        }
    }

    void disableAllCams(){
        camFakeL.SetActive(false);
        camFakeR.SetActive(false);
        foreach(GameObject cam in camsLeft){
            cam.SetActive(false);
        }
        foreach(GameObject cam in camsRight){
            cam.SetActive(false);
        }
    }
}
