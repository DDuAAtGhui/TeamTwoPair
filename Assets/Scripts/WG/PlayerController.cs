using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    [Header("Move info")]
    [SerializeField] float growSpeed = 1f;
    float initialGrowSpeed;
    [SerializeField] float damageLength = 10f;
    Vector2 moveDir;
    float initialDamageLength;

    [Header("Attack Info")]
    [SerializeField] float chargingTime = 1f;
    [SerializeField] float chargingTimer = 0f;
    [SerializeField] bool isFire = false;
    bool isCharging = false;
    bool isMaxCharging = false;
    bool isAttackAble = true;
    bool isDash = false;

    [Header("Components")]
    [SerializeField] HPController hpController;
    [SerializeField] Animator flowerAnim;


    void Start()
    {
        initialGrowSpeed = growSpeed;
        initialDamageLength = damageLength;

        StartCoroutine(ToggleAttackable());
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

        flowerAnim.SetBool("isCharging", isCharging);
        flowerAnim.SetBool("isMaxCharging", isMaxCharging);
        Debug.Log(isAttackAble);
    }

    void OnMove(InputValue value)
    {
        Vector2 dir = value.Get<Vector2>();
        moveDir = new Vector2(dir.x, dir.y);
    }

    void OnFire()
    {
        if (isDash)
            return;

        StartCoroutine(AttackTweak());
    }

    void OnFireUp()
    {
        isFire = false;
        isCharging = false;
        growSpeed = initialGrowSpeed;
    }

    IEnumerator AttackTweak()
    {
        if (isAttackAble)
        {
            isAttackAble = false;
            flowerAnim.SetBool("PRESS", true);
            chargingTimer = chargingTime;
            isMaxCharging = false;
            isFire = true;

            while (isFire)
            {
                isCharging = true;

                chargingTimer -= Time.deltaTime;
                chargingTimer = Mathf.Clamp(chargingTimer, 0, chargingTime);

                growSpeed = initialGrowSpeed * (chargingTimer / chargingTime);

                if (chargingTimer <= 0)
                {
                    isMaxCharging = true;
                }
                yield return null;
            }
        }
    }

    IEnumerator ToggleAttackable()
    {
        while (true)
        {
            yield return null;
            if (!isAttackAble)
            {
                isAttackAble = true;
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
