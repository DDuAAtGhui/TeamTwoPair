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
        foreach (var player in gameManager.heightDic)
        {
            int i = 0;


            Instantiate(dummies[i], transform.position, Quaternion.identity, transform);





            i++;
        }
    }

    private void Update()
    {
        foreach (var player in gameManager.heightDic)
        {
            Debug.Log($"Player Nick : {player.Key}, Player Height : {player.Value}");
        }
    }
}
