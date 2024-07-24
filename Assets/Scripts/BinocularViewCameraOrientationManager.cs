using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class BinocularViewCameraOrientationManager : MonoBehaviour
{
    public static BinocularViewCameraOrientationManager instance;

    [SerializeField]
    private GameObject trackedMovementCam;
    [SerializeField]
    private GameObject controlledMovementCam;
    private Vector3 currCamStartRot;
    private Vector3[] possibleCamStartRots =  new []{new Vector3(-18.4f,0.0f,0.0f), new Vector3(18.4f,0.0f,0.0f), new Vector3(0.0f,18.4f,0.0f)}; // startCaRotFloor/Ceiling/Side
    private Vector3 startReferenceRot, newReferenceRot, newRot;

    private Vector3 currScaleVector, scaleVectorUpDown = new Vector3(1.0f, 0.7f, 0.7f), scaleVectorSide = new Vector3(0.7f, 1.0f, 0.7f);

    private void OnEnable(){
        StimulusSequenceManager.action_sceneChangeRoomVariant += setStartRot;
        CamerasManager.action_changeToBinocularView += changeToBinocularView;
    }
    
    private void OnDisable(){
        StimulusSequenceManager.action_sceneChangeRoomVariant -= setStartRot;
        CamerasManager.action_changeToBinocularView -= changeToBinocularView;
    }

    void Awake(){
        if(instance!=null){
            Debug.LogWarning("Multiple instances of BinocularViewCameraOrientationManager");
        }
        instance = this;
    }

    void setStartRot(RoomVariantManager.RoomVariant roomVariant){
        currCamStartRot = possibleCamStartRots[(int)roomVariant];
        if(roomVariant == RoomVariantManager.RoomVariant.side){
            currScaleVector = scaleVectorSide;
        }
        else{
            currScaleVector = scaleVectorUpDown;
        }
    }

    void changeToBinocularView(float _temp){
        controlledMovementCam.transform.localEulerAngles = currCamStartRot;
        startReferenceRot = contrainVectorAngles(trackedMovementCam.transform.localEulerAngles);
        // Debug.LogWarning(startReferenceRot);
    }

    void Update(){
        //all operates in +-180 yaw (TODO: and +-90 Pitch and Yaw) ranges (used contrainVectorAngles)
        newReferenceRot = trackedMovementCam.transform.localEulerAngles;
        newRot = newReferenceRot - startReferenceRot;
        newRot = contrainVectorAngles(newRot);
        newRot = Vector3.Scale(newRot, currScaleVector);
        newRot = contrainVectorAngles(currCamStartRot + newRot);
        newRot = contrainVectorAngleYaw(newRot, -11.0f, 11.0f);
        // Debug.LogWarning(newRot);
        controlledMovementCam.transform.localEulerAngles = newRot;
    }

    Vector3 contrainVectorAngles(Vector3 inVec){
        for(int i = 0; i < 3; i ++){
            if (inVec[i] > 180.0f){
                inVec[i] = inVec[i] - 360.0f;
            }
            else if (newRot[i] < -180.0f){
                inVec[i] = inVec[i] + 360.0f;
            }
        }
        return inVec;
    }

    Vector3 contrainVectorAngleYaw(Vector3 inVec, float downLimit, float upLimit){
        if(inVec[1] > upLimit){
            inVec[1] = upLimit + ((inVec[1] - upLimit) / 5);
        } 
        else if(inVec[1] < downLimit){
            inVec[1] = downLimit - ((inVec[1] - downLimit) / 5);
        }
        return inVec;
    }
}

