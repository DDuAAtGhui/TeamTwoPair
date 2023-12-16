using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMeshProUGUI;
    [SerializeField] GameObject fadeoutPanel;
    private void Update()
    {
        textMeshProUGUI.text = GameObject.FindWithTag("Player").transform.position.y.ToString("F1") + "M";

        if (GameManager.instance.isDead)
        {
            fadeoutPanel.GetComponent<Image>().enabled = true;
            fadeoutPanel.GetComponent<DOTweenAnimation>().DOPlay();
        }

        else
        {
            fadeoutPanel.GetComponent<Image>().enabled = false;
            fadeoutPanel.GetComponent<DOTweenAnimation>().DOPause();
        }
    }

    public void SceneLoad(int index)
    {
        SceneManager.LoadScene(index);
    }
}
