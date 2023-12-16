using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    HPController hpController;

    [SerializeField] Image currentHpImage;
    [SerializeField] Image maxHpImage;

    void Awake()
    {
        hpController = GameObject.FindObjectOfType<HPController>();

    }

    void Update()
    {
        ImageControll();
    }

    private void ImageControll()
    {
        currentHpImage.fillAmount = hpController.CurrentHP / (float)hpController.MaxHP;

        currentHpImage.GetComponent<RectTransform>()
            .SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, hpController.MaxHP * 4);

        maxHpImage.GetComponent<RectTransform>()
            .SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, hpController.MaxHP * 4);
    }
}
