using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class ObjectInfo
{
    public GameObject goPrefab;
    public int count;
}
public class ObjectPool : MonoBehaviour
{
    [SerializeField] ObjectInfo[] objectInfos;

    public static ObjectPool instance;

    public Queue<GameObject> objectsQueue = new Queue<GameObject>();

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;

        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        objectsQueue = InsertQueue(objectInfos[0]);
    }

    Queue<GameObject> InsertQueue(ObjectInfo objectInfo)
    {
        Queue<GameObject> temp_queue = new Queue<GameObject>();

        for (int i = 0; i < objectInfo.count; i++)
        {
            GameObject go = Instantiate(objectInfo.goPrefab, transform.position, Quaternion.identity);
            go.SetActive(false);

            temp_queue.Enqueue(go);
        }

        return temp_queue;
    }
}
