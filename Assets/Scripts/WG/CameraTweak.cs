using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTweak : MonoBehaviour
{
    CinemachineVirtualCamera vcam;
    void Awake()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
    }
    void Start()
    {
        vcam.LookAt = GameObject.FindWithTag("Player").transform;
        vcam.Follow = GameObject.FindWithTag("Player").transform;
    }
}
