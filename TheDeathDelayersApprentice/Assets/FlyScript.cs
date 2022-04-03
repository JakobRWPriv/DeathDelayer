using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyScript : MonoBehaviour
{
    public float maxSpeed;
    public Rigidbody2D rb2d;
    public Transform scriptTransformObject;
    public Item itemScript;

    void Update() {
        if (itemScript.isGrounded) return;

        rb2d.velocity = new Vector3(maxSpeed, rb2d.velocity.y, 0);

        if (transform.position.x > 12 && maxSpeed > 0) {
            scriptTransformObject.localScale = new Vector3(-1, 1, 1);
            maxSpeed = -maxSpeed;
        }
        if (transform.position.x < -1 && maxSpeed < 0) {
            scriptTransformObject.localScale = new Vector3(1, 1, 1);
            maxSpeed = -maxSpeed;
        }
    }
}
