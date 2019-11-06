using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カメラクラス
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField]
    Transform player = default;                     // プレイヤー
    [SerializeField]
    PlayerHideController hideController = default;  // 隠れるアクション管理クラス
    [SerializeField]
    float sensitivity = default;                    // カメラの感度

    /// <summary>
    /// 起動処理
    /// </summary>
    void OnEnable()
    {
        // 起動時はプレイヤーの正面を向く
        transform.Rotate(0, 0, 0);
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        float Y_Rotation = Input.GetAxis("Mouse Y") * sensitivity;
        float X_Rotation = Input.GetAxis("Mouse X") * sensitivity;

        // ベッドに隠れている時の視点移動
        if (hideController.IsHideBed || hideController.IsHideLocker)
        {
            player.transform.Rotate(0, 0, -X_Rotation);
        }
        // それ以外の視点移動
        else
        {
            player.transform.Rotate(0, X_Rotation, 0);
            transform.Rotate(-Y_Rotation, 0, 0);
        }
    }
}
