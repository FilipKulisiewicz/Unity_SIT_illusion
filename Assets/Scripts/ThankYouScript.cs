using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ThankYouScript : MonoBehaviour
{
    [SerializeField]
    private GameObject thankYouBoard;
    
    private void OnEnable(){
        StimulusSequenceManager.action_studyEnd += displayThankYou;   
    }
    
    private void OnDisable(){
        StimulusSequenceManager.action_studyEnd -= displayThankYou;
    }

    void Awake(){
        thankYouBoard.SetActive(false);
    }

    void displayThankYou(float temp){
        thankYouBoard.SetActive(true);
    }
}
