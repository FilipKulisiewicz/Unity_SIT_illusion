using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public static Menu instance;
    
    public static event Action<int> action_startLog;
    public string participantID = "";
    [SerializeField]
    public string pathToCSVVariantFile;

    private Rect windowRect = new Rect(20, 20, 450, 850);
    public int selectedEyeOption = 0, selectedLinesSetOption = 0,  selectedRoomVariantOption = 01;
    private string[] eyeOptionNames = new string[]{"LeftEye", "RightEye"};
    private string[] linesSetOptionNames = new string[]{"RoomLines", "AnsweringLines"};
    private string[] roomVariantOptionNames = new string[]{"Floor", "Ceiling", "Side"};

    [SerializeField]
    public GameObject tutorialCamera;
    // public string basePath;

    void Awake(){
        if(instance!=null){
            Debug.LogWarning("Multiple instances of Menu");
        }
        instance = this;
    }

    public string getParticipantID(){
        return participantID;
    }

    private void OnGUI(){
        windowRect = GUI.Window(0, windowRect, windowFunction, "Controls");
    }

    void windowFunction(int windowID){
        GUI.Label(new Rect(5, 5, 240, 20), "Participant ID:");
        participantID = GUI.TextField(new Rect(5, 35, 440, 35), participantID);
        GUI.Label(new Rect(5, 75, 240, 20), "Dominant eye:"); 
        selectedEyeOption = GUI.SelectionGrid(new Rect(5, 100, 440, 195), selectedEyeOption, eyeOptionNames, 2);
        GUI.Label(new Rect(5, 300, 240, 20), "Lines set to be rotated:"); 
        selectedLinesSetOption = GUI.SelectionGrid(new Rect(5, 320, 440, 195), selectedLinesSetOption, linesSetOptionNames, 2);
        GUI.Label(new Rect(5, 520, 240, 20), "Lines set to be rotated:"); 
        selectedRoomVariantOption = GUI.SelectionGrid(new Rect(5, 550, 440, 195), selectedRoomVariantOption, roomVariantOptionNames, 3);
        //CSV:
        pathToCSVVariantFile = GUI.TextField(new Rect(5, 750, 440, 45), pathToCSVVariantFile); 
        if (GUI.Button(new Rect(5, 800, 440, 40), "Start Tracking")){
            action_startLog?.Invoke(0);
            Debug.Log("Selected Eye option " + eyeOptionNames[selectedEyeOption] + " ("+ selectedEyeOption + ')' + "Selected Lines option " + linesSetOptionNames[selectedLinesSetOption] + " ("+ selectedLinesSetOption + ')' + "Room_Var: " + roomVariantOptionNames[selectedRoomVariantOption] + " ("+ selectedRoomVariantOption + ')' );
            Debug.Log("Participants id " + participantID);
            // basePath = basePath + "/ParticipantsData/" + participantID + "_" + selectedEyeOption;
            // System.IO.Directory.CreateDirectory(basePath);
            SceneManager.LoadScene("SampleScene", LoadSceneMode.Additive); // Name "SampleScene" is important - DO NOT CHANGE!!!
            GUI.enabled = false;
            tutorialCamera.SetActive(false);
        }
    }
}
