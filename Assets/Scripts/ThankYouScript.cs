using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ThankYouScript : MonoBehaviour
{
    [SerializeField]
    private GameObject[] thankYouObjects;
    
    private void OnEnable(){
        StimulusSequenceManager.action_studyEnd += displayThankYou;   
    }
    
    private void OnDisable(){
        StimulusSequenceManager.action_studyEnd -= displayThankYou;
    }

    void Awake() {
        foreach (GameObject thankYouObject in thankYouObjects){
            thankYouObject.SetActive(false);
        }
    }

    void displayThankYou(float temp){
        foreach (GameObject thankYouObject in thankYouObjects)
        {
            thankYouObject.SetActive(true);
        }
    }
}
