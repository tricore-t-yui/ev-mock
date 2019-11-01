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
    /// 更新処理
    /// </summary>
    void Update()
    {
        float Y_Rotation = Input.GetAxis("Mouse Y") * sensitivity;
        float X_Rotation = Input.GetAxis("Mouse X") * sensitivity;

        // ベッドに隠れている時の視点移動
        if (hideController.IsHideBed)
        {
            player.transform.Rotate(0, 0, -X_Rotation);
            transform.Rotate(0, Y_Rotation, 0);
        }
        // それ以外の視点移動
        else
        {
            player.transform.Rotate(0, X_Rotation, 0);
            transform.Rotate(-Y_Rotation, 0, 0);
        }
    }
}
