using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterRotate : MonoBehaviour
{
    public Transform transformToCounter;

    void Update() {
        float counterRot = transformToCounter.localRotation.z;

        float inverseRot = -counterRot;
        
        transform.eulerAngles = new Vector3(0, 0, inverseRot);
    }
}
