using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlantRotAnsManager : MonoBehaviour{
    [SerializeField] private GameObject[] slantBoardsParentFrame; // in the RoomVariant Enum order - floor = 0,ceiling = 1,side = 2 
    [SerializeField] private GameObject[] slantBoardsPivot; //for rotation - floor = 0,ceiling = 1,side = 2 
    private GameObject currSlantBoardsPivot;
    
    private float rotationSpeed = 25.0f;
    private float rotationLimitUpper = 179.99f;
    private float rotationLimitLower = 0.01f;
    private Vector3 currRotation;
    private float currPitch = 0.0f;
    private int rotationDirection = 0;
    
    void OnEnable(){
        ControllersInput.action_joyStickRightValueChange += slantRotationDirectionInput;
        StimulusSequenceManager.action_sceneChangeRoomVariant += setRoomVariantSlant;
        StimulusSequenceManager.action_sceneChangeAnsSlantRot += nextScene;
        StimulusSequenceManager.func_requestAngleSlant += getAngle;
    }

    void OnDestroy(){
        ControllersInput.action_joyStickRightValueChange -= slantRotationDirectionInput;
        StimulusSequenceManager.action_sceneChangeRoomVariant -= setRoomVariantSlant;
        StimulusSequenceManager.action_sceneChangeAnsSlantRot -= nextScene;
        StimulusSequenceManager.func_requestAngleSlant -= getAngle;
    }   

    void FixedUpdate(){
        if (rotationDirection == 0){
            return;
        }
        currRotation = currSlantBoardsPivot.transform.localRotation.eulerAngles;
        currPitch = currRotation[0];
        if((int)currRotation[1] == 270 && (int)currRotation[2] == 180){
            currPitch = 180.0f - currPitch;
        }
        if (currPitch > 270.0f){
            currPitch = currPitch - 360.0f;
        }
        if (currPitch < -270.0f){
            currPitch = currPitch + 360.0f;
        }
        // Debug.Log("Slant board rotation : " + currPitch);        
        if (rotationDirection == 1 && currPitch > rotationLimitUpper || rotationDirection == -1 && currPitch < rotationLimitLower){
            Debug.Log("Rotation limit reached ! angle: " + currPitch);
            rotationDirection = 0;
            return;
        }
        else{
            float step = rotationSpeed * rotationDirection * Time.deltaTime;
            currSlantBoardsPivot.transform.localRotation = currSlantBoardsPivot.transform.localRotation * Quaternion.Euler(step, 0, 0);  
            //currSlantBoardsPivot.transform.Rotate(Vector3.right * rotationSpeed * rotationDirection * Time.deltaTime);
        }
    }

    void slantRotationDirectionInput(float joyStickValue){
        //Debug.Log("Joy rotation : " + joyStickValue);
        if(CamerasManager.instance.currCam != CamerasManager.CameraView.ansRoom){
            rotationDirection = 0;
        }
        else if(joyStickValue > 0.5f){
            rotationDirection = -1;
        }
        else if(joyStickValue < -0.5f) {
            rotationDirection = 1;
        }
        else{
            rotationDirection = 0;
        }
        if(RoomVariantManager.instance.getRoomVariant() == RoomVariantManager.RoomVariant.ceiling){
            rotationDirection = -rotationDirection;
        }
    }

    void nextScene(float newAngle){
        setAngle(newAngle);
    }

    void setAngle(float newAngle){
        currSlantBoardsPivot.transform.localEulerAngles = new Vector3(newAngle,90.0f,0.0f); //edits pitch
    }

    float getAngle(){
        return currSlantBoardsPivot.transform.localEulerAngles[0];
    }

    void printAngle(){
        currRotation = currSlantBoardsPivot.transform.localRotation.eulerAngles;
        currPitch = currRotation[0];
        if((int)currRotation[1] == 270 && (int)currRotation[2] == 180){
            currPitch = 180.0f - currPitch;
        }
        if (currPitch > 270.0f){
            currPitch = currPitch - 360.0f;
        }
        if (currPitch < -270.0f){
            currPitch = currPitch + 360.0f;
        }
        Debug.Log("Slant board final rotation : " + currPitch);
    }

    void setRoomVariantSlant(RoomVariantManager.RoomVariant roomVariant){
        disableAllSlants();
        slantBoardsParentFrame[(int)roomVariant].SetActive(true);
        currSlantBoardsPivot = slantBoardsPivot[(int)roomVariant];
    }

    void disableAllSlants(){
        foreach(GameObject board in slantBoardsParentFrame){
            board.SetActive(false);
        }
    }
}
