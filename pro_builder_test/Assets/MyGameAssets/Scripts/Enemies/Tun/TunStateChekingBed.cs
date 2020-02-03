using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunStateChekingBed : StateMachineBehaviour
{
    /// <summary>
    /// ステートの開始
    /// </summary>
    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // 既にすべてのハイドポイントを確認済みであれば、その場でツンを消す
        if (animator.GetBool("isHideCheckEnd"))
        {
            animator.gameObject.SetActive(false);
        }
        else
        {
            animator.SetBool("isApproachingHide", true);
        }
    }
}
