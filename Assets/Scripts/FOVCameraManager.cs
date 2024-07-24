using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class FOVCameraManager : MonoBehaviour
{
    public static FOVCameraManager instance;

    [SerializeField]
    private Camera[] cams;
    
    private void OnEnable(){
        StimulusSequenceManager.action_sceneChangeFOV += changeFOV;
    }
    
    private void OnDisable(){
        StimulusSequenceManager.action_sceneChangeFOV += changeFOV;
    }

    void Awake(){
        if(instance!=null){
            Debug.LogWarning("Multiple instances of FOVCameraManager");
        }
        instance = this;
    }

    void changeFOV(float newFOV){
        foreach (Camera cam in cams){
            cam.fieldOfView = newFOV;
        }
    }
}
