using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices.WindowsRuntime;

public class Menu : MonoBehaviour
{
    public static Menu instance;

    public static event Action<int> action_startLog;
    public string participantID = "";
    [SerializeField]
    public string pathToCSVVariantFile;

    private Rect windowRect = new Rect(20, 20, 450, 335);
    private int selectedEyeOption = 0, selectedLinesSetOption = 0; //, selectedRoomVariantOption = 01;
    private string[] eyeOptionNames = new string[] { "Left Eye", "Right Eye" };
    private string[] linesSetOptionNames = new string[] {"No Rotation", "Room Lines", "Answering Lines"};
    //   private string[] roomVariantOptionNames = new string[] { "Floor", "Ceiling", "Side" };

    [SerializeField]
    public GameObject tutorialCamera;
    // public string basePath;

    void Awake() {
        if (instance != null) {
            Debug.LogWarning("Multiple instances of Menu");
        }
        instance = this;
    }

    public string getParticipantID() {
        return participantID;
    }

    public CamerasManager.EyeDominance getEyeDominance() {
        return (CamerasManager.EyeDominance) selectedEyeOption;
    }
    public LinesControl.LinesSet getLinesSet()
    {
        return (LinesControl.LinesSet) selectedLinesSetOption;
    }

    private void OnGUI() {
        windowRect = GUI.Window(0, windowRect, windowFunction, "Controls");
    }

    public void Update() {

    }

    void windowFunction(int windowID){
        GUI.Label(new Rect(5, 5, 240, 20), "Participant ID:");
        participantID = GUI.TextField(new Rect(5, 35, 440, 35), participantID);
        GUI.Label(new Rect(5, 75, 240, 20), "Dominant eye:"); 
        selectedEyeOption = GUI.SelectionGrid(new Rect(5, 100, 440, 55), selectedEyeOption, eyeOptionNames, 2);
        GUI.Label(new Rect(5, 160, 240, 20), "Lines to be rotated:");
        selectedLinesSetOption = GUI.SelectionGrid(new Rect(5, 190, 440, 55), selectedLinesSetOption, linesSetOptionNames, 3);
        //GUI.Label(new Rect(5, 520, 240, 20), "Lines set to be rotated:");
        //selectedRoomVariantOption = GUI.SelectionGrid(new Rect(5, 550, 440, 195), selectedRoomVariantOption, roomVariantOptionNames, 3);
        //CSV:
        //pathToCSVVariantFile = GUI.TextField(new Rect(5, 750, 440, 45), pathToCSVVariantFile); 
        //if (GUI.Button(new Rect(5, 800, 440, 40), "Start Tracking")){
        pathToCSVVariantFile = GUI.TextField(new Rect(5, 250, 440, 35), pathToCSVVariantFile);
        if (GUI.Button(new Rect(5, 290, 440, 40), "Start Tracking")){
                action_startLog?.Invoke(0);
            Debug.Log("Selected Eye option " + eyeOptionNames[selectedEyeOption] + " (" + selectedEyeOption + ')' + "Selected Lines option " + linesSetOptionNames[selectedLinesSetOption] + " (" + selectedLinesSetOption + ')'); //+ "Room_Var: " + roomVariantOptionNames[selectedRoomVariantOption] + " ("+ selectedRoomVariantOption + ')' );
            Debug.Log("Participants id " + participantID);
            // basePath = basePath + "/ParticipantsData/" + participantID + "_" + selectedEyeOption;
            // System.IO.Directory.CreateDirectory(basePath);
            SceneManager.LoadScene("SampleScene", LoadSceneMode.Additive); // Name "SampleScene" is important - DO NOT CHANGE!!!
            GUI.enabled = false;
            tutorialCamera.SetActive(false);
        }
    }
}
