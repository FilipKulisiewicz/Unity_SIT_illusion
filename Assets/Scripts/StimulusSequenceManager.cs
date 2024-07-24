using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StimulusSequenceManager : MonoBehaviour
{
    public class SceneData{ //CSV variables
        public int Lp { get; set; }
        public int EyeNum { get; set; }
        public int FOV { get; set; }
        public float StimRot { get; set; }
        public float RoomRot { get; set; }
    }

    // [SerializeField]
    private string csvFilePath;
    private List<SceneData> sceneCodes;
    private int currentSceneIndex = 0;
    //variables from GUI 
    private CamerasManager.EyeDominance eyeDominant = (CamerasManager.EyeDominance)0;
    private LinesRotation.LinesSet linesSetToRotate = (LinesRotation.LinesSet)0;
    private RoomVariantManager.RoomVariant roomVariantOption = (RoomVariantManager.RoomVariant)0; 

    public static event Action<float> action_sceneChangeLinesRot, action_sceneChangeAnsSlantRot, action_sceneChangeFOV, action_sceneChangeCam, action_sceneSetStimRoomRotation, action_studyEnd;
    public static event Action<int> action_numberUpdate;
    public static event Action<int, CamerasManager.EyeDominance> action_sceneChangeEye; //maybe split into two events ??
    public static event Action<LinesRotation.LinesSet> action_sceneChangeRotationLinesSet;
    public static event Action<RoomVariantManager.RoomVariant> action_sceneChangeRoomVariant;
    public static event Func<float> func_requestAngleSlant, func_requestAngleStimuli;
    
    private void Awake(){
        csvFilePath = Menu.instance.pathToCSVVariantFile;
        sceneCodes = ReadCSV(csvFilePath);
        if (sceneCodes.Count > 0){
            Debug.Log("Scene codes read from CSV:" + sceneCodes.Count);
        }
    }

    private void OnEnable(){
        ControllersInput.action_nextScene += OnSceneChange;
    }

    private void OnDestroy(){
        ControllersInput.action_nextScene -= OnSceneChange;
    }

    private void getGUIVariablesFromMenu(){
        eyeDominant = (CamerasManager.EyeDominance) Menu.instance.selectedEyeOption;
        linesSetToRotate = (LinesRotation.LinesSet) Menu.instance.selectedLinesSetOption;
        roomVariantOption = (RoomVariantManager.RoomVariant) Menu.instance.selectedRoomVariantOption;
    }

    private void Start(){ //after all object in the scene finish awake processes
        getGUIVariablesFromMenu();
        currentSceneIndex = 0;
        LoadScene(currentSceneIndex);
    }

    private void OnSceneChange(float _temp){
        Debug.Log("Final Answer: " + currentSceneIndex + "; EyeNum: " + sceneCodes[currentSceneIndex].EyeNum + "; FOV: " + sceneCodes[currentSceneIndex].FOV + "; Stim_r_0: " 
                    + sceneCodes[currentSceneIndex].StimRot + "; Answer_Stim_deg:" + func_requestAngleStimuli?.Invoke() + "; Answer_Slant_deg:" + func_requestAngleSlant?.Invoke());
        
        // Logic to determine the new scene index based on newValue
        currentSceneIndex = currentSceneIndex + 1;
        LoadScene(currentSceneIndex);
    }

    private void LoadScene(int sceneIndex){
        if (sceneIndex >= 0 && sceneIndex < sceneCodes.Count){
            action_sceneChangeRotationLinesSet?.Invoke(linesSetToRotate);
            action_sceneChangeRoomVariant?.Invoke(roomVariantOption);
            action_sceneChangeEye?.Invoke(sceneCodes[sceneIndex].EyeNum, eyeDominant);
            action_sceneChangeLinesRot?.Invoke(sceneCodes[sceneIndex].StimRot);
            action_sceneChangeFOV?.Invoke(sceneCodes[sceneIndex].FOV);
            action_sceneChangeAnsSlantRot?.Invoke(90.0f); //default
            action_sceneSetStimRoomRotation?.Invoke(sceneCodes[sceneIndex].RoomRot);
            action_sceneChangeCam?.Invoke(0.0f); //default
            action_numberUpdate?.Invoke(sceneCodes.Count - sceneIndex);
            Debug.Log("Loading scene: " + sceneIndex + "; EyeNum: " + sceneCodes[sceneIndex].EyeNum + "; FOV: " + sceneCodes[sceneIndex].FOV + "; Stim_r_0: " + sceneCodes[sceneIndex].StimRot + "; ");
        }
        else if(sceneIndex == sceneCodes.Count){  //"Thank you" scene
            action_sceneChangeLinesRot?.Invoke(0.0f);
            action_sceneChangeAnsSlantRot?.Invoke(90.0f);
            action_sceneChangeCam?.Invoke(0.0f);
            action_studyEnd?.Invoke(0.0f);
            action_numberUpdate?.Invoke(sceneIndex - sceneIndex);
            Debug.Log("End - finished sessions:" + sceneCodes.Count);
        }
        else{
            Debug.LogError("Scene index out of range");
        }
    }

    private List<SceneData> ReadCSV(string filePath){
        List<SceneData> sceneDataList = new List<SceneData>();
        try{
            using (StreamReader reader = new StreamReader(filePath)){
                string line;
                bool isHeader = true;
                for (int i = 1; (line = reader.ReadLine()) != null; i++){
                    if (isHeader){
                        isHeader = false; // Skip header line
                        continue;
                    }
                    string[] values = line.Split(';');
                    if (values.Length == 4 &&
                        int.TryParse(values[0], out int eyeNum) &&
                        int.TryParse(values[1], out int fov) &&
                        float.TryParse(values[2], out float stimRot) &&
                        float.TryParse(values[3], out float roomRot))
                    {
                        SceneData data = new SceneData{
                            Lp = i,
                            EyeNum = eyeNum,
                            FOV = fov,
                            StimRot = stimRot,
                            RoomRot = roomRot
                        };
                        sceneDataList.Add(data);
                    }
                    else{
                        Debug.LogError($"Invalid data in CSV: {line}");
                    }
                }
            }
        }
        catch (Exception e){
            Debug.LogError($"Failed to read CSV file: {e.Message}");
        }
        return sceneDataList;
    }
}
