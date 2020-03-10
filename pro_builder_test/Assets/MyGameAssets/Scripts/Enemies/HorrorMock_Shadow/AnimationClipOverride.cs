using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class ClipParameter
{
    public ClipParameter(string animName,string clipName)
    {
        animationName = animName;
        oldClipName = clipName;
    }

    bool isActivate = false;
    [EnableIf("isActivate")]
    [Tooltip("アニメーションの名前です。" +
        "\nどのアニメーションか分かるように用意しています。")]
    public string animationName = null;
    [EnableIf("isActivate")]
    [Tooltip("もともとのアニメーションのファイル名です。" +
        "\nこのアニメーションが置き換わります。")]
    public string oldClipName = null;
    [Tooltip("新しく置き換えられるアニメーションです。" +
        "\nこのアニメーションに置き換えられます。")]
    public AnimationClip newClip = null;
}

[TypeInfoBox("ここでは敵に使用されているアニメーションを別のものに置き換えることができます。" +
    "\n\"Assets\"から\"New Clip\"にアニメーションをアタッチします。" +
    "\n何もアタッチされていない場合はもともとのアニメーションが再生されます。")]
public class AnimationClipOverride : MonoBehaviour
{
    // オーバーライドコントローラー
    [SerializeField]
    Animator anim = default;
    AnimatorOverrideController overrideController = default;

    // クリップのパラメータリスト
    [SerializeField]
    ClipParameter[] clipParameters = default;

    /// <summary>
    /// 開始
    /// </summary>
    void Start()
    {
        overrideController = new AnimatorOverrideController();
        overrideController.name = "overrideController";
        overrideController.runtimeAnimatorController = anim.runtimeAnimatorController;
        anim.runtimeAnimatorController = overrideController;
        foreach (ClipParameter parameter in clipParameters)
        {
            // アニメーションを置き換える
            overrideController[parameter.oldClipName] = parameter.newClip;
        }
        anim.Update(0);
    }
}
