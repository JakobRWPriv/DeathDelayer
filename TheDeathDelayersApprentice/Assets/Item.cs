using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public WizardScript wizard;
    public Transform parentTransform;
    public int itemIndex;
    public Rigidbody2D rb2d;
    public float forceMultiplier = 1;
    public bool hitCauldron;
    public bool isGrounded;
    public bool hasLandedOnGround;
    public bool canBePickedUp;
    public Collider2D groundTrigger;
    public float groundFriction;
    public Transform spriteTransform;
    public GameObject splashEffect;
    public bool beingHeld;
    public bool TESTOBJECT;
    public Sprite itemSprite;
    public bool rotateTo0WhenGrounded;
    public bool isRock;
    public bool isTear;
    public GameObject tearParticles;

    Vector3 moveDirection;
    Vector3 mousePos;
    Vector3 myWorldPos;
    Vector2 offset;
    float angle;

    void OnEnable() {
        if (isTear) {
            LeanTween.scale(spriteTransform.gameObject, Vector3.one, 0.3f);
        }
        StartCoroutine(CanBePickedUpCo());
    }

    void Update() {
        if (!hitCauldron) {
            if (!isTear)
                isGrounded = ((Physics2D.IsTouchingLayers(groundTrigger, 1 << LayerMask.NameToLayer("Ground")) && rb2d.velocity.y < 0.01f && rb2d.velocity.y > -0.01f) || beingHeld);
            else 
                isGrounded = ((Physics2D.IsTouchingLayers(groundTrigger, 1 << LayerMask.NameToLayer("Ground"))));
        }

        if (TESTOBJECT) {
            if (isGrounded) print("GROUNDED");
        }

        if (isTear && isGrounded) {
            Instantiate(tearParticles, parentTransform.position, Quaternion.identity);
            Destroy(parentTransform.gameObject);
        }

        if (isGrounded && !hasLandedOnGround) {
            hasLandedOnGround = true;
            if (!isTear) {
                if (!wizard.lookingAtPlayer) {
                    wizard.LookAtPlayer();
                }
            }
        }

        if (isGrounded) {
            rb2d.drag = groundFriction;
            if (rotateTo0WhenGrounded) {
                spriteTransform.rotation = Quaternion.Euler(0, 0, 0);
            }
        } else {
            rb2d.drag = 0;
            hasLandedOnGround = false;
        }
    }
    
    public void GetShot() {
        //transform.rotation = Quaternion.Euler(0, 0, 45);

        moveDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        moveDirection.z = 0;
        moveDirection.Normalize();
        LimitRotation();

        rb2d.AddForce((100f * forceMultiplier) * moveDirection);
        
        spriteTransform.rotation = Quaternion.Euler(0, 0, moveDirection.z);
    }

    private void LimitRotation() {
        moveDirection.z = (moveDirection.z > 180) ? moveDirection.z - 360 : moveDirection.z;
        moveDirection.z = Mathf.Clamp(moveDirection.z, 60, 160);
    }

    IEnumerator CanBePickedUpCo() {
        canBePickedUp = false;
        yield return new WaitForSeconds(0.5f);
        canBePickedUp = true;
    }

    void OnTriggerStay2D(Collider2D otherCollider) {
        if (otherCollider.tag == "CauldronHit" && rb2d.velocity.y < 0 && !isRock) {
            hitCauldron = true;
            isGrounded = true;
            Instantiate(splashEffect, transform.position, Quaternion.identity);
            Destroy(parentTransform.gameObject);
        }
    }
}
