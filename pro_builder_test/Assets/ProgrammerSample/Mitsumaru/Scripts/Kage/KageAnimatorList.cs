using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 影人間のアニメーターリスト
/// </summary>
public class KageAnimatorList : MonoBehaviour
{
    // 影人間管理クラス
    [SerializeField]
    KageManager kageManager = default;

    public Dictionary<int, Animator> Animators { get; private set; } = new Dictionary<int, Animator>();

    /// <summary>
    /// 開始
    /// </summary>
    void Start()
    {
        Animators = kageManager.KageList.ToDictionary(kageKey => kageKey.gameObject.GetInstanceID(), kageValue => kageValue.GetComponent<Animator>());
    }
}
