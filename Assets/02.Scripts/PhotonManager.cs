using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    // 서로 다른 버전이 같이 플레이 할 경우 버그 발생 가능성 있음
    // 1.0 버전은 1.0 버전끼리 매칭
    private readonly string gameVersion = "v1.0";
    private string UserId = "SnowyOwl";

    void Awake()
    {   
        // 게임 버전 지정
        PhotonNetwork.GameVersion = gameVersion;

        // 닉네임 지정
        PhotonNetwork.NickName = UserId;

        // 서버접속(UsingSettings는 가장 빠른 서버)
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Server");
        PhotonNetwork.JoinRandomRoom(); // 랜덤한 방 접속
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"code = {returnCode}, msg = {message}"); // 입장 실패
        PhotonNetwork.CreateRoom("My Room");
    }

    // 방 생성 완료 콜백
    public override void OnCreatedRoom()
    {
        Debug.Log("방 생성 완료");
    }

    // 방 입장 시 호출되는 콜백함수
    public override void OnJoinedRoom()
    {
        Debug.Log("방 입장 완료");
        Debug.Log(PhotonNetwork.CurrentRoom.Name);

        // 통신이 가능한 주인공 캐릭터(탱크) 생성
        PhotonNetwork.Instantiate("Tank", new Vector3(0, 5.0f, 0), Quaternion.identity, 0);
    }

}
