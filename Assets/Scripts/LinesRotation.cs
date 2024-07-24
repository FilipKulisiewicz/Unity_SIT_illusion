using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinesRotation : MonoBehaviour
{
    public enum LinesSet{ //probably should be also used in the StimulusSequnceManager
        roomLines = 0,
        ansLines = 1
    }

    [SerializeField] public GameObject RoomLineL, RoomLineR; 
    [SerializeField] public GameObject AnsLineL, AnsLineR;    
    private GameObject StimL, StimR;    
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
        currRotation = StimL.transform.localEulerAngles[1]; 
        if (currRotation > 180.0f){
            currRotation = currRotation - 360.0f;
        }
        if (rotationDirection == 1 && currRotation > rotationLimitUpper || rotationDirection == -1 && currRotation < rotationLimitLower){
            Debug.Log("Rotation limit reached ! angle: " + currRotation);
            rotationDirection = 0;
            return;
        }
        if(CamerasManager.instance.currCam != CamerasManager.CameraView.stimuli && linesSet == LinesSet.roomLines){
            rotationDirection = 0;
        }
        if(CamerasManager.instance.currCam != CamerasManager.CameraView.ansRoom && linesSet == LinesSet.ansLines){
            rotationDirection = 0;
        }
        else{
            StimL.transform.Rotate(Vector3.up * rotationSpeed * rotationDirection * Time.deltaTime);
            StimR.transform.Rotate(Vector3.up * rotationSpeed * -rotationDirection * Time.deltaTime);
        }
    }

    void rotationDirectionInput(float triggerRightValue, float triggerLeftValue){
        if(triggerRightValue > triggerThreshold){
            rotationDirection = -1;
        }
        else if(triggerLeftValue > triggerThreshold){
            rotationDirection = 1;
        }
        if (triggerRightValue < triggerThreshold && rotationDirection == -1 || triggerLeftValue < triggerThreshold && rotationDirection == 1){
            rotationDirection = 0;
        }
    }

    void nextScene(float newAngle){
        setAngle(newAngle);
    }

    void setAngle(float newAngle){
        if(StimL == null || StimR == null){
            Debug.LogWarning("LinesSet not chosen in the GUI, assigned the Room lines");
            setLinesSet(LinesSet.roomLines);
        }
        StimL.transform.localEulerAngles = new Vector3(0.0f,  newAngle, 0.0f);
        StimR.transform.localEulerAngles = new Vector3(0.0f, -newAngle, 0.0f);
    }

    float getAngle(){
        currRotation = StimL.transform.localEulerAngles[1]; 
        if (currRotation > 180.0f){
            currRotation = currRotation - 360.0f;
        }
        return currRotation;
    }

    void setLinesSet(LinesSet newlinesSet){
        linesSet = newlinesSet;
        if(newlinesSet == LinesSet.roomLines){
            StimL = RoomLineL;
            StimR = RoomLineR;
        }
        else if (newlinesSet == LinesSet.ansLines){
            StimL = AnsLineL;
            StimR = AnsLineR;
        }
    }
    
    void printAngle(){
        currRotation = StimL.transform.localEulerAngles[1]; 
        if (currRotation > 180.0f){
            currRotation = currRotation - 360.0f;
        }
        Debug.Log("Line stimuli final rotation : " + currRotation);
    }
}
