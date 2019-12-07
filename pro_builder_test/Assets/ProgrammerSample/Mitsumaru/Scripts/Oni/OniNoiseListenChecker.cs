using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OniNoiseListenChecker : MonoBehaviour
{
    [SerializeField]
    Animator animator = default;

    [SerializeField]
    List<string> targetTagNames = default;

    private void OnTriggerEnter(Collider other)
    {
        if (targetTagNames.Find(x => x == other.tag) != default)
        {
            // 音を聞いたフラグをオンにする
            animator.SetBool("isNoiseListener", true);
        }
    }
}
