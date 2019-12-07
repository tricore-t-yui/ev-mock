using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 敵のパラメータのIDリストを管理
/// </summary>
[RequireComponent(typeof(Animator))]
public class KageAnimParameterList : MonoBehaviour
{
    /// <summary>
    /// パラメータの種類
    /// NOTE : Animatorのパラメータ名とEnumの項目名で比較して格納しており
    ///        大文字と小文字も区別しているためローワーキャメルケースを使用
    /// </summary>
    public enum ParameterType
    {
        normalBehaviourKindId,
        standingMotionKindId,
        loiteringKindId,
        isLoiteringMove,
        otherActionKindId,
        loiteringOtherActionStart,
        loiteringBehaviorRate,
        isVigilanceMode,
        isFightingMode,
        targetPositionX,
        targetPositionY,
        targetPositionZ,
        perceiveSound,
        targetPosStop,
        isAttack,
        isAttackFromLocker,
        isAttackFromBed,
        isReturnPoint,
        prevStateKindId,
        currentStateKindId,
    }

    // アニメーター
    [SerializeField]
    Animator animator = default;

    // キーとセットになったIDにのリスト
    Dictionary<ParameterType, int> idList = new Dictionary<ParameterType, int>();
    // 読み取り専用のIDリスト
    public IReadOnlyDictionary<ParameterType, int> IdList => idList;

    /// <summary>
    /// 開始
    /// </summary>
    void OnEnable()
    {
        foreach (AnimatorControllerParameter animParam in animator.parameters)
        {
            // パラメータ名からハッシュ値を生成
            int paramHash = Animator.StringToHash(animParam.name);
            // パラメータ名から、Enumオブジェクトを取得
            object type = System.Enum.Parse(typeof(ParameterType), animParam.name);
            // Enumオブジェクトとハッシュ値をセットで格納する
            idList.Add((ParameterType)type, paramHash);
        }
    }

    /// <summary>
    /// boolパラメータにフラグをセットする
    /// </summary>
    public void SetBool(ParameterType type,bool value)
    {
        // リストからパラメータIDを取得
        int paramId = IdList[type];
        // パラメーターにフラグをセットする
        animator.SetBool(paramId, value);
    }

    public bool GetBool(ParameterType type)
    {
        // リストからパラメータIDを取得
        int paramId = IdList[type];
        // パラメーターにフラグをセットする
        return animator.GetBool(paramId);
    }

    /// <summary>
    /// トリガーをセット
    public void SetTrigger(ParameterType type)
    {
        // リストからパラメータIDを取得
        int paramId = IdList[type];
        // パラメータのトリガーをオンにする
        animator.SetTrigger(paramId);
    }

    /// <summary>
    /// トリガーをリセット
    public void ResetTrigger(ParameterType type)
    {
        // リストからパラメータIDを取得
        int paramId = IdList[type];
        // パラメータのトリガーをオンにする
        animator.ResetTrigger(paramId);
    }

    /// <summary>
    /// int型の値をセット
    /// </summary>
    public void SetInteger(ParameterType type,int value)
    {
        // リストからパラメータIDを取得
        int paramId = IdList[type];
        // パラメーターに値をセットする
        animator.SetInteger(paramId, value);
    }

    /// <summary>
    /// int型の値を取得
    /// </summary>
    public int GetInteger(ParameterType type)
    {
        // リストからパラメータIDを取得
        int paramId = IdList[type];
        // パラメーターに値を取得する
        return animator.GetInteger(paramId);
    }

    /// <summary>
    /// float型の値をセット
    /// </summary>
    public void SetFloat(ParameterType type, float value)
    {
        // リストからパラメータIDを取得
        int paramId = IdList[type];
        // パラメーターに値をセットする
        animator.SetFloat(paramId, value);
    }

    /// <summary>
    /// float型の値を取得
    /// </summary>
    public float GetFloat(ParameterType type)
    {
        // リストからパラメータIDを取得
        int paramId = IdList[type];
        // パラメーターに値を取得する
        return animator.GetFloat(paramId);
    }

    /// <summary>
    /// 終了処理
    /// </summary>
    private void OnDisable()
    {
        // リストの内容を削除
        idList.Clear();
    }
}
