using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager instance;

    [SerializeField] List<GameObject> instantiateOnLoad;

    public bool isWin = false;

    public Queue<GameObject> deadmen = new Queue<GameObject>();

    public Dictionary<string, float> heightDic = new Dictionary<string, float>();

    Transform playerTF;

    public static GameManager Instance
    {
        get
        {
            if (instance != null)
            {
                Destroy(instance.gameObject);
            }
            else
            {
                instance = new GameManager();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
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

        if (GameObject.Find("ServerManager"))
            InGame.GetInstance().SendData(playerTF.position.y);

        else
            Debug.Log("�����Ŵ��� ���� ������������");
    }
}
