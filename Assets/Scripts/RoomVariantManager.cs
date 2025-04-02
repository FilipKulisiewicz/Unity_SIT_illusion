using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class RoomVariantManager : MonoBehaviour{
    public static RoomVariantManager instance;

    public enum RoomVariant{
        floor = 0,
        ceiling = 1,
        side = 2
    }

    [SerializeField] private GameObject[] projectionPlanesSet; // RoomVariant enum: floor: [0],  ceiling: [1], side: [2]
    private RoomVariant roomVariant = RoomVariant.floor;

    private void OnEnable(){
        StimulusSequenceManager.action_sceneChangeRoomVariant += setRoomVariant;
    }
    
    private void OnDisable(){
        StimulusSequenceManager.action_sceneChangeRoomVariant -= setRoomVariant;
    }

    private void Awake(){
        if(instance!=null){
            Debug.LogWarning("Multiple instances of RoomVariantManager");
        }
        instance = this;
    }

    public RoomVariant getRoomVariant(){
        return roomVariant; 
    }

    private void setRoomVariant(RoomVariant newRoomVariant){
        roomVariant = newRoomVariant;
        disableAllProjections();
        projectionPlanesSet[(int)roomVariant].SetActive(true);
    }

    private void disableAllProjections(){
        foreach(GameObject projection in projectionPlanesSet){
            projection.SetActive(false);
        }
    }
}
