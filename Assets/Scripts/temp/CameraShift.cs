using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShift : MonoBehaviour
{
    public float movespeed = 0.5f;
    private float _hInput, _vInput, _uInput;
    public int currentRoom = 3;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && currentRoom != 1) {
            this.transform.Translate(Vector3.right * -5);
            currentRoom -= 1; 
        }
        if (Input.GetKeyDown(KeyCode.E) && currentRoom != 3)
        {
            this.transform.Translate(Vector3.right * 5);
            currentRoom += 1;
        }

        _hInput = Input.GetAxis("Horizontal") * movespeed;
        _vInput = Input.GetAxis("Vertical") * movespeed;
        _uInput = Input.GetAxis("Up") * movespeed;

        this.transform.Translate(Vector3.right * _hInput * Time.deltaTime);
        this.transform.Translate(Vector3.forward * _vInput * Time.deltaTime);
        this.transform.Translate(Vector3.up * _uInput * Time.deltaTime);
    }
}
