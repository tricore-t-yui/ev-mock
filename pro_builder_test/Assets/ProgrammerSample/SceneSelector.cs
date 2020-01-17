using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// シーン選択
/// </summary>
public class SceneSelector : MonoBehaviour
{
    [SerializeField]
    Button goingButton = default;

    [SerializeField]
    Button returnButton = default;

    [SerializeField]
    Button invincibleSwitcher = default;

    [SerializeField]
    Text invincibleText = default;

    bool isInvincible = true;
    string prefsKey = "isInvincibleKey";

    void Start()
    {
        isInvincible = (PlayerPrefs.GetInt("isInvincibleKey") == 1) ? true : false;
    }

    void Update()
    {
        if (isInvincible)
        {
            invincibleText.text = "isInvincible : true";
        }
        else
        {
            invincibleText.text = "isInvincible : false";
        }
    }

    public void OnGoingScene()
    {
        SceneManager.LoadScene("GoingScene_1.2");
    }

    public void OnReturnScene()
    {
        SceneManager.LoadScene("ReturnScene_1.2");
    }

    public void OnInvincibleSwitcher()
    {
        if (isInvincible)
        {
            isInvincible = false;
        }
        else
        {
            isInvincible = true;
        }
        PlayerPrefs.SetInt(prefsKey, isInvincible ? 1 : 0);
    }
}
