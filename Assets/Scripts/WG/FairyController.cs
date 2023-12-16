using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyController : MonoBehaviour
{
    int playerEncounter = 0;
    bool runAway = false;

    [SerializeField] float speed = 12f;
    [SerializeField] float flyUpTime = 2f;
    [SerializeField] float rotateSpeed = 6f;
    Collider2D coll;
    private void Awake()
    {
        coll = GetComponent<Collider2D>();
    }
    private void Start()
    {
        StartCoroutine(Rotate());
    }
    private void Update()
    {
        if (playerEncounter >= 3)
            runAway = true;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerEncounter++;

            StartCoroutine(FlyUp());
        }
    }

    IEnumerator FlyUp()
    {
        float timer = flyUpTime;
        float speed = this.speed;

        coll.enabled = false;

        while (true)
        {
            yield return null;
            transform.position += Vector3.up * speed * Time.deltaTime;

            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                speed = 0f;
                break;
            }
        }
        coll.enabled = true;
    }

    IEnumerator Rotate()
    {
        while (coll.enabled)
        {
            yield return null;

            transform.RotateAround(new Vector2(-6f, 18f), Vector3.forward, rotateSpeed);
        }
    }
}
