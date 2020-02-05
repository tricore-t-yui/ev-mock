using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 影人間のステート
/// </summary>
public class KageState
{
    /// <summary>
    /// ステートの種類
    /// </summary>
    public enum Kind
    {
        Normal,     // 通常状態
        Vigilance,  // 警戒状態
        Fight,      // 戦闘状態
        Attack,     // 攻撃
    }
}
