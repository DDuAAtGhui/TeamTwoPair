using BackEnd.Tcp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool isWin = false;
    public bool isDead = false;

    public Queue<GameObject> deadmen = new Queue<GameObject>();
    public Queue<SessionId> deadmenQueue = new Queue<SessionId>();

    public Dictionary<string, float> heightDic = new Dictionary<string, float>();

    Transform playerTF;

    [SerializeField] GameObject ending;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }

        else
            instance = this;

        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        playerTF = GameObject.FindWithTag("Player").transform;
        Instantiate(ending, transform.position, Quaternion.identity);
        ending.SetActive(false);
    }
    private void Update()
    {
        if (isWin)
        {
            ending.SetActive(true);
        }

        if (isDead)
        {

        }

        if (GameObject.Find("ServerManager"))
            InGame.GetInstance().SendData(playerTF.transform.position.y);


        else
            Debug.Log("서버매니저 씬에 존재하지않음");
    }
    public void GetMessage(string nickname, float height)
    {
        heightDic[nickname] = height;
    }
    public void DeadPlayer(SessionId sessionId)
    {
        deadmenQueue.Enqueue(sessionId);
    }
}
