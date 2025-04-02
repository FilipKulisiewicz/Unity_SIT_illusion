using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class binocularMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject XRrigToMimic;
    [SerializeField]
    private GameObject[] ObjectsThatMimicRot;
    private float rotOffset = -30.0f;

    // Update is called once per frame
    void Update(){
        foreach (GameObject ObjectThatMimicRot in ObjectsThatMimicRot){
            if (RoomVariantManager.instance.getRoomVariant() == RoomVariantManager.RoomVariant.side) {
                ObjectThatMimicRot.transform.localRotation = Quaternion.Euler(0, XRrigToMimic.transform.localRotation.eulerAngles.y, 0);
            }
            else if (RoomVariantManager.instance.getRoomVariant() == RoomVariantManager.RoomVariant.floor) {
                ObjectThatMimicRot.transform.localRotation = Quaternion.Euler(XRrigToMimic.transform.localRotation.eulerAngles.x + rotOffset, 0, 0);
            }
            else if (RoomVariantManager.instance.getRoomVariant() == RoomVariantManager.RoomVariant.ceiling) {
                ObjectThatMimicRot.transform.localRotation = Quaternion.Euler(XRrigToMimic.transform.localRotation.eulerAngles.x - rotOffset, 0, 0);
            }
        }
    }
}