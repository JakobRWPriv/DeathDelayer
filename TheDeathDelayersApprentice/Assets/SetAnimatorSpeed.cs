using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAnimatorSpeed : MonoBehaviour
{
    public Animator animator;
    public float animatorSpeed;

    void Start() {
        animator.speed = animatorSpeed;
    }
}
