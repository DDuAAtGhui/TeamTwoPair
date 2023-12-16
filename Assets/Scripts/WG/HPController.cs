using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPController : MonoBehaviour
{
    int maxHP = 100;
    [SerializeField] int currentHP = 0;

    void Start()
    {
        currentHP = maxHP;
    }

    private void Update()
    {
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
    }


    public int MaxHP { get { return maxHP; } set { maxHP = value; } }
    public int CurrentHP { get { return currentHP; } set { currentHP = value; } }

}
