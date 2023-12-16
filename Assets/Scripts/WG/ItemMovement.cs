using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum MovementType { Hovering = 1, Patrol, Linear, Wave }
public class ItemMovement : MonoBehaviour
{
    [SerializeField]
    MovementType movementType;
    [SerializeField] ItemData itemData;

    void Start()
    {
        switch (movementType)
        {
            case MovementType.Hovering:
                break;
        }
    }

    void Update()
    {

    }
}
