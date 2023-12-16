using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPController : MonoBehaviour
{
    float maxHP = 100.0f;
    [SerializeField] float currentHP = 0;
    [SerializeField] float autoRecoveryHp = 0.9f;
    void Start()
    {
        currentHP = maxHP;
    }

    private void Update()
    {
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        if (currentHP < maxHP)
        {
            //현재 체력 재생
            currentHP += autoRecoveryHp * Time.deltaTime;
        }
    }


    public float MaxHP { get { return maxHP; } set { maxHP = value; } }
    public float CurrentHP { get { return currentHP; } set { currentHP = value; } }

}
