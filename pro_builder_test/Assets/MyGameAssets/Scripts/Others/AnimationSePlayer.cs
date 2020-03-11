using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// アニメーションに合わせてSEを鳴らす
/// </summary>
public class AnimationSePlayer : MonoBehaviour
{
    [SerializeField, LabelText("コピー元のAudioSourceリスト(詳細マウスオーバー"),
    Tooltip("指定元は普段非表示でよい。「PlayAudioByAnimation」をアニメーションイベントによって設定すると、intパラメータに指定された番号のAudioSourceがコピーされて音が生成される。(0開始)")]
    List<AudioSource> copyAudioSourceList = default;

    [SerializeField, LabelText("ランダムピッチ変化幅リスト(詳細マウスオーバー"),
        Tooltip("「PlayAudioByAnimation」で再生指定された音に反映されるピッチ変化のランダム幅。(0開始)\n" +
        "何も指定がなければピッチ変化しない。コピー元のAudioSourceリストと数を合わせる必要がある")]
    List<float> randomPitchParamList = default;

    // Start is called before the first frame update
    void Awake()
    {
        if(randomPitchParamList != null && copyAudioSourceList.Count != randomPitchParamList.Count)
        {
            Debug.LogError("ランダムピッチ変化幅リストはコピー元のAudioSourceリストの数と同じでなければなりません");
        }
    }

    /// <summary>
    /// オーディオからアニメーションする
    /// </summary>
    public void PlayAudioByAnimation(int audioIndex)
    {
        if(copyAudioSourceList == null || audioIndex >= copyAudioSourceList.Count || audioIndex < 0)
        {
            Debug.LogError("不正なパラメータでPlayAudioByAnimationが呼ばれたか、AnimationSePlayerの設定が不十分です。intパラメーター:"+audioIndex);
            return;
        }
        var playAudio = copyAudioSourceList[audioIndex];
        float randomPitch = (randomPitchParamList == null) ? 0 : randomPitchParamList[audioIndex];
        SePlayer.Inst.PlaySe(playAudio, 0, randomPitch);
    }
}
