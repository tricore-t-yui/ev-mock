using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MockStartScene : MonoBehaviour
{
    [SerializeField]
    Text loadUi = default;
    [SerializeField]
    float speed = 0.02f;
    float count = 0;
    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(LoadCoroutine());
        // 一台もコントローラが接続されていなければエラー
        var controllerNames = Input.GetJoystickNames();
        if (controllerNames.Length == 0 || controllerNames[0] == "")
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
    private void Update()
    {
        if(loadUi)
        {
            var alpha = 0.5f + Mathf.Sin(count) * 0.5f;
            count += speed;
            loadUi.color = new Color(loadUi.color.r, loadUi.color.g, loadUi.color.b, alpha);
        }
    }
    IEnumerator LoadCoroutine()
    {
        AsyncOperation load;
        load = SceneManager.LoadSceneAsync("HorrorMock_03_root", LoadSceneMode.Additive);
        while (!load.isDone) { yield return null; }
        SceneManager.LoadScene("HorrorMock_03_child00", LoadSceneMode.Additive);
        load = SceneManager.LoadSceneAsync("HorrorMock_03_child01", LoadSceneMode.Additive);
        while (!load.isDone) { yield return null; }
        load = SceneManager.LoadSceneAsync("HorrorMock_03_child02", LoadSceneMode.Additive);
        while (!load.isDone) { yield return null; }
        var root = SceneManager.GetSceneByName("HorrorMock_03_root");
        SceneManager.SetActiveScene(root);
        Shader.WarmupAllShaders();
        Destroy(loadUi.transform.parent.gameObject);
        loadUi = null;
    }
}
