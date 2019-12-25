using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 影人間のステート：音が聞こえて何かを察知
/// </summary>
public class KageStatePerception : StateMachineBehaviour
{
    // 大音量とみなす音の最小サイズ
    [SerializeField]
    float loundVolumeRadiusMin = 0;

    [SerializeField]
    int perceptionTime = 0;
    int perceptionTimeCounter = 0;

    /// <summary>
    /// ステートの開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // カウンターリセット
        perceptionTimeCounter = 0;
        // 聞こえたコライダーの範囲をリセット
        animator.SetFloat("heardSoundRadius", 0);

        // 影人間のメッシュレンダラーを取得
        MeshRenderer[] kageMeshRenderers = animator.GetComponentsInChildren<MeshRenderer>();

        // 影人間の全マテリアルを黄色にする
        foreach (MeshRenderer meshRenderer in kageMeshRenderers)
        {
            foreach (Material material in meshRenderer.materials)
            {
                material.color = new Color(190,255,0,255);
                material.renderQueue = 4000;
            }
        }
    }

    /// <summary>
    // ステートの更新
    /// </summary>
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // 一定以上の音を聞いたら、警戒状態へ
        if (animator.GetFloat("heardSoundRadius") > loundVolumeRadiusMin)
        {
            animator.SetTrigger("loudVolumeNoise");
            perceptionTimeCounter = 0;
        }
        // 指定時間を超えたら、解除して通常に戻る
        if (perceptionTimeCounter > perceptionTime)
        {
            animator.SetBool("isVigilanceMode", false);
            perceptionTimeCounter = 0;
        }
        // カウンターリセット
        perceptionTimeCounter++;
    }
    
    /// <summary>
    /// ステートの終了
    /// </summary>
    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // カウンターリセット
        perceptionTimeCounter = 0;
        // トリガーリセット
        animator.ResetTrigger("perceiveSound");
        animator.ResetTrigger("loudVolumeNoise");
    }

}
