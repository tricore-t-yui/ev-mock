using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キーの入力状態取得クラス
/// </summary>
public class KeyController : MonoBehaviour
{
    /// <summary>
    /// キーの種類
    /// </summary>
    public enum KeyType
    {
        LOOKBACK,       // 振り返り
        MOVE,           // 移動
        ENDUREBREATH,   // 息我慢
        INTERACT,       // インタラクト
        HOLDBREATH,     // 息止め
        OPTION,         // スタート画面
        SHOES,          // 靴
        SQUAT,          // しゃがみ
        DEEPBREATH,     // 深呼吸
        DASH,           // ダッシュ
        SAVE,           // セーブ
        LOOKINTO,       // 覗き込み
    }

    /// <summary>
    /// スティックのタイプ
    /// </summary>
    public enum StickType
    {
        RIGHTSTICK,     // 右スティック
        LEFTSTICK,      // 左スティック
    }

    [SerializeField]
    bool isUseController = false;   // コントローラーを使うかどうか

    /// <summary>
    /// キー取得
    /// </summary>
    /// <param name="type">キーの種類</param>
    public bool GetKey(KeyType type)
    {
        if (isUseController)
        {
            return GetControllerButton(type);
        }
        else
        {
            return GetKeyboardKey(type);
        }
    }

    /// <summary>
    /// キーボード用キー取得
    /// </summary>
    /// <param name="type">キーの種類</param>
    bool GetKeyboardKey(KeyType type)
    {
        switch (type)
        {
            case KeyType.LOOKBACK: if (Input.GetKey(KeyCode.Q)) { return true; } break;
            case KeyType.MOVE: return GetDirectionKey();
            case KeyType.ENDUREBREATH: if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.R)) { return true; } break;
            case KeyType.INTERACT: if (Input.GetMouseButton(0)) { return true; } break;
            case KeyType.HOLDBREATH: if (Input.GetMouseButton(1)) { return true; } break;
            case KeyType.OPTION: if (Input.GetKey(KeyCode.P)) { return true; } break;
            case KeyType.SHOES: if (Input.GetKey(KeyCode.Space)) { return true; } break;
            case KeyType.SQUAT: if (Input.GetKey(KeyCode.C)) { return true; } break;
            case KeyType.DEEPBREATH: if (Input.GetKey(KeyCode.LeftControl)) { return true; } break;
            case KeyType.DASH: if (Input.GetKey(KeyCode.LeftShift)) { return true; } break;
            case KeyType.LOOKINTO: if (Input.GetKey(KeyCode.T)) { return true; } break;
        }

        return false;
    }

    /// <summary>
    /// コントローラー用ボタン取得
    /// </summary>
    /// <param name="type">キーの種類</param>
    bool GetControllerButton(KeyType type)
    {
        switch (type)
        {
            case KeyType.LOOKBACK: if (Input.GetKey("joystick button 8")) { return true; } break;
            case KeyType.MOVE: return GetDirectionKey();
            case KeyType.ENDUREBREATH: if (Input.GetKey("joystick button 4") && Input.GetKey("joystick button 5")) { return true; } break;
            case KeyType.INTERACT: if (Input.GetKey("joystick button 0")) { return true; } break;
            case KeyType.HOLDBREATH: if (Input.GetKey("joystick button 4")) { return true; } break;
            case KeyType.OPTION: if (Input.GetKey("joystick button 7")) { return true; } break;
            case KeyType.SHOES: if (Input.GetKey("joystick button 2")) { return true; } break;
            case KeyType.SQUAT: if (Input.GetKey("joystick button 1")) { return true; } break;
            case KeyType.DEEPBREATH: if (Input.GetKey("joystick button 3")) { return true; } break;
            case KeyType.DASH: if (Input.GetKey("joystick button 5")) { return true; } break;
            case KeyType.SAVE: if (Input.GetKey("joystick button 6")) { return true; } break;
            case KeyType.LOOKINTO: if (Input.GetKey("joystick button 9")) { return true; } break;
        }

        return false;
    }

    /// <summary>
    /// 方向キー検知
    /// </summary>
    /// <returns>方向キーのどれか１つが押されたかどうか</returns>
    public bool GetDirectionKey()
    {
        if (isUseController)
        {
            if (((Input.GetAxis("L_Stick_H") != 0) || (Input.GetAxis("L_Stick_V") != 0)) || ((Input.GetAxis("D_Pad_H") != 0) || (Input.GetAxis("D_Pad_V") != 0)))
            {
                return true;
            }
        }
        else
        {
            if ((Input.GetKey(KeyCode.W)) || (Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.S)) || (Input.GetKey(KeyCode.D)))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// スティックの入力加減を取得
    /// </summary>
    public Vector2 GetStick(StickType type)
    {
        if (isUseController)
        {
            switch (type)
            {
                case StickType.RIGHTSTICK: return new Vector2(Input.GetAxis("R_Stick_H"), Input.GetAxis("R_Stick_V"));
                case StickType.LEFTSTICK:
                    if (Input.GetAxis("L_Stick_H") == 0 && Input.GetAxis("L_Stick_V") == 0)
                    {
                        return new Vector2(Input.GetAxis("D_Pad_H"), Input.GetAxis("D_Pad_V"));
                    }
                    else
                    {
                        return new Vector2(Input.GetAxis("L_Stick_H"), Input.GetAxis("L_Stick_V"));
                    }
            }
        }
        else
        {
            switch (type)
            {
                case StickType.RIGHTSTICK: return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
                case StickType.LEFTSTICK: return GetKeyboard();
            }
        }

        return Vector2.zero;
    }

    /// <summary>
    /// キーボードの入力加減
    /// </summary>
    Vector2 GetKeyboard()
    {
        float x = 0;
        float y = 0;

        if (Input.GetKey(KeyCode.W))
        {
            y += 1;
        }
        // 後ろ移動
        else if (Input.GetKey(KeyCode.S))
        {
            y -= 1;
        }

        // 左移動
        if (Input.GetKey(KeyCode.A))
        {
            x -= 1;
        }
        // 右移動
        else if (Input.GetKey(KeyCode.D))
        {
            x += 1;
        }
        return new Vector2(x, y);
    }

    /// <summary>
    /// コントローラーをつかうかどうか
    /// </summary>
    public bool GetIsUseController()
    {
        return isUseController;
    }
}
