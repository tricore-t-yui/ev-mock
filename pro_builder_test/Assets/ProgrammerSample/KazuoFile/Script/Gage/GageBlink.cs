using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲージの点滅クラス
/// </summary>
public class GageBlink : MonoBehaviour
{
    /// <summary>
    /// ゲージのタイプ
    /// </summary>
    enum GageType
    {
        BRETH,
        HEALTH,
    }

    [SerializeField]
    Image image = default;          // 点滅させるゲージの色
    [SerializeField]
    Animator brinkAnim = default;   // 点滅アニメーション
    [SerializeField]
    GageType type = default;        // ゲージのタイプ

    Color originColor = default;    // 元の色  

    /// <summary>
    /// 起動処理
    /// </summary>
    void OnEnable()
    {
        // 起動時に点滅前の色を取ってから点滅開始
        originColor = image.color;
        brinkAnim.enabled = true;

        // ゲージのタイプで点滅アニメーションを切り替える
        switch (type)
        {
            case GageType.BRETH : brinkAnim.SetTrigger("Breth");  break;
            case GageType.HEALTH: brinkAnim.SetTrigger("Health"); break;
        }
    }

    /// <summary>
    /// 停止処理
    /// </summary>
    void OnDisable()
    {
        // 点滅を終了し、元の色に戻す
        image.color = originColor;
        brinkAnim.enabled = false;
    }
}
