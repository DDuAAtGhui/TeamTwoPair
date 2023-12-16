using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    Vector2 moveDir;
    [SerializeField] float growSpeed = 1f;

    [SerializeField] float damageLength = 10f;
    float initialDamageLength;

    [SerializeField] HPController hpController;
    void Start()
    {
        initialDamageLength = damageLength;
    }
    void Update()
    {
        if (moveDir != Vector2.zero)
        {
            transform.position += (Vector3)moveDir * Time.deltaTime * growSpeed;

            damageLength -= Time.deltaTime;

            if (damageLength <= 0)
            {
                damageLength = initialDamageLength;
                hpController.MaxHP -= 10;
            }
        }
    }

    void OnMove(InputValue value)
    {
        Vector2 dir = value.Get<Vector2>();
        moveDir = new Vector2(dir.x, dir.y);
    }
}
