using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    public enum KeyType
    {
        
    }

    [SerializeField]
    KeyCode dashKey = KeyCode.LeftShift;        // ダッシュキー
    [SerializeField]
    KeyCode squatKey = KeyCode.LeftCommand;     // しゃがみキー
    [SerializeField]
    KeyCode stealthKey = KeyCode.LeftControl;   // 忍び足キー
    [SerializeField]
    KeyCode deepBreathKey = KeyCode.Space;      // 深呼吸キー
    [SerializeField]
    KeyCode shoeshKey = KeyCode.V;              // 靴着脱キー
    [SerializeField]
    KeyCode breathReductionKey = KeyCode.Q;     // 息の消費軽減キー

    void GetKey(KeyType type)
    {
        if (Input.GetJoystickNames().Length == 0)
        {

        }
    }


    /// <summary>
    /// 方向キー検知
    /// </summary>
    /// <returns>方向キーのどれか１つが押されたかどうか</returns>
    public bool GetDirectionKey()
    {
        if ((Input.GetKey(KeyCode.W)) || (Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.S)) || (Input.GetKey(KeyCode.D)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
