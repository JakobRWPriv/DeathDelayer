using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMouse : MonoBehaviour
{
    Vector3 mousePos;
    Vector3 myWorldPos;
    Vector2 offset;
    float objectOffset;
    public float angle;
    float angleToSet;
    float angleSmoothing;
    public float speed;
    public bool playerFlipOffset;
    public Transform objectToLookAtInstead;
    public bool shouldClamp;
    public float clampMin, clampMax;

	void Update () {
        mousePos = Input.mousePosition;
        myWorldPos = Camera.main.WorldToScreenPoint(transform.position); 

        if (shouldClamp) {
            if (mousePos.y < myWorldPos.y) {
                if (mousePos.x < myWorldPos.x) {
                    angleToSet = Mathf.SmoothDampAngle(angleToSet, clampMax, ref angleSmoothing, speed);
                } else {
                    angleToSet = Mathf.SmoothDampAngle(angleToSet, clampMin, ref angleSmoothing, speed);
                }

                transform.eulerAngles = new Vector3(0, 0, angleToSet);
                return;
            }
        }

        offset = new Vector2(mousePos.x - myWorldPos.x, mousePos.y - myWorldPos.y);
        if (!playerFlipOffset) {
            angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        } else {
            angle = Mathf.Atan2(offset.y, offset.x) * (Mathf.Rad2Deg - 180);
        }

        if (objectToLookAtInstead != null) {
            Vector2 direction = (Vector2)objectToLookAtInstead.transform.position - (Vector2)transform.position;
            direction.Normalize();
            float objectAngle = Mathf.Atan2(direction.y, direction.x) * (Mathf.Rad2Deg);

            Quaternion dir = Quaternion.Euler(Vector3.forward * (objectAngle + objectOffset));
            angleToSet = Mathf.SmoothDampAngle(angleToSet, dir.eulerAngles.z, ref angleSmoothing, speed);
        } else {
            angleToSet = Mathf.SmoothDampAngle(angleToSet, angle, ref angleSmoothing, speed);
        }

        transform.eulerAngles = new Vector3(0, 0, angleToSet);

        if (shouldClamp) {
            LimitRotation();
        }
    }

    private void LimitRotation() {
        Vector3 myEulerAngles = transform.rotation.eulerAngles;

        myEulerAngles.z = (myEulerAngles.z > 180) ? myEulerAngles.z - 360 : myEulerAngles.z;
        myEulerAngles.z = Mathf.Clamp(myEulerAngles.z, clampMin, clampMax);

        transform.rotation = Quaternion.Euler(myEulerAngles);
    }

    public void ChangeSpeed(float newSpeed) {
        speed = newSpeed;
    }

    public void ChangeTarget(Transform newTarget) {
        objectToLookAtInstead = newTarget;
    }

    public void TargetMouse() {
        objectToLookAtInstead = null;
    }
}
