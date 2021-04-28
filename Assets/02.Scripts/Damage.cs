using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Damage : MonoBehaviour
{
    private List<MeshRenderer> renderers = new List<MeshRenderer>();
    public int hp = 100;
    private PhotonView pv;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        GetComponentsInChildren<MeshRenderer>(renderers);
    }

    void OnCollisionEnter(Collision coll)
    {
        if(coll.collider.CompareTag("CANNON"))
        {
            string shooter = coll.gameObject.GetComponent<Cannon>().shooter;
            hp -= 10;
            if(hp <= 0)
            {
                StartCoroutine(TankDestroy(shooter));
            }
           // Destroy(coll.gameObject);
        }
    }

    IEnumerator TankDestroy(string shooter)
    {
        string msg = $"\n<color=#00ff00>{pv.Owner.NickName}</color> is killed by <color=#ff0000>{shooter}</color>";
        GameManager.instance.messageText.text += msg;

        // 발사로직 정지


        // 랜더러 컴포넌트 비활성화
        GetComponent<BoxCollider>().enabled = false;
        if(pv.IsMine)
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
        foreach(var mesh in renderers)
        {
            mesh.enabled = false;
        }

        // 5초 Wating
        yield return new WaitForSeconds(5f);

        // 불규칙한 위치로 이동
        Vector3 pos = new Vector3(Random.Range(-200f, 200f), 5.0f, Random.Range(-100f, 100f));
        transform.position = pos;

        // 랜더러 컴포넌트 활성화
        hp = 100;
        GetComponent<BoxCollider>().enabled = true;
        if(pv.IsMine)
        {
            GetComponent<Rigidbody>().isKinematic = false;
        }
        foreach(var mesh in renderers)
        {
            mesh.enabled = true;
        }
    }
}
