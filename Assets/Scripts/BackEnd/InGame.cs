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

    public bool isInGame = false;

    private int[] grade;

    public HashSet<string> deadPlayerCheck = new HashSet<string>();
    public Stack<SessionId> deadmanStack = new Stack<SessionId>();
    public Dictionary<string, PlayerData> playerDataDic = new Dictionary<string, PlayerData>();
    public class PlayerData
    {
        public string nickname;
        public SessionId sessionId;
        public float height;
    }

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

    private void Update()
    {
        if (GameManager.instance && GameManager.instance.isWin)
        {
            SendImWin();
        }
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
        isInGame = true;
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
        public int action;
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
                GameManager.instance.GetMessage(args.From.NickName, msg.height);

                if (msg.action == 0)
                {
                    if (deadPlayerCheck.Contains(args.From.NickName))
                    {
                        Debug.Log($"{args.From.NickName}���� ĳ���� �Դϴ�.");
                        return;
                    }

                    if (playerDataDic.ContainsKey(args.From.NickName))
                    {
                        playerDataDic[args.From.NickName].height = msg.height;

                        Debug.Log($"�÷��̾� ��ġ ������Ʈ : {args.From.NickName} : " + msg.height);
                    }
                    else
                    {

                        PlayerData playerData = new PlayerData();
                        playerData.nickname = args.From.NickName;
                        playerData.sessionId = args.From.SessionId;
                        playerData.height = msg.height;

                        playerDataDic.Add(args.From.NickName, playerData);
                        Debug.Log("�÷��̾� ���� �Ҵ� : " + playerData.nickname);
                    }
                }
                else if (msg.action == 1)
                {
                    deadPlayerCheck.Add(args.From.NickName);
                    deadmanStack.Push(args.From.SessionId);
                    playerDataDic.Remove(args.From.NickName);
                }
                else if (msg.action == 2)
                {
                    isInGame = false;
                    MatchEnd(args.From.SessionId);
                }
                else
                {
                    Debug.LogError("���� �Ҵ���� �ʴ� msg �Դϴ�" + msg.ToString());
                }
            };
        }

        Debug.Log("����");

        Message message = new Message();
        message.height = height;

        message.action = 0;
        var jsonData = JsonUtility.ToJson(message); // Ŭ������ json���� ��ȯ���ִ� �Լ�
        var dataByte = System.Text.Encoding.UTF8.GetBytes(jsonData); // json�� byte[]�� ��ȯ���ִ� �Լ�
        Backend.Match.SendDataToInGameRoom(dataByte);
    }

    public void SendImDead()
    {
        Message message = new Message();
        message.action = 1;

        var jsonData = JsonUtility.ToJson(message); // Ŭ������ json���� ��ȯ���ִ� �Լ�
        var dataByte = System.Text.Encoding.UTF8.GetBytes(jsonData); // json�� byte[]�� ��ȯ���ִ� �Լ�
        Backend.Match.SendDataToInGameRoom(dataByte);
    }

    public void SendImWin()
    {
        Message message = new Message();
        message.action = 2;

        var jsonData = JsonUtility.ToJson(message); // Ŭ������ json���� ��ȯ���ִ� �Լ�
        var dataByte = System.Text.Encoding.UTF8.GetBytes(jsonData); // json�� byte[]�� ��ȯ���ִ� �Լ�
        Backend.Match.SendDataToInGameRoom(dataByte);
        isInGame = false;
    }

    public void MatchEnd(SessionId winnerSessionId)
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

            // GIF �Ѹ���
            //SceneManager.LoadScene("HY");
            StopAllCoroutines();
        };

        Debug.Log("8-1. MatchEnd ȣ��");

        MatchGameResult matchGameResult = new MatchGameResult();
        matchGameResult.m_winners = new List<SessionId>();



        while (playerDataDic.Count > 0)
        {
            float tempHeight = 0;
            string tempNick = "";

            foreach (var playerData in playerDataDic)
            {
                Debug.Log($"��� : {playerData.Key} : {playerData.Value.height}");
                if (playerData.Value.height > tempHeight)
                {
                    tempHeight = playerData.Value.height;
                    tempNick = playerData.Value.nickname;
                }
            }

            matchGameResult.m_winners.Add(playerDataDic[tempNick].sessionId);
            playerDataDic.Remove(tempNick);
        }

        while (deadmanStack.Count > 0)
        {
            matchGameResult.m_winners.Add(deadmanStack.Pop());
        }

        Backend.Match.MatchEnd(matchGameResult);

        
        //�ִϸ��̼� ���
    }
}