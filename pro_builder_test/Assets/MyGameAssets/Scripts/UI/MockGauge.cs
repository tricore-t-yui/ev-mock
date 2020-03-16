using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MockGauge : MonoBehaviour
{
    [SerializeField]
    CanvasGroup canvasGroup;

    [SerializeField]
    Image breathImage;

    [SerializeField]
    Image staminaImage;

    [SerializeField]
    playerStaminaController staminaController;

    [SerializeField]
    PlayerBreathController breathController;

    float fullTimeCount = 100.0f;
    float aimAlpha;

    private void Update()
    {
        var nowBreath = breathController.NowAmount;
        var nowStamina = staminaController.NowAmount;

        breathImage.rectTransform.localScale = new Vector3(nowBreath / 100, breathImage.rectTransform.localScale.y, breathImage.rectTransform.localScale.z);
        staminaImage.rectTransform.localScale = new Vector3(nowStamina / 100, staminaImage.rectTransform.localScale.y, staminaImage.rectTransform.localScale.z);
        if (nowBreath == 100 && nowBreath == 100)
        {
            fullTimeCount += Time.deltaTime;
        }
        else
        {
            fullTimeCount = 0;
        }
        if(fullTimeCount > 2.0f)
        {
            aimAlpha = 0.0f;
        }
        else
        {
            aimAlpha = 1.0f;
        }

        canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, aimAlpha, 0.1f);
    }
}
