using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVSwitcher : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Camera cam;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {

            cam.fieldOfView = 90f;
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {

            cam.fieldOfView = 11.25f;
        }
    }
}
