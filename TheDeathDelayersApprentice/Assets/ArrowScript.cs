using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public float arrowSpeedMultiplier;
    public float power;
    public float enabledTime;

    void Update() { 
        if (isActiveAndEnabled) {
            enabledTime += Time.deltaTime;
        }

        transform.localScale = new Vector3(Mathf.SmoothStep(1.5f, 0.5f, Mathf.PingPong(enabledTime * arrowSpeedMultiplier, 1)), transform.localScale.y, transform.localScale.z);
        transform.localScale = new Vector3(transform.localScale.x, Mathf.SmoothStep(0.5f, 1.5f, Mathf.PingPong(enabledTime * arrowSpeedMultiplier, 1)), transform.localScale.z);
        power = Mathf.SmoothStep(4f, 11f, Mathf.PingPong(enabledTime * arrowSpeedMultiplier, 1));
    }
}
