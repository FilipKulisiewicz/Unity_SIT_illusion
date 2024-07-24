using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinesRoomRotManager : MonoBehaviour{
    [SerializeField] 
    private GameObject[] rooms;

    void OnEnable(){
        StimulusSequenceManager.action_sceneSetStimRoomRotation += setRoomRot;
    }

    void OnDestroy(){
        StimulusSequenceManager.action_sceneSetStimRoomRotation -= setRoomRot;
    }   
    
    void setRoomRot(float newAngle){
        foreach (GameObject room in rooms){
            room.transform.localEulerAngles = new Vector3(0,0,-newAngle);
        }
    }
}
