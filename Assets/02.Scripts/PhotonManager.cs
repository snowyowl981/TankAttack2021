using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    // 서로 다른 버전이 같이 플레이 할 경우 버그 발생 가능성 있음
    // 1.0 버전은 1.0 버전끼리 매칭
    private readonly string gameVersion = "v1.0";
    private string userId = "SnowyOwl";

    public TMP_InputField userIdText;
    public TMP_InputField roomNameText;

    // 룸 목록 저장을 위한 자료형
    private Dictionary<string, GameObject> roomDict = new Dictionary<string, GameObject>();

    // 룸을 표시할 프리팹
    public GameObject roomPrefab;

    // Room 프리팹이 차일드화 시킬 부모객체
    public Transform scrollContent;
    void Awake()
    {   
        // 자동으로 씬 로딩
        PhotonNetwork.AutomaticallySyncScene = true;

        // 게임 버전 지정
        PhotonNetwork.GameVersion = gameVersion;

        // 닉네임 지정
        //PhotonNetwork.NickName = userId;

        // 서버접속(UsingSettings는 가장 빠른 서버)
        PhotonNetwork.ConnectUsingSettings();
    }

    void Start()
    {
        // 닉네임 있으면 가져오고 없으면 랜덤 세팅
        userId = PlayerPrefs.GetString("USER_ID", $"USER_{Random.Range(0, 100):00}");
        userIdText.text = userId;
        PhotonNetwork.NickName = userId;
    }
    
    // 서버에 접속함
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Server");
        // PhotonNetwork.JoinRandomRoom(); // 랜덤한 방 접속

        // 로비에 접속
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joinned Lobby");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"code = {returnCode}, msg = {message}"); // 입장 실패

        // 룸 옵션 설정
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 30;

        if(string.IsNullOrEmpty(roomNameText.text))
        {
            roomNameText.text = $"ROOM_{Random.Range(0, 100):000}";
        }

        // 룸을 생성
        PhotonNetwork.CreateRoom(roomNameText.text, ro);
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

        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("BattleField");
        }

        // 통신이 가능한 주인공 캐릭터(탱크) 생성
        //PhotonNetwork.Instantiate("Tank", new Vector3(0, 5.0f, 0), Quaternion.identity, 0);
    }

    // 룸 목록이 변경(갱신)될때마다 호출
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        GameObject tempRoom = null;

        foreach(var room in roomList)
        {
            // Debug.Log($"Room = {room.Name}, ({room.PlayerCount}/{room.MaxPlayers})");
            // 룸 삭제된 경우 => 
            if(room.RemovedFromList)
            {
                // 딕셔너리에서 삭제, 프리팹 삭제
                roomDict.TryGetValue(room.Name, out tempRoom);

                // RoomItem 프리팹 삭제
                Destroy(tempRoom);

                // 딕셔너리에서 데이터 삭제
                roomDict.Remove(room.Name);
                
            }
            else // 룸 정보가 갱신 또는 변경된 경우
            {
                // 처음 생성된 경우 딕셔너리에 데이터 추가 + roomItem 생성
                if(roomDict.ContainsKey(room.Name) == false)
                {
                    GameObject _room = Instantiate(roomPrefab, scrollContent);
                    // 룸 정보 
                    _room.GetComponent<RoomData>().RoomInfo = room;
                    // 딕셔너리에 저장
                    roomDict.Add(room.Name, _room);
                }
                else
                {
                    // 룸 정보를 갱신
                    roomDict.TryGetValue(room.Name, out tempRoom);
                    tempRoom.GetComponent<RoomData>().RoomInfo = room;
                }
            }
        }
    }

#region UI_BUTTON_CALLBACK
    // 로그인 버튼 누르면 아무 랜덤방에 접속
    public void OnLoginClick()
    {
        if(string.IsNullOrEmpty(userIdText.text))
        {
            userId = $"USER_{Random.Range(0, 100):00}";
            userIdText.text = userId;
        }

        PlayerPrefs.SetString("USER_ID", userIdText.text);
        PhotonNetwork.NickName = userIdText.text;
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnMakeRoomClick()
    {
         // 룸 옵션 설정
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 30;

        if(string.IsNullOrEmpty(roomNameText.text))
        {
            roomNameText.text = $"Room_{Random.Range(0, 100):000}";
        }

        // 룸을 생성과 동시에 생성한 방에 입장
        PhotonNetwork.CreateRoom(roomNameText.text, ro);
    }

#endregion
}
