using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Damage : MonoBehaviour
{
    public int hp = 100;
    private PhotonView pv;

    void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    void OnCollisionEnter(Collision coll)
    {
        if(coll.collider.CompareTag("CANNON"))
        {
            string shooter = coll.gameObject.GetComponent<Cannon>().shooter;
            hp -= 10;
            if(hp <= 0)
            {
                string msg = $"\n<color=#00ff00>{pv.Owner.NickName}</color> is killed by <color=#ff0000>{shooter}</color>";
                GameManager.instance.messageText.text += msg;
            }
        }
    }
}
