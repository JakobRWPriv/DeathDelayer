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

    void Update() {
        if (thrownItem == null && checkCoroutine != null) {
            StopCoroutine(checkCoroutine);
        }
    }

    public void LookAtItem(GameObject go, Item item) {
        foreach(LookAtMouse lam in lookAtScripts) {
            lam.ChangeTarget(go.transform);
        }
        thrownItem = item;
        checkCoroutine = StartCoroutine(CheckIfItemHasLanded());
    }

    public void LookAtPlayer() {
        foreach(LookAtMouse lam in lookAtScripts) {
            lam.ChangeTarget(player.transform);
        }
    }

    //pingpong leantween snabb animation

    public void StopCheckCoroutine() {
        StopCoroutine(checkCoroutine);
    }

    public IEnumerator CheckIfItemHasLanded() {
        while(thrownItem == null) {
            yield return new WaitForSeconds(0.1f);
        }
        if (thrownItem != null) {
            while(!thrownItem.isGrounded) {
                print("IS NOT GROUNDED");
                if (thrownItem == null) {
                    yield break;
                }
                yield return new WaitForSeconds(0.5f);
            }
        }

        LookAtPlayer();

        thrownItem = null;
        StopCheckCoroutine();
    }
}
