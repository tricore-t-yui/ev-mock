using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// キャラクターの出入りの監視を行う
/// </summary>
public class AreaEnterWatcher : MonoBehaviour
{
    // 隣接しているエリアの名のハッシュ
    int adjacentAreaNameHash;

    // デリゲート
    OnEnterDelegate onEnterDelegate;
    OnExitDelegate onExitDelegate;

    /// <summary>
    /// 開始
    /// </summary>
    void Start()
    {
        // 親オブジェクトを取得
        Transform parent = transform.parent;
        // 子オブジェクト番号
        int childIndex = 0;
        // 互いのエリアの子オブジェクト番号を取得
        if (transform.GetSiblingIndex() == 0) { childIndex = 1; }
        else                                  { childIndex = 0; }
        // 相手の子オブジェクトを取得
        Transform adjacentChild = parent.GetChild(childIndex);
        // 子オブジェクトの名のハッシュを取得
        adjacentAreaNameHash = adjacentChild.name.GetHashCode();

    }

    /// <summary>
    /// エリア行った時のデリゲートをセット
    /// <summary>
    public void SetOnEnterDelegate(OnEnterDelegate set)
    {
        onEnterDelegate = set;
    }

    /// <summary>
    /// エリアから出たときのデリゲートをセット
    /// </summary>
    public void SetOnExitDelegate(OnExitDelegate set)
    {
        onExitDelegate = set;
    }

    /// <summary>
    /// ゲートのコライダーに当たったとき
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        // エリア名とキャラクター名のハッシュ値を取得
        int areaNameHash = name.GetHashCode();
        int charaNameHash = other.name.GetHashCode();

        // デリゲートからコールバック関数を呼ぶ
        onExitDelegate(adjacentAreaNameHash, charaNameHash);
        onEnterDelegate(areaNameHash, charaNameHash);
    }
}
