using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 影人間の聴こえる範囲のコライダーを監視する
/// </summary>
public class HearRangeColliderWatcher : MonoBehaviour
{
    // 対象のタグ名
    [SerializeField]
    string targetTagName = default;

    // 物音聴こえた瞬間
    UnityAction<Collider> onHearEnter;

    /// <summary>
    /// コールバックのセット
    /// </summary>
    /// <param name="call"></param>
    public void SetCallback(UnityAction<Collider> call)
    {
        onHearEnter = call;
    }

    /// <summary>
    /// コライダーに当たった瞬間
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        // タグが一致したら
        if (other.tag == targetTagName)
        {
            onHearEnter(other);
        }
    }
}
