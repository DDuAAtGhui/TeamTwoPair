using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPController : MonoBehaviour
{
    int maxHP = 100;
    int currentHP = 0;

    void Start()
    {
        currentHP = maxHP;
    }



    public int MaxHP { get { return maxHP; } set { maxHP = value; } }
    public int CurrentHP { get { return currentHP; } }

}
