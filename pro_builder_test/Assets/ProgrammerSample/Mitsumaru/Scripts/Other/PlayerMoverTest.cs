using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// テスト用のプレイヤーの移動処理
/// （プレイヤーにみたてたオブジェクトを移動させるだけ）
/// </summary>
public class PlayerMoverTest : MonoBehaviour
{
    // 移動スピード
    [SerializeField]
    float speed = 0;

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        // 向き
        Vector3 direction = Vector3.zero;

        // キー入力
        if (Input.GetKey(KeyCode.UpArrow))    { direction.z++; }
        if (Input.GetKey(KeyCode.DownArrow))  { direction.z--; }
        if (Input.GetKey(KeyCode.LeftArrow))  { direction.x--; }
        if (Input.GetKey(KeyCode.RightArrow)) { direction.x++; }

        // 正規化
        direction = direction.normalized;
        // 向きに移動量を加える
        Vector3 velocity = direction * speed;
        // 移動する
        transform.position += velocity;
    }
}
