using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager instance;

    [SerializeField] List<GameObject> instantiateOnLoad;

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
        if (instantiateOnLoad.Count <= 0)
            return;

        foreach (GameObject go in instantiateOnLoad)
        {
            Instantiate(go);
        }
    }
}
