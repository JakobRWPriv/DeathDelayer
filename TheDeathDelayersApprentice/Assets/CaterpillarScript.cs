using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaterpillarScript : MonoBehaviour
{
    public float moveSpeed;
    public float minSpeed, maxSpeed;
    public float speedMultiplier;
    public Rigidbody2D rb2d;
    public Transform scriptTransformObject;
    public Item itemScript;

    void Update() {
        if (!itemScript.isGrounded) return;

        rb2d.velocity = new Vector3(Mathf.SmoothStep(maxSpeed, minSpeed, Mathf.PingPong(Time.time * speedMultiplier, 1)), rb2d.velocity.y, 0);

        if (transform.position.x > 24 && scriptTransformObject.localScale.x < 0) {
            scriptTransformObject.localScale = new Vector3(1, 1, 1);
            maxSpeed = -maxSpeed;
        }
        if (transform.position.x < -24 && scriptTransformObject.localScale.x > 0) {
            scriptTransformObject.localScale = new Vector3(-1, 1, 1);
            maxSpeed = -maxSpeed;
        }
    }
}
