using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShadowControl : MonoBehaviour
{
    public bool isInvisible = false;

    private void OnBecameInvisible()
    {
        isInvisible = true;

        ObjectPool.instance.objectsQueue.Enqueue(gameObject);
        gameObject.SetActive(false);

    }
}
