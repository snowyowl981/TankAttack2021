using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    void Awake()
    {   
        // PhotonNetwork.IsMessageQueueRunning = true;
        Vector3 pos = new Vector3(Random.Range(-200f, 200f), 5.0f, Random.Range(-200f, 200f));

        // 주인공 탱크 생성
        PhotonNetwork.Instantiate("Tank", pos, Quaternion.identity, 0);
    }
}
