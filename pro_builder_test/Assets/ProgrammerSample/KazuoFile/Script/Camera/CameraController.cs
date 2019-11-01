using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カメラクラス
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField]
    Transform player = default;     // プレイヤー

    [SerializeField]
    float sensitivity = default;    // カメラの感度

    Vector3 squatPos = new Vector3(0, -0.5f, 0);    // しゃがんだ時のカメラの位置

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        float X_Rotation = Input.GetAxis("Mouse X") * sensitivity;
        float Y_Rotation = Input.GetAxis("Mouse Y") * sensitivity;
        player.transform.Rotate(0, X_Rotation, 0);
        transform.Rotate(-Y_Rotation, 0, 0);
    }
}
