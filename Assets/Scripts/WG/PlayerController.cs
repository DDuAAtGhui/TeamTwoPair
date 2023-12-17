using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    [Header("Move info")]
    [SerializeField] public float growSpeed = 1f;
    float initialGrowSpeed;
    [SerializeField] float damageLength = 0.1f;
    [SerializeField] float damageMultiplier = 10f;
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

    public CustomInputAction inputActions;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputActions = new CustomInputAction();

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
            if (moveDir.x != 0 && moveDir.y != 0)
            {
                moveDir = new Vector2(0, 1);
            }

            if (moveDir.y < 0)
                return;


            float angle = Mathf.Atan2(-moveDir.x, moveDir.y) * Mathf.Rad2Deg;
            flower.transform.rotation = Quaternion.Euler(0, 0, angle);

            transform.position += (Vector3)moveDir * Time.deltaTime * growSpeed;

            damageLength -= Time.deltaTime;

            if (damageLength <= 0)
            {
                float hpDecrease = damageMultiplier / (hpController.InitialHP * 1.25f / hpController.MaxHP);

                hpDecrease = Mathf.Clamp(hpDecrease, damageMultiplier, float.MaxValue);

                hpController.MaxHP -= hpDecrease;
                damageLength = initialDamageLength;
            }
        }



        flowerAnim.SetBool("isCharging", isCharging);
        flowerAnim.SetBool("isMaxCharging", isMaxCharging);
        // Debug.Log(hpController.MaxHP);
    }

    void OnMove(InputValue value)
    {
        if (isDash || GameManager.instance.isDead)
            return;

        Vector2 dir = value.Get<Vector2>();
        moveDir = new Vector2(dir.x, dir.y);
    }

    void OnFire()
    {
        if (isDash || GameManager.instance.isDead)
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
        if (GameManager.instance.isDead)
            return;

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
                yield return new WaitForSeconds(0.2f);
                isAttackAble = true;
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

            transform.position += (Vector3)moveDir * Time.deltaTime * 50f;


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
