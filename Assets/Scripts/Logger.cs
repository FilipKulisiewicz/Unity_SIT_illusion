using System;
using System.IO;
using UnityEngine;

public class LogToFile : MonoBehaviour
{
    [SerializeField]
    public string participantsDataFolder;
    private string txtPath, csvPath;
    private bool firstWrite = true;

    private void OnEnable(){
        Application.logMessageReceived += HandleLog;
        Menu.action_startLog += getIDAndStartLog;
    }

    private void OnDestroy(){
        Application.logMessageReceived -= HandleLog;
        Menu.action_startLog -= getIDAndStartLog;
    }

    private void getIDAndStartLog(int _temp){
        if (string.IsNullOrEmpty(participantsDataFolder)){
            Debug.LogError("Base path is not set!");
            return;
        }     
        txtPath = participantsDataFolder + "\\" + Menu.instance.getParticipantID() + ".txt";
        csvPath = participantsDataFolder + "\\" + Menu.instance.getParticipantID() + ".csv";
        Directory.CreateDirectory(Path.GetDirectoryName(txtPath)); // Ensure the directory exists
    }

    private void HandleLog(string logString, string stackTrace, LogType type){
        WriteToFile(logString, stackTrace, type);
    }

    private void WriteToFile(string logString, string stackTrace, LogType type){
        try{
            using (StreamWriter writer = new StreamWriter(txtPath, true)){
                string[] logLines = logString.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                string firstLogLine = logLines.Length > 0 ? logLines[0] : logString;

                writer.WriteLine($"[{DateTime.Now}] [{type}] {firstLogLine}");

                string[] stackTraceLines = stackTrace.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                if (stackTraceLines.Length > 0){
                    writer.WriteLine(stackTraceLines[0]);
                }
            }
            // Write to CSV file if the log entry contains relevant scene information
            if (firstWrite == true){
                using (StreamWriter csvWriter = new StreamWriter(csvPath, true)){
                    string participantID = Menu.instance.getParticipantID();
                    csvWriter.WriteLine($"Participant ID:,{participantID},,,,");
                    csvWriter.WriteLine($"Dominant Eye:,{Menu.instance.selectedEyeOption},Room Variant:,{Menu.instance.selectedRoomVariantOption},selectedLinesOption:,{Menu.instance.selectedLinesSetOption}");
                    csvWriter.WriteLine("Loading scene,Eye,FOV,Stim_r_0,Answer_Stim_deg,Answer_Slant_deg");
                }
                firstWrite = false;
            }
            if (logString.Contains("Final Answer:")){
                using (StreamWriter csvWriter = new StreamWriter(csvPath, true)){
                    // Parse the logString to extract relevant data
                    string[] parts = logString.Split(new[] { ':', ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 11){
                        // Format: Loading scene: 0; Eye: 2; FOV: 90; Stim_r_0: 1;Answer Stim_deg: 0.4; Answer Slant_deg: 58.49997
                        string sceneIndex = parts[1].Trim();
                        string eye = parts[3].Trim();
                        string fov = parts[5].Trim();
                        string stim_r_0 = parts[7].Trim();
                        string answerStimDeg = parts[9].Trim();
                        string answerSlantDeg = parts[11].Trim();

                        // Write to CSV file in the specified format
                        string csvEntry = $"{sceneIndex},{eye},{fov},{stim_r_0},{answerStimDeg},{answerSlantDeg}";
                        csvWriter.WriteLine(csvEntry);
                    }
                }
            }
        }
        catch (Exception e){
            Debug.LogWarning($"Failed to write log to file: {e}");
        }
    }
}
