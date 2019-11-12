using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵のパラメータのIDリストを管理
/// </summary>
[RequireComponent(typeof(Animator))]
public class EnemyParameterIdList : MonoBehaviour
{
    /// <summary>
    /// パラメータの種類
    /// </summary>
    public enum ParameterType
    {
        IsPlayerDiscover,
        IsPlayerSearching,
        AttackStart,
        IsAlert,
        IsFrenzy,
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
        for (int paramIndex = 0; paramIndex < animator.parameterCount; paramIndex++)
        {
            // パラメータを取得する
            AnimatorControllerParameter parameter = animator.parameters[paramIndex];
            // パラメータ名からハッシュ値を生成
            int hash = Animator.StringToHash(parameter.name);
            // ハッシュ値をidとしてセット（intからenumに型変換）
            idList.Add((ParameterType)paramIndex, hash);
        }
    }

    /// <summary>
    /// boolパラメータにフラグをセットする
    /// </summary>
    /// <param name="id"></param>
    /// <param name="value"></param>
    public void SetBool(ParameterType type,bool value)
    {
        // リストからパラメータIDを取得
        int paramId = IdList[type];
        // パラメーターにフラグをセットする
        animator.SetBool(paramId, value);
    }

    public void SetTrigger(ParameterType type)
    {
        // リストからパラメータIDを取得
        int paramId = IdList[type];
        // パラメーターにフラグをセットする
        animator.SetTrigger(paramId);
    }

    public AnimatorStateInfo GetCurrentAnimatorStateInfo(int layerIndex)
    {
        return animator.GetCurrentAnimatorStateInfo(layerIndex);
    }
}
