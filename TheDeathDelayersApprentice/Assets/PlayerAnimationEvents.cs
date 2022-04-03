using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    public Animator scaleAnimator;

    public void BounceLight() {
        scaleAnimator.SetTrigger("BounceLight");
    }
}
