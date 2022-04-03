using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float acceleration;
    public float maxSpeed;
    public float jumpPower;
    public Rigidbody2D rb2d;
    public bool isGrounded;
    public Collider2D groundTrigger;
    bool hasLanded;
    bool isFacingRight;
    public Animator animator;
    public Animator scaleAnimator;
    public bool walkingKeys;
    public bool holdingShot;
    public Transform bodyBendBoneTransform;
    public LookAtMouse bodyLookAtMouse;
    public Transform flipTransform;
    public Transform flipTransform2;
    public ArrowScript aimingArrow;
    public Item activeHeldItem;
    public bool isHoldingItem;
    public Transform heldItemsTransform;
    public GameObject[] holdableItems;
    public GameObject activeHeldItemSprite;
    public WizardScript wizard;

    public SpriteRenderer[] allSprites;
    public GameObject spritePos;

    public int health = 5;
    bool isTakingDamage;
    bool isBlinking;
    bool cannotMove;
    bool isInvincible;

    void Start() {
        allSprites = spritePos.GetComponentsInChildren<SpriteRenderer>(true);
        isFacingRight = true;
    }

    float KeysDirectionXAndFacing, KeysDirectionY, keysDirectionXAndFacingSmoothing, keysDirectionYSmoothing;
    void Update() {
        isGrounded = (Physics2D.IsTouchingLayers(groundTrigger, 1 << LayerMask.NameToLayer("Ground")) && rb2d.velocity.y < 0.01f && rb2d.velocity.y > -0.01f);

        animator.SetBool("Walking", isHoldingWalkingKeys());
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetFloat("yVelocity", rb2d.velocity.y);

        //if (isHoldingWalkingKeys()) {
        //    animator.speed = Mathf.Abs(rb2d.velocity.x / 6f);
        //    walkingKeys = true;
        //} else {
        //    animator.speed = 1;
        //}
        
        if (!isGrounded && hasLanded) {
            hasLanded = false;
        }

        if (isGrounded && !hasLanded) {
            hasLanded = true;
            scaleAnimator.SetTrigger("BounceLand");
            //AudioHandler.Instance.PlaySound(AudioHandler.Instance.playerLand, 0.6f, Random.Range(0.95f, 1.2f));
        }

        if (transform.localScale.x > 0 && !isFacingRight)
            isFacingRight = true;
        else if (transform.localScale.x < 0 && isFacingRight)
            isFacingRight = false;

        Shoot();
        Jump();
        FlipPlayer();
    }

    private bool isHoldingWalkingKeys() {
        return(Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.D)));
    }

    IEnumerator PickUpItemCo(Item item) {
        while (item == null) {
            print("ISNULL");
            yield return new WaitForEndOfFrame();
        }
        PickUpItem(item);
    }

    private void PickUpItem(Item item) {
        activeHeldItem = item;
        activeHeldItem.beingHeld = true;
        item.parentTransform.gameObject.SetActive(false);
        item.parentTransform.parent = heldItemsTransform;
        item.parentTransform.localPosition = Vector3.zero;
        activeHeldItemSprite = holdableItems[item.itemIndex];
        holdableItems[item.itemIndex].SetActive(true);
    }

    private void Shoot() {
        if (Input.GetKeyDown(KeyCode.Mouse0) && isHoldingItem) {
            holdingShot = true;
            aimingArrow.enabledTime = 0;
            aimingArrow.gameObject.SetActive(true);
            animator.SetTrigger("SpitterCharge");
        }
        if (Input.GetKeyUp(KeyCode.Mouse0) && holdingShot) {
            activeHeldItem.parentTransform.parent = null;
            activeHeldItem.parentTransform.gameObject.SetActive(true);
            activeHeldItem.forceMultiplier = aimingArrow.power;
            activeHeldItem.beingHeld = false;
            activeHeldItem.isGrounded = false;

            wizard.LookAtItem(activeHeldItem.parentTransform.gameObject, activeHeldItem);

            activeHeldItem.GetShot();
            aimingArrow.gameObject.SetActive(false);
            activeHeldItemSprite.SetActive(false);
            animator.SetTrigger("SpitterSpit");
            holdingShot = false;
            isHoldingItem = false;
            activeHeldItem = null;
        }
    }

    private void Jump() {
        if (cannotMove) return;

#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.U)) {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpPower);
        }
