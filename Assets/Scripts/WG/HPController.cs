using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPController : MonoBehaviour
{
    [SerializeField] float maxHP = 100.0f;
    float initialHP;
    [SerializeField] float currentHP = 0;
    [SerializeField] float autoRecoveryHp = 0.9f;
    void Start()
    {
        currentHP = maxHP;
        initialHP = maxHP;
    }

    private void Update()
    {
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        maxHP = Mathf.Clamp(maxHP, 0, 800);
        if (currentHP < maxHP)
        {
            //현재 체력 재생
            currentHP += autoRecoveryHp * Time.deltaTime;
        }
      //  Debug.Log("maxHP : " + maxHP);
    }


    public float MaxHP { get { return maxHP; } set { maxHP = value; } }
    public float CurrentHP { get { return currentHP; } set { currentHP = value; } }
    public float InitialHP { get { return initialHP; } }

}
