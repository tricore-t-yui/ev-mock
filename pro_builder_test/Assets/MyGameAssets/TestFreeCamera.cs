using UnityEngine;
using Rewired;

/// <summary>
/// GameビューにてSceneビューのようなカメラの動きをマウス操作によって実現する
/// </summary>
[RequireComponent(typeof(Camera))]
public class TestFreeCamera : MonoBehaviour
{
    [SerializeField, Range(0.1f, 10f)]
    private float moveSpeed = 0.3f;

    [SerializeField, Range(0.1f, 10f)]
    private float rotateSpeed = 0.3f;

    Player player;

    private void Start()
    {
        player = ReInput.players.GetPlayer(0);
    }

    private void Update()
    {
        // pos
        var CameraMoveV = player.GetAxis("CameraMoveV");
        var CameraMoveH = player.GetAxis("CameraMoveH");
        var CameraRotV = player.GetAxis("CameraRotV");
        var CameraRotH = player.GetAxis("CameraRotH");
        var CameraMoveZ = player.GetAxis("CameraMoveZ");

        var cameraMove = new Vector3(CameraMoveH, CameraMoveV, CameraMoveZ);
        var cameraRot = new Vector2(-CameraRotV, CameraRotH);

        transform.Translate(cameraMove * Time.deltaTime * moveSpeed);

        // rot コントローラー接続されていなかったらマウス扱いして原則。コントローラー使ってマウス動かしたらはやいけどしらん
        var controllerNames = Input.GetJoystickNames();
        if(controllerNames.Length <= 0 || controllerNames[0] == "")
        {
            cameraRot *= 0.05f;
        }
        CameraRotate(cameraRot * rotateSpeed);
    }

    public void CameraRotate(Vector2 angle)
    {
        transform.RotateAround(transform.position, transform.right, angle.x);
        transform.RotateAround(transform.position, Vector3.up, angle.y);
    }
}