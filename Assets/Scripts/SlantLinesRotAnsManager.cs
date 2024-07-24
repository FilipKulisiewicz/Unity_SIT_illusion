using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlantLinesRotAnsManager : MonoBehaviour{
    [SerializeField] private GameObject ansSlantLinesRotPivot;
    private RoomVariantManager.RoomVariant roomVariant = RoomVariantManager.RoomVariant.floor; 
    
    private void OnEnable(){
        StimulusSequenceManager.action_sceneChangeRoomVariant += setRoomVariant;
    }
    
    private void OnDisable(){
        StimulusSequenceManager.action_sceneChangeRoomVariant -= setRoomVariant;
    }

    private void setRoomVariant(RoomVariantManager.RoomVariant newRoomVariant){
        roomVariant = newRoomVariant;
        if(roomVariant == RoomVariantManager.RoomVariant.floor){
            ansSlantLinesRotPivot.transform.localEulerAngles = new Vector3(0.0f ,0.0f,0.0f);
        }
        else if(roomVariant == RoomVariantManager.RoomVariant.ceiling){
            ansSlantLinesRotPivot.transform.localEulerAngles = new Vector3(180.0f ,0.0f,0.0f);
        }
        else if(roomVariant == RoomVariantManager.RoomVariant.side){
            ansSlantLinesRotPivot.transform.localEulerAngles = new Vector3(90.0f ,0.0f,0.0f);
        }
    }
}
