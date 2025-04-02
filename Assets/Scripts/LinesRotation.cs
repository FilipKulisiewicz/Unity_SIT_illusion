using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinesControl : MonoBehaviour
{
    public enum LinesSet{ //probably should be also used in the StimulusSequnceManager
        noLinesRotation = 0,
        stimulusLines = 1,
        ansLines = 2
    }

    [SerializeField] private GameObject StimulusLineL, StimulusLineR; 
    [SerializeField] private GameObject AnsLineL, AnsLineR;
    [SerializeField] private GameObject AnsLineStand;
    private GameObject RotatedLineL, RotatedLineR;    
    private LinesSet linesSet;
    
    float rotationSpeed = 2.0f;
    float rotationLimitUpper = 7.0f;
    float rotationLimitLower = -2.5f;
    private float currRotation = 0.0f;
    private int rotationDirection = 0;
    private float triggerThreshold = 0.8f;
    
    void OnEnable(){
        ControllersInput.action_triggersValueChange += rotationDirectionInput;
        StimulusSequenceManager.action_sceneChangeRotationLinesSet += setLinesSet;
        StimulusSequenceManager.action_sceneChangeLinesRot += nextScene;
        StimulusSequenceManager.func_requestAngleStimuli += getAngle;
    }

    void OnDestroy(){
        ControllersInput.action_triggersValueChange -= rotationDirectionInput;
        StimulusSequenceManager.action_sceneChangeRotationLinesSet -= setLinesSet;
        StimulusSequenceManager.action_sceneChangeLinesRot -= nextScene;
        StimulusSequenceManager.func_requestAngleStimuli -= getAngle; 
    }   

    void FixedUpdate(){
        if (rotationDirection == 0){
            return;
        }
        currRotation = RotatedLineL.transform.localEulerAngles[1]; 
        if (currRotation > 180.0f){
            currRotation = currRotation - 360.0f;
        }
        if (rotationDirection == 1 && currRotation > rotationLimitUpper || rotationDirection == -1 && currRotation < rotationLimitLower){
            Debug.Log("Rotation limit reached ! angle: " + currRotation);
            rotationDirection = 0;
            return;
        }
        
        if (CamerasManager.instance.currCam != CamerasManager.CameraView.stimuli && linesSet == LinesSet.stimulusLines){
            rotationDirection = 0;
        }
        if(CamerasManager.instance.currCam != CamerasManager.CameraView.ansRoom && linesSet == LinesSet.ansLines){
            rotationDirection = 0;
        }
        else{
            RotatedLineL.transform.Rotate(Vector3.up * rotationSpeed * rotationDirection * Time.deltaTime);
            RotatedLineR.transform.Rotate(Vector3.up * rotationSpeed * -rotationDirection * Time.deltaTime);
        }
    }

    void rotationDirectionInput(float triggerRightValue, float triggerLeftValue){
        if(triggerRightValue > triggerThreshold){
            rotationDirection = -1;
        }
        else if(triggerLeftValue > triggerThreshold){
            rotationDirection = 1;
        }
        if ((triggerRightValue < triggerThreshold && rotationDirection == -1 || triggerLeftValue < triggerThreshold && rotationDirection == 1) || linesSet == LinesSet.noLinesRotation)
        {
            rotationDirection = 0;
        }
    }

    void setLinesSet(LinesSet newlinesSet){
        linesSet = newlinesSet;
        if (newlinesSet == LinesSet.stimulusLines)
        {
            RotatedLineL = StimulusLineL;
            RotatedLineR = StimulusLineR;
            AnsLineStand.SetActive(false);
        }
        else if (newlinesSet == LinesSet.ansLines)
        {
            RotatedLineL = AnsLineL;
            RotatedLineR = AnsLineR;
        }
    }

    void setAngle(float newAngle){
        if (RotatedLineL == null || RotatedLineR == null){
            StimulusLineL.transform.localEulerAngles = new Vector3(0.0f, newAngle, 0.0f);
            StimulusLineR.transform.localEulerAngles = new Vector3(0.0f, -newAngle, 0.0f);
            setLinesSet(LinesSet.noLinesRotation);
        }
        else{
            RotatedLineL.transform.localEulerAngles = new Vector3(0.0f, newAngle, 0.0f);
            RotatedLineR.transform.localEulerAngles = new Vector3(0.0f, -newAngle, 0.0f);
        }
    }

    float getAngle(){
        if (RotatedLineL == null || RotatedLineR == null){
            return -9999999999.0f;
        }
        currRotation = RotatedLineL.transform.localEulerAngles[1]; 
        if (currRotation > 180.0f){
            currRotation = currRotation - 360.0f;
        }
        return currRotation;
    }
    void nextScene(float newAngle)
    {
        setAngle(newAngle);
    }

    void printAngle(){
        currRotation = RotatedLineL.transform.localEulerAngles[1]; 
        if (currRotation > 180.0f){
            currRotation = currRotation - 360.0f;
        }
        Debug.Log("Line stimuli final rotation : " + currRotation);
    }
}
