using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Transform parentTransform;
    public int itemIndex;
    public Rigidbody2D rb2d;
    public float forceMultiplier = 1;
    public bool hitCauldron;
    public bool isGrounded;
    public bool canBePickedUp;
    public Collider2D groundTrigger;
    public float groundFriction;
    public Transform spriteTransform;
    public GameObject splashEffect;
    public bool beingHeld;
    public bool TESTOBJECT;

    Vector3 moveDirection;
    Vector3 mousePos;
    Vector3 myWorldPos;
    Vector2 offset;
    float angle;

    void Awake() {
        mousePos = Input.mousePosition;
        myWorldPos = Camera.main.WorldToScreenPoint(transform.position);
        offset = new Vector2(mousePos.x - myWorldPos.x, mousePos.y - myWorldPos.y);
        angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        spriteTransform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void OnEnable() {
        StartCoroutine(CanBePickedUpCo());
    }

    void Update() {
        if (!hitCauldron)
            isGrounded = ((Physics2D.IsTouchingLayers(groundTrigger, 1 << LayerMask.NameToLayer("Ground")) && rb2d.velocity.y < 0.01f && rb2d.velocity.y > -0.01f) || beingHeld);

        if (TESTOBJECT) {
            if (isGrounded) print("GROUNDED");
        }

        if (isGrounded) {
            rb2d.drag = groundFriction;
        } else {
            rb2d.drag = 0;
        }
    }
    
    public void GetShot() {
        transform.rotation = Quaternion.Euler(0, 0, 45);

        moveDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        moveDirection.z = 0;
        moveDirection.Normalize();

        rb2d.AddForce((100f * forceMultiplier) * moveDirection);
    }

    IEnumerator CanBePickedUpCo() {
        canBePickedUp = false;
        yield return new WaitForSeconds(0.5f);
        canBePickedUp = true;
    }

    void OnTriggerStay2D(Collider2D otherCollider) {
        if (otherCollider.tag == "CauldronHit" && rb2d.velocity.y < 0) {
            hitCauldron = true;
            isGrounded = true;
            Instantiate(splashEffect, transform.position, Quaternion.identity);
            Destroy(parentTransform.gameObject);
        }
    }
}
