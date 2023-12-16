using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeEnemy : MonoBehaviour
{
    Rigidbody2D playerRb;
    Rigidbody2D myRb;
    PlayerController playerController;
    [SerializeField] float speed = 10f;
    private void Awake()
    {
        playerRb = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
        myRb = GetComponent<Rigidbody2D>();
        playerController = GameObject.FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;
    }
}
