using UnityEngine;

public class FloatingNumber3D : MonoBehaviour{
    private int numberToDisplay = 0; 
    private GameObject textObject;
    private TextMesh textMesh;

    void OnEnable(){
        StimulusSequenceManager.action_numberUpdate += setNumber;
    }

    void OnDestroy(){
        StimulusSequenceManager.action_numberUpdate -= setNumber;
    }   

    void Awake(){
        textObject = new GameObject("FloatingNumberText");
        textMesh = textObject.AddComponent<TextMesh>();
        textMesh.fontSize = 35;
        textMesh.characterSize = 0.1f;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.text = numberToDisplay.ToString();
        textObject.transform.position = transform.position;
    }

    void setNumber(int newNumber){
        textMesh.text = newNumber.ToString();
        textObject.transform.position = transform.position;
    }
}