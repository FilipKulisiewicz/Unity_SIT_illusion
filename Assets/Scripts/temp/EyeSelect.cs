using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeSelect : MonoBehaviour
{
    Camera MainCamera;
    private const float TimeBetweenSwitch = 1.0f;  // seconds
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        MainCamera = Camera.main;
        MainCamera.stereoTargetEye = StereoTargetEyeMask.Both;
    }

    // Update is called once per frame
    void Update(){
        if (timer > 0){
            timer -= Time.deltaTime;
        }
        if (timer <= 0){
            if(MainCamera.stereoTargetEye != StereoTargetEyeMask.Left){
                MainCamera.stereoTargetEye = StereoTargetEyeMask.Left;
            }
            else if(MainCamera.stereoTargetEye != StereoTargetEyeMask.Right){
                MainCamera.stereoTargetEye = StereoTargetEyeMask.Right;
            }
            timer = TimeBetweenSwitch;
        }
    }
}
