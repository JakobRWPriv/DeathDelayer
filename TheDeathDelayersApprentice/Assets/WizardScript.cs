using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardScript : MonoBehaviour
{
    public PlayerController player;
    public LookAtMouse[] lookAtScripts;
    public Item thrownItem;
    public Coroutine checkCoroutine;
    public Animator animator;
    public bool lookingAtPlayer;

    void Update() {
        if (thrownItem == null && checkCoroutine != null) {
            StopCoroutine(checkCoroutine);
        }
    }

    public void LookAtItem(GameObject go, Item item) {
        lookingAtPlayer = false;
        foreach(LookAtMouse lam in lookAtScripts) {
            lam.ChangeTarget(go.transform);
        }
        thrownItem = item;
        //checkCoroutine = StartCoroutine(CheckIfItemHasLanded());
        print("LOOK AT ITEM: " + item.parentTransform.name);
    }

    public void LookAtPlayer() {
        lookingAtPlayer = true;
        foreach(LookAtMouse lam in lookAtScripts) {
            lam.ChangeTarget(player.transform);
        }
    }

    //pingpong leantween snabb animation

    public void StopCheckCoroutine() {
        StopCoroutine(checkCoroutine);
    }

    //public IEnumerator CheckIfItemHasLanded() {
    //    if (thrownItem != null) {
    //        while(!thrownItem.isGrounded) {
    //            print("IS NOT GROUNDED");
    //            if (thrownItem == null) {
    //                yield break;
    //            }
    //            yield return new WaitForSeconds(0.5f);
    //        }
    //        StopCheckCoroutine();
    //        thrownItem = null;
    //    }
//
    //    LookAtPlayer();
    //}
}
