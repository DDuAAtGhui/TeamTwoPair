using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    Vector2 moveDir;
    [SerializeField] float growSpeed = 1f;

    void Start()
    {
    }
    void Update()
    {
        if (moveDir != Vector2.zero)
            transform.position += (Vector3)moveDir * Time.deltaTime * growSpeed;
    }

    void OnMove(InputValue value)
    {
        Vector2 dir = value.Get<Vector2>();
        moveDir = new Vector2(dir.x, dir.y);
    }
}
