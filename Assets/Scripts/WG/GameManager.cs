using BackEnd.Tcp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] List<GameObject> instantiateOnLoad;

    public bool isWin = false;

    public Queue<GameObject> deadmen = new Queue<GameObject>();
    public Queue<SessionId> deadmenQueue = new Queue<SessionId>();

    public Dictionary<string, float> heightDic = new Dictionary<string, float>();

    Transform playerTF;

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

        if (instantiateOnLoad.Count <= 0)
            return;

        foreach (GameObject go in instantiateOnLoad)
        {
            Instantiate(go);
        }
    }
    private void Update()
    {
        if (isWin)
        {

        }

        if (GameObject.Find("ServerManager"))
            StartCoroutine(SendDataRoutine());


        else
            Debug.Log("서버매니저 씬에 존재하지않음");
    }
    IEnumerator SendDataRoutine()
    {
        InGame.GetInstance().SendData(playerTF.position.y);
        yield return new WaitForSeconds(0.5f);
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
