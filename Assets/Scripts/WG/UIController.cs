using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMeshProUGUI;

    private void Update()
    {
        textMeshProUGUI.text = GameObject.FindWithTag("Player").transform.position.y.ToString("F1") + "M";
    }
}
