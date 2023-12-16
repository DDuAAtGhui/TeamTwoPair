using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    HPController hpController;
    Image image;
    void Awake()
    {
        image = GetComponent<Image>();
        hpController = GameObject.FindObjectOfType<HPController>();

    }

    void Update()
    {

        image.fillAmount = hpController.CurrentHP / (float)hpController.MaxHP;
    }

}
