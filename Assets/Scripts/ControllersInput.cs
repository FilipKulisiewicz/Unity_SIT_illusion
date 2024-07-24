using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using System.IO;
using System; 

public class ControllersInput : MonoBehaviour
{
    public static event Action<float, float> action_triggersValueChange;
    public static event Action<float> action_joyStickRightValueChange;
    public static event Action<float> action_buttonLeftValueChange;
    public static event Action<float> action_nextScene;
    private static ControllersInput instance;
    [SerializeField]
    public XRBaseController controllerRight, controllerLeft;
    [SerializeField]
    public InputActionProperty triggerRight, triggerLeft;
    float triggerRightValue = 0.0f, triggerLeftValue = 0.0f, prev_triggerRightValue = 0.0f, prev_triggerLeftValue = 0.0f;    
    [SerializeField]
    public InputActionProperty joyStickRight;
    float joyStickRightValue = 0.0f, prev_joyStickRightValue = 0.0f;
    [SerializeField]
    public InputActionProperty buttonLeft, buttonRight, buttonSecondaryRight;
    float buttonLeftValue = 0.0f, buttonRightValue = 0.0f, buttonSecondaryRightValue = 0.0f;
    private float lastLeftPressTime = 0.0f, longPressStart = 0.0f;
    private const float clickDelay = 0.5f, longPressMinimumTimeLength = 2.5f;
    private bool statedLongPress = false;

    void Awake(){
        if(instance!=null)
            Debug.LogWarning("Multiple instances of ControllerInput");
        instance = this;
    }

    void Update(){
        //joystick
        joyStickRightValue = joyStickRight.action.ReadValue<Vector2>()[1];
        if(prev_joyStickRightValue != joyStickRightValue){   
            //Debug.LogWarning("joyStickRightValue : " + joyStickRightValue[1]);
            prev_joyStickRightValue = joyStickRightValue; 
            action_joyStickRightValueChange?.Invoke(joyStickRightValue);  
        }
        //trigger
        triggerRightValue = triggerRight.action.ReadValue<float>();
        triggerLeftValue = triggerLeft.action.ReadValue<float>();
        if(prev_triggerRightValue != triggerRightValue || prev_triggerLeftValue != triggerLeftValue){
            //Debug.LogWarning("ControllerInputs: " + triggerLeftValue + " " + triggerRightValue);
            action_triggersValueChange?.Invoke(triggerRightValue, triggerLeftValue);  
            prev_triggerRightValue = triggerRightValue;
            prev_triggerLeftValue = triggerLeftValue;
        }
        //left button
        buttonLeftValue = buttonLeft.action.ReadValue<float>();
        if(buttonLeftValue > 0.8f){
            if (lastLeftPressTime + clickDelay > Time.unscaledTime){
                return;
            }
            lastLeftPressTime = Time.unscaledTime;
            action_buttonLeftValueChange?.Invoke(buttonLeftValue);
        }
        //right buttons
        buttonRightValue = buttonRight.action.ReadValue<float>();
        buttonSecondaryRightValue = buttonSecondaryRight.action.ReadValue<float>();
        if(buttonSecondaryRightValue > 0.8f && buttonRightValue > 0.8f && statedLongPress == false){
            longPressStart = Time.unscaledTime;
            statedLongPress = true;
        }
        else if((buttonSecondaryRightValue < 0.8f || buttonRightValue < 0.8f) && statedLongPress == true){
            statedLongPress = false;
        }
        else if(buttonSecondaryRightValue > 0.8f && buttonRightValue > 0.8f && statedLongPress == true){
            if(longPressStart + longPressMinimumTimeLength < Time.unscaledTime){
                controllerRight.SendHapticImpulse(0.4f, 0.4f);
                controllerLeft.SendHapticImpulse(0.4f, 0.4f);
                statedLongPress = false;
                Debug.Log("Final Answers Submited:");
                action_nextScene?.Invoke(0.0f); //0.0f is a random value - no meaning
            }
        }
    }
}

