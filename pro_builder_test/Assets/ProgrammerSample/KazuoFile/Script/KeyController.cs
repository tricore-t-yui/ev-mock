﻿using System.Collections;
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
        LOOKBACK,           // 振り返り
        MOVE,               // 移動
        LEFTENDUREBREATH,   // 左息我慢
        RIGHTENDUREBREATH,  // 右息我慢
        INTERACT,           // インタラクト
        HOLDBREATH,         // 息止め
        OPTION,             // スタート画面
        SHOES,              // 靴
        SQUAT,              // しゃがみ
        DEEPBREATH,         // 深呼吸
        DASH,               // ダッシュ
        SAVE,               // セーブ
        LOOKINTO,           // 覗き込み
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
    public bool GetKeyUp(KeyType type)
    {
        if (isUseController)
        {
            return Input.GetKeyUp(GetControllerButton(type));
        }
        else
        {
            if (type == KeyType.INTERACT)
            {
                return Input.GetMouseButtonUp(0);
            }
            return Input.GetKeyUp(GetKeyboardKey(type));
        }
    }

    /// <summary>
    /// キー取得
    /// </summary>
    /// <param name="type">キーの種類</param>
    public bool GetKey(KeyType type)
    {
        if (isUseController)
        {
            switch (type)
            {
                case KeyType.HOLDBREATH: if (Input.GetAxis("L_R_Trigger") > 0) { return true; } break;
                case KeyType.MOVE: return GetDirectionKey();
                case KeyType.DASH: if (Input.GetAxis("L_R_Trigger") < 0) { return true; } break;
                default: if (Input.GetKey(GetControllerButton(type))) { return true; } break;
            }
            return false;
        }
        else
        {
            switch (type)
            {
                case KeyType.MOVE: return GetDirectionKey();
                case KeyType.INTERACT: if (Input.GetMouseButton(0)) { return true; } break;
                case KeyType.HOLDBREATH: if (Input.GetMouseButton(1)) { return true; } break;
                default: if (Input.GetKey(GetKeyboardKey(type))) { return true; } break;
            }
            return false;
        }
    }

    /// <summary>
    /// キー取得
    /// </summary>
    /// <param name="type">キーの種類</param>
    public bool GetKeyDown(KeyType type)
    {
        if (isUseController)
        {
            return Input.GetKeyDown(GetControllerButton(type));
        }
        else
        {
            if(type == KeyType.INTERACT)
            {
                return Input.GetMouseButtonDown(0);
            }
            return Input.GetKeyDown(GetKeyboardKey(type));
        }
    }

    /// <summary>
    /// キーボード用キー取得
    /// </summary>
    /// <param name="type">キーの種類</param>
    KeyCode GetKeyboardKey(KeyType type)
    {
        switch (type)
        {
            case KeyType.LOOKBACK: return KeyCode.Q;
            case KeyType.OPTION: return KeyCode.P;
            case KeyType.SHOES: return KeyCode.Space;
            case KeyType.SQUAT: return KeyCode.C;
            case KeyType.DEEPBREATH: return KeyCode.LeftControl;
            case KeyType.DASH: return KeyCode.LeftShift;
            case KeyType.LOOKINTO: return KeyCode.T;
            case KeyType.LEFTENDUREBREATH: return KeyCode.E;
            case KeyType.RIGHTENDUREBREATH: return KeyCode.R;
        }

        Debug.Log("対象外のキー");
        return default;
    }

    /// <summary>
    /// コントローラー用ボタン取得
    /// </summary>
    /// <param name="type">キーの種類</param>
    string GetControllerButton(KeyType type)
    {
        switch (type)
        {
            case KeyType.LOOKBACK: return "joystick button 8";
            case KeyType.INTERACT: return "joystick button 0";
            case KeyType.OPTION: return "joystick button 7";
            case KeyType.SHOES: return "joystick button 2";
            case KeyType.SQUAT: return "joystick button 1";
            case KeyType.DEEPBREATH: return "joystick button 3";
            case KeyType.SAVE: return "joystick button 6";
            case KeyType.LOOKINTO: return "joystick button 9";
            case KeyType.LEFTENDUREBREATH: return "joystick button 4";
            case KeyType.RIGHTENDUREBREATH: return "joystick button 5";
        }

        Debug.Log("対象外のボタン");
        return default;
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
