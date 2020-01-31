using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowDespawner : StateMachineBehaviour
{
    [SerializeField]
    ParticleSystem particle = default;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        animator.gameObject.SetActive(false);
    }
}
