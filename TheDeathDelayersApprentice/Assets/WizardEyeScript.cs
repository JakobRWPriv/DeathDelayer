using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardEyeScript : MonoBehaviour
{
    public bool isIrritated;
    public Animator eyeAnimator;
    public GameObject tear;
    Coroutine eyeCoroutine;
    public Collider2D myCollider1;

    void OnTriggerStay2D(Collider2D otherCollider) {
        if (otherCollider.tag == "Item" && !isIrritated) { 
            isIrritated = true;
            eyeAnimator.SetTrigger("Shot");
            Instantiate(tear, transform.position, Quaternion.identity);
            if (eyeCoroutine != null) {
                StopCoroutine(eyeCoroutine);
            }
            eyeCoroutine = StartCoroutine(EyeBackToNormal());
        }
    }

    IEnumerator EyeBackToNormal() {
        yield return new WaitForSeconds(0.3f);
        myCollider1.enabled = false;
        yield return new WaitForSeconds(0.7f);
        eyeAnimator.SetTrigger("Idle");
        yield return new WaitForSeconds(1f);
        myCollider1.enabled = true;
        eyeCoroutine = null;
        isIrritated = false;
    }
}
