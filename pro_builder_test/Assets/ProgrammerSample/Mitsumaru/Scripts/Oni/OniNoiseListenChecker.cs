using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 鬼が音を聞き取ったか確認
/// </summary>
public class OniNoiseListenChecker : MonoBehaviour
{
    // アニメーター
    [SerializeField]
    Animator animator = default;

    // 対象のタグ
    [SerializeField]
    List<string> targetTagNames = default;

    /// <summary>
    /// 聞き取った瞬間
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (targetTagNames.Find(x => x == other.tag) != default)
        {
            // 音を聞いたフラグをオンにする
            StartCoroutine(OnListener());
        }
    }

    IEnumerator OnListener()
    {
        animator.SetBool("isNoiseListener", true);
        yield return null;
        animator.SetBool("isNoiseListener", false);
    }
}
