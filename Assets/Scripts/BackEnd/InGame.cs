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
            Debug.LogError("인스턴스가 존재하지 않음");
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
            Debug.Log("OnLeaveMatchMakingServer - 매칭 서버 접속 종료 : " + args.ToString());
        };

        Debug.Log($"5-a. LeaveMatchMakingServer 매치메이킹 서버 접속 종료 요청");
    }

    public void JoinGameServer(MatchInGameRoomInfo gameRoomInfo)
    {
        Backend.Match.OnSessionJoinInServer = (JoinChannelEventArgs args) =>
        {
            if (args.ErrInfo == ErrorInfo.Success)
            {
                Debug.Log("4-2. OnSessionJoinInServer 게임 서버 접속 성공 : " + args.ToString());
                Debug.Log("이제 게임방에 접속합니다");
                JoinGameRoom();
            }
            else
            {
                Debug.LogError("4-2. OnSessionJoinInServer 게임 서버 접속 실패 : " + args.ToString());
            }
        };

        Debug.Log("4-1. JoinGameServer 인게임 서버 접속 요청");

        currentGameRoomInfo = gameRoomInfo;
        ErrorInfo errorInfo = null;

        if (Backend.Match.JoinGameServer(currentGameRoomInfo.m_inGameServerEndPoint.m_address, currentGameRoomInfo.m_inGameServerEndPoint.m_port, false, out errorInfo) == false)
        {
            Debug.LogError("JoinGameServer 중 로컬 에러가 발생했습니다." + errorInfo);
            return;
        }
    }

    public void JoinGameRoom()
    {
        Backend.Match.OnSessionListInServer = (MatchInGameSessionListEventArgs args) =>
        {
            if (args.ErrInfo == ErrorCode.Success)
            {
                Debug.Log("5-2. OnSessionListInServer 게임룸 접속 성공 : " + args.ToString());

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
                Debug.Log($"5-3. OnMatchInGameAccess - 유저가 접속했습니다 : {args.GameRecord.m_nickname}({args.GameRecord.m_sessionId})");
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
            string userListString = "접속한 유저 : \n";
            foreach (var list in inGameUserList)
            {
                userListString += $"{list.Value.m_nickname}({list.Value.m_sessionId})" + (list.Value.m_isSuperGamer == true ? "슈퍼게이머" : "");
            }

            Debug.Log("6-1. OnMatchInGameStart 인게임 시작");
            Debug.Log(userListString);
            Debug.Log("데이터를 보낼 수 있습니다!");
        };

        Debug.Log($"5-1. JoinGameRoom 게임룸 접속 요청 : 토큰({currentGameRoomInfo.m_inGameRoomToken}");
        Backend.Match.JoinGameRoom(currentGameRoomInfo.m_inGameRoomToken);

        //Game 씬 로드
        SceneManager.LoadScene(1);
    }

    // 릴레이할 데이터
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
                Debug.Log($"서버에서 받은 데이터 : {args.From.NickName} : {msg.ToString()}");


                Debug.Log(args.From.NickName + "님은 현재 : " + msg.height + "m 입니다");
                GameManager.Instance.GetMessage(args.From.NickName, msg.height);
            };
        }

        Message message = new Message();
        message.height = height;

        var jsonData = JsonUtility.ToJson(message); // 클래스를 json으로 변환해주는 함수
        var dataByte = System.Text.Encoding.UTF8.GetBytes(jsonData); // json을 byte[]로 변환해주는 함수
        Backend.Match.SendDataToInGameRoom(dataByte);
    }

    public void MatchEnd()
    {
        Backend.Match.OnLeaveInGameServer = (MatchInGameSessionEventArgs args) =>
        {
            if (args.ErrInfo == ErrorCode.Success)
            {
                Debug.Log("OnLeaveInGameServer 인게임 서버 접속 종료 : " + args.ErrInfo.ToString());
            }
            else
            {
                Debug.LogError("OnLeaveInGameServer 인게임 서버 접속 종료 : " + args.ErrInfo + " / " + args.Reason);
            }
        };

        Backend.Match.OnMatchResult = (MatchResultEventArgs args) =>
        {
            if (args.ErrInfo == ErrorCode.Success)
            {
                Debug.Log("8-2. OnMatchResult 성공 : " + args.ErrInfo.ToString());
            }
            else
            {
                Debug.LogError("8-2. OnMatchResult 실패 : " + args.ErrInfo.ToString());
            }
        };
        Debug.Log("8-1. MatchEnd 호출");
        MatchGameResult matchGameResult = new MatchGameResult();
        matchGameResult.m_winners = new List<SessionId>();

        foreach (var session in inGameUserList)
        {
            // 순서는 무관합니다.
            matchGameResult.m_winners.Add(session.Value.m_sessionId);
        }

        Backend.Match.MatchEnd(matchGameResult);
    }
}