#endif

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Space)) {
            if (isGrounded) {
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpPower);
                
                //sizeAnimator.SetTrigger("JumpBounce");
                //AudioHandler.Instance.PlaySound(AudioHandler.Instance.playerJump, 0.6f, Random.Range(0.7f, 0.75f));
            }
        }

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.Z)) {
            if (rb2d.velocity.y > 0)
                rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y * 0.5f);
        }
    }

    private void FlipPlayer() {
        //if ((Input.GetKey(KeyCode.D) && flipTransform2.localScale.x < 0) ||
        //            (((Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.Q))) && flipTransform2.localScale.x > 0))) {
        //    flipTransform.localScale = new Vector3(-flipTransform.localScale.x, flipTransform.localScale.y, flipTransform.localScale.z);
        //    flipTransform2.localScale = new Vector3(-flipTransform2.localScale.x, flipTransform2.localScale.y, flipTransform2.localScale.z);
        //}

        if (Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.Q))) {
            flipTransform.localScale = new Vector3(-1, flipTransform.localScale.y, flipTransform.localScale.z);
            flipTransform2.localScale = new Vector3(-1, flipTransform2.localScale.y, flipTransform2.localScale.z);
        }
        if (Input.GetKeyUp(KeyCode.A) || (Input.GetKeyUp(KeyCode.Q))) {
            flipTransform.localScale = new Vector3(1, flipTransform.localScale.y, flipTransform.localScale.z);
            flipTransform2.localScale = new Vector3(-1, flipTransform2.localScale.y, flipTransform2.localScale.z);
        }
        
        if (Input.GetKey(KeyCode.D)) {
            flipTransform.localScale = new Vector3(1, flipTransform.localScale.y, flipTransform.localScale.z);
            flipTransform2.localScale = new Vector3(1, flipTransform2.localScale.y, flipTransform2.localScale.z);
        }
        if (Input.GetKeyUp(KeyCode.D)) {
            flipTransform.localScale = new Vector3(1, flipTransform.localScale.y, flipTransform.localScale.z);
            flipTransform2.localScale = new Vector3(1, flipTransform2.localScale.y, flipTransform2.localScale.z);
        }

        if (Input.GetKey(KeyCode.D) && (Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.Q)))) {
            flipTransform.localScale = new Vector3(1, flipTransform.localScale.y, flipTransform.localScale.z);
            flipTransform2.localScale = new Vector3(1, flipTransform2.localScale.y, flipTransform2.localScale.z);
        }
    }

    void FixedUpdate() {
        AddFriction();
        
        if (cannotMove) return;

        if (Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.Q))) {
            float accel = acceleration;
            if (rb2d.velocity.x > 0) {
                accel = acceleration * 2f;
            }
            if (rb2d.velocity.x > -maxSpeed) {
                rb2d.AddForce(-Vector3.right * accel);
            }
        }
        if (Input.GetKey(KeyCode.D)) {
            float accel = acceleration;
            if (rb2d.velocity.x < 0) {
                accel = acceleration * 2f;
            }
            if (rb2d.velocity.x < maxSpeed) {
                rb2d.AddForce(Vector3.right * accel);
            }
        }
    }

    private void AddFriction() {
        if (cannotMove) return;

        if (!(Input.GetKey(KeyCode.D)) && !(Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.Q)))) {
            if (isGrounded)
                rb2d.velocity = new Vector2(rb2d.velocity.x * 0.75f, rb2d.velocity.y);
            else 
                rb2d.velocity = new Vector2(rb2d.velocity.x * 0.9f, rb2d.velocity.y);

            if (isGrounded) {
                if (isFacingRight && rb2d.velocity.x < 0.1f) {
                    rb2d.velocity = new Vector2(0, rb2d.velocity.y);
                } else if (!isFacingRight && rb2d.velocity.x > -0.1f) {
                    rb2d.velocity = new Vector2(0, rb2d.velocity.y);
                }
            }
        } else if ((Input.GetKey(KeyCode.D)) && (Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.Q)))) {
            rb2d.velocity = new Vector2(rb2d.velocity.x / 1.2f, rb2d.velocity.y);
        }
    }

    void OnTriggerEnter2D(Collider2D otherCollider) {
        if (otherCollider.tag == "Enemy") {
            
        }

        if (otherCollider.tag == "Item" && !isHoldingItem) { 
            Item itemToPickUp = otherCollider.GetComponent<Item>();
            if (itemToPickUp.canBePickedUp) {
                isHoldingItem = true;
                PickUpItem(itemToPickUp);
            }
        }
    }

    void OnTriggerStay2D(Collider2D otherCollider) {
        if (otherCollider.tag == "Enemy") {
            if (!isInvincible) {
                //TakeDamage(1, otherCollider.transform.position.x < transform.position.x);
                //isInvincible = true;
                //AudioHandler.Instance.PlaySound(AudioHandler.Instance.playerDamaged);
            }
        }

        if (otherCollider.tag == "Item" && !isHoldingItem) { 
            Item itemToPickUp = otherCollider.GetComponent<Item>();
            if (itemToPickUp.canBePickedUp) {
                isHoldingItem = true;
                PickUpItem(itemToPickUp);
            }
        }
    }

    public void SpriteBlink() {
        StartCoroutine(SpriteBlinkCo());
    }

    public IEnumerator SpriteBlinkCo() {
        foreach(SpriteRenderer sr in allSprites) {
            sr.enabled = false;
        }

        yield return new WaitForSeconds(0.1f);

        foreach(SpriteRenderer sr in allSprites) {
            sr.enabled = true;
        }

        yield return new WaitForSeconds(0.15f);

        if (isBlinking) {
            StartCoroutine(SpriteBlinkCo());
        }
    }
}
