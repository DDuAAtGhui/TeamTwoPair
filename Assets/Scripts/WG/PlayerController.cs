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
    [HideInInspector] public static Vector2 moveDir;
    float initialDamageLength;

    [Header("Attack Info")]
    [SerializeField] float chargingTime = 1f;
    [SerializeField] float chargingTimer = 0f;
    [SerializeField] bool isFire = false;
    bool isCharging = false;
    bool isMaxCharging = false;
    bool isAttackAble = true;
    [SerializeField] bool isDash = false;
    [SerializeField] bool isDashAble = true;
    [SerializeField] float dashTimer = 0.1f;

    [Header("Components")]
    [SerializeField] HPController hpController;
    [SerializeField] GameObject flower;
    [SerializeField] Animator flowerAnim;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
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
            float angle = Mathf.Atan2(-moveDir.x, moveDir.y) * Mathf.Rad2Deg;
            flower.transform.rotation = Quaternion.Euler(0, 0, angle);

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
    }

    void OnMove(InputValue value)
    {
        if (isDash)
            return;

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

    void OnDash()
    {
        if (hpController.CurrentHP > 0 && isDashAble)
        {
            StartCoroutine(Dash());
            hpController.CurrentHP -= 10;
        }
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
    IEnumerator Dash()
    {
        isDash = true;

        while (isDash)
        {
            yield return null;
            isDashAble = false;
            dashTimer -= Time.deltaTime;

            transform.position += transform.position.normalized * Time.deltaTime * 50f;


            if (dashTimer <= 0)
            {
                isDash = false;
                dashTimer = 0.1f;
            }
        }

        StartCoroutine(ToggleDashAble());
    }
    IEnumerator ToggleDashAble()
    {
        yield return new WaitForSeconds(1f);
        isDashAble = true;
    }

}
