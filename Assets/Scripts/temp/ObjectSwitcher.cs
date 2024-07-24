using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSwitcher : MonoBehaviour
{
    public GameObject Stim1, Stim2, Stim3, Stim4;
    public int current_stim;

    void UpdateStimulus()
    {
        ToggleStimuli(false);
        switch (current_stim)
        {
            case 0:
                Stim1.SetActive(true);
                break;
            case 1:
                Stim2.SetActive(true);
                break;
            case 2:
                Stim3.SetActive(true);
                break;
            case 3:
                Stim4.SetActive(true);
                break;
        }
    }

    void ToggleStimuli(bool state){
        Stim1.SetActive(state);
        Stim2.SetActive(state);
        Stim3.SetActive(state);
        Stim4.SetActive(state);
    }

    // Start is called before the first frame update
    void Start()
    {
        current_stim = 0;
        UpdateStimulus();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            current_stim = 0;
            UpdateStimulus();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            current_stim = 1;
            UpdateStimulus();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            current_stim = 2;
            UpdateStimulus();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            current_stim = 3;
            UpdateStimulus();
        }
    }
}
