using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragRig1 : MonoBehaviour
{
    public GameObject XrRig;
    private Vector3 viewSpot;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            viewSpot = this.transform.position + (this.transform.rotation * this.transform.forward * 0.2f);
            viewSpot[1] = 1.625f;

            Debug.Log(viewSpot);

            XrRig.transform.position = viewSpot;
        }
    }
}
