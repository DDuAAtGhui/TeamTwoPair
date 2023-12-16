using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using BackEnd.Tcp;
using LitJson;
using System;
using UnityEngine.UI;

public class LogIn : MonoBehaviour
{
    private Match _match;

    [SerializeField]
    private string nickname;

    [SerializeField]
    private GameObject loginButton;
    [SerializeField]
    private GameObject createRoomButton;
    [SerializeField]
    private GameObject searchRoomButton;



    void Start()
    {
        nickname = "PlayerZ";
        loginButton.SetActive(true);
        createRoomButton.SetActive(false);
        searchRoomButton.SetActive(false);

        var bro = Backend.Initialize();
        Debug.Log("�ʱ�ȭ ��� : " + bro);
    }

    public void SettingNickName()
    {
        var bro = Backend.BMember.UpdateNickname(nickname);

        if (bro.IsSuccess())
        {
            Debug.Log("�г��� ���� : " + bro);
        }
        else
        {
            Debug.LogError("�г��� ���� : " + bro);
        }
    }

    public void GuestLogIn()
    {
        var bro = Backend.BMember.CustomLogin(nickname, nickname);
        Debug.Log(nickname);

        if (bro.IsSuccess())
        {
            Debug.Log("�α��� : " + bro);

            loginButton.SetActive(false);
            createRoomButton.SetActive(true);
            searchRoomButton.SetActive(true);

            _match = GetComponent<Match>();
            _match.JoinMatchMakingServer();
        }
        else
        {
            var _bro = Backend.BMember.CustomSignUp(nickname, nickname);
            if (_bro.IsSuccess())
            {
                SettingNickName();
                Debug.Log("ȸ������ : " + _bro);
            }
            else
            {
                Debug.LogError("ȸ������ : " + _bro);
            }
        }
    }
}
