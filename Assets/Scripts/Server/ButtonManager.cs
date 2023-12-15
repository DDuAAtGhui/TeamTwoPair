using BackEnd;
using BackEnd.Tcp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public voidCreateMatchRoom(MatchMakingInteractionEventArgs args)
    {
        Backend.Match.CreateMatchRoom();
    }
}
