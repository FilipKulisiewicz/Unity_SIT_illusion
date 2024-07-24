using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragRig4 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public GameObject XrRig;

    private Vector3 viewSpot;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            viewSpot = this.transform.position + (this.transform.rotation * this.transform.forward * 0.2f);
            viewSpot[1] = -0.375f;

            Debug.Log(viewSpot);

            XrRig.transform.position = viewSpot;
        }
    }
}
