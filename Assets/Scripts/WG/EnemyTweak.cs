using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTweak : MonoBehaviour
{
    [SerializeField] GameObject[] dummies;
    GameManager gameManager;
    private void Awake()
    {
        gameManager = GameManager.instance;
    }
    private void Start()
    {
        foreach (var dummy in dummies)
        {
            Instantiate(dummy, transform.position, Quaternion.identity, transform);
            dummy.SetActive(false);
        }
    }

    private void Update()
    {
        foreach (var player in gameManager.heightDic)
        {
            int i = 0;

            if (i > 0)
            {
                Debug.Log($"Player Nick : {player.Key}, Player Height : {player.Value}");

                transform.GetChild(i).gameObject.SetActive(true);
                transform.GetChild(i).transform.position =
                    new Vector3(transform.GetChild(i).transform.position.x,
                    player.Value, transform.GetChild(i).transform.position.z);
            }

            i++;
        }
    }
}
