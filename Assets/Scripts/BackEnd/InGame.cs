using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using BackEnd.Tcp;
using LitJson;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGame : MonoBehaviour
{
    static InGame instance;

    private int[] grade;

    public static InGame GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("�ν��Ͻ��� �������� ����");
            return null;
        }

        return instance;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    MatchInGameRoomInfo currentGameRoomInfo;
    Dictionary<string, MatchUserGameRecord> inGameUserList = new Dictionary<string, MatchUserGameRecord>();

    private void LeaveMatchMaking()
    {
        Backend.Match.OnLeaveMatchMakingServer = (LeaveChannelEventArgs args) =>
        {
            Debug.Log("OnLeaveMatchMakingServer - ��Ī ���� ���� ���� : " + args.ToString());
        };

        Debug.Log($"5-a. LeaveMatchMakingServer ��ġ����ŷ ���� ���� ���� ��û");
    }

    public void JoinGameServer(MatchInGameRoomInfo gameRoomInfo)
    {
        Backend.Match.OnSessionJoinInServer = (JoinChannelEventArgs args) =>
        {
            if (args.ErrInfo == ErrorInfo.Success)
            {
                Debug.Log("4-2. OnSessionJoinInServer ���� ���� ���� ���� : " + args.ToString());
                Debug.Log("���� ���ӹ濡 �����մϴ�");
                JoinGameRoom();
            }
            else
            {
                Debug.LogError("4-2. OnSessionJoinInServer ���� ���� ���� ���� : " + args.ToString());
            }
        };

        Debug.Log("4-1. JoinGameServer �ΰ��� ���� ���� ��û");

        currentGameRoomInfo = gameRoomInfo;
        ErrorInfo errorInfo = null;

        if (Backend.Match.JoinGameServer(currentGameRoomInfo.m_inGameServerEndPoint.m_address, currentGameRoomInfo.m_inGameServerEndPoint.m_port, false, out errorInfo) == false)
        {
            Debug.LogError("JoinGameServer �� ���� ������ �߻��߽��ϴ�." + errorInfo);
            return;
        }
    }

    public void JoinGameRoom()
    {
        Backend.Match.OnSessionListInServer = (MatchInGameSessionListEventArgs args) =>
        {
            if (args.ErrInfo == ErrorCode.Success)
            {
                Debug.Log("5-2. OnSessionListInServer ���ӷ� ���� ���� : " + args.ToString());

                foreach (var list in args.GameRecords)
                {
                    if (inGameUserList.ContainsKey(list.m_nickname))
                    {
                        continue;
                    }

                    inGameUserList.Add(list.m_nickname, list);

                    ///inGameUserList[0].m_isSuperGamer
                }

            }
            else
            {
                Debug.LogError("5-2. OnSessionListInServer : " + args.ToString());
            }
        };

        Backend.Match.OnMatchInGameAccess = (MatchInGameSessionEventArgs args) =>
        {
            if (args.ErrInfo == ErrorCode.Success)
            {
                Debug.Log($"5-3. OnMatchInGameAccess - ������ �����߽��ϴ� : {args.GameRecord.m_nickname}({args.GameRecord.m_sessionId})");
                if (!inGameUserList.ContainsKey(args.GameRecord.m_nickname))
                {
                    inGameUserList.Add(args.GameRecord.m_nickname, args.GameRecord);
                }
            }
            else
            {
                Debug.LogError("5-3. OnMatchInGameAccess : " + args.ErrInfo.ToString());
            }
        };

        Backend.Match.OnMatchInGameStart = () =>
        {
            string userListString = "������ ���� : \n";
            foreach (var list in inGameUserList)
            {
                userListString += $"{list.Value.m_nickname}({list.Value.m_sessionId})" + (list.Value.m_isSuperGamer == true ? "���۰��̸�" : "");
            }

            Debug.Log("6-1. OnMatchInGameStart �ΰ��� ����");
            Debug.Log(userListString);
            Debug.Log("�����͸� ���� �� �ֽ��ϴ�!");
        };

        Debug.Log($"5-1. JoinGameRoom ���ӷ� ���� ��û : ��ū({currentGameRoomInfo.m_inGameRoomToken}");
        Backend.Match.JoinGameRoom(currentGameRoomInfo.m_inGameRoomToken);

        //Game �� �ε�
        SceneManager.LoadScene(1);
    }

    // �������� ������
    public class Message
    {
        public float height;

        public override string ToString()
        {
            return height.ToString();
        }
    }

    public void SendData(float height)
    {
        if (Backend.Match.OnMatchRelay == null)
        {
            Backend.Match.OnMatchRelay = (MatchRelayEventArgs args) =>
            {
                var strByte = System.Text.Encoding.Default.GetString(args.BinaryUserData);
                Message msg = JsonUtility.FromJson<Message>(strByte);
                Debug.Log($"�������� ���� ������ : {args.From.NickName} : {msg.ToString()}");


                Debug.Log(args.From.NickName + "���� ���� : " + msg.height + "m �Դϴ�");
                GameManager.Instance.GetMessage(args.From.NickName, msg.height);
            };
        }

        Message message = new Message();
        message.height = height;

        var jsonData = JsonUtility.ToJson(message); // Ŭ������ json���� ��ȯ���ִ� �Լ�
        var dataByte = System.Text.Encoding.UTF8.GetBytes(jsonData); // json�� byte[]�� ��ȯ���ִ� �Լ�
        Backend.Match.SendDataToInGameRoom(dataByte);
    }

    public void MatchEnd()
    {
        Backend.Match.OnLeaveInGameServer = (MatchInGameSessionEventArgs args) =>
        {
            if (args.ErrInfo == ErrorCode.Success)
            {
                Debug.Log("OnLeaveInGameServer �ΰ��� ���� ���� ���� : " + args.ErrInfo.ToString());
            }
            else
            {
                Debug.LogError("OnLeaveInGameServer �ΰ��� ���� ���� ���� : " + args.ErrInfo + " / " + args.Reason);
            }
        };

        Backend.Match.OnMatchResult = (MatchResultEventArgs args) =>
        {
            if (args.ErrInfo == ErrorCode.Success)
            {
                Debug.Log("8-2. OnMatchResult ���� : " + args.ErrInfo.ToString());
            }
            else
            {
                Debug.LogError("8-2. OnMatchResult ���� : " + args.ErrInfo.ToString());
            }
        };
        Debug.Log("8-1. MatchEnd ȣ��");
        MatchGameResult matchGameResult = new MatchGameResult();
        matchGameResult.m_winners = new List<SessionId>();

        foreach (var session in inGameUserList)
        {
            // ������ �����մϴ�.
            matchGameResult.m_winners.Add(session.Value.m_sessionId);
        }

        Backend.Match.MatchEnd(matchGameResult);
    }
}