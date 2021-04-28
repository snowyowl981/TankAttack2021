using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public TMP_Text roomNameText;
    public TMP_Text connectInfoText;
    public Button exitButton;

    void Awake()
    {   
        // PhotonNetwork.IsMessageQueueRunning = true;
        Vector3 pos = new Vector3(Random.Range(-200f, 200f), 5.0f, Random.Range(-130f, 130f));

        // 주인공 탱크 생성
        PhotonNetwork.Instantiate("Tank", pos, Quaternion.identity, 0);
    }

    void Start()
    {
        SetRoomInfo();
    }

    void SetRoomInfo()
    {
        Room currentRoom = PhotonNetwork.CurrentRoom;
        roomNameText.text = currentRoom.Name;
        connectInfoText.text = $"{currentRoom.PlayerCount}/{currentRoom.MaxPlayers}";
    }

    // Exit 버튼 클릭 연결
    public void OnExitClick()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        // Lobby 씬 되돌아가기(이미 로비 상태이므로 로비 씬만 호출)
        SceneManager.LoadScene("Lobby");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        SetRoomInfo();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        SetRoomInfo();
    }
}